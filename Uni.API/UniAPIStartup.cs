using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Uni.API.Exceptions;
using Uni.API.Models;
using Uni.API.Services;

namespace Uni.API
{
	/// <summary>
	/// Startup implementation for the Uni API
	/// </summary>
	public class UniAPIStartup
	{
		public static string DefaultPluginNamespace = "Uni.API.Plugins";

		public IConfiguration Configuration { get; }
		public List<string> PluginNamespaces { get; set; } = new List<string>()
		{
			DefaultPluginNamespace
		};

		private readonly List<IUniAPIPlugin> _plugins;

		public UniAPIStartup(IConfiguration configuration)
		{
			_plugins = new List<IUniAPIPlugin>();
			Configuration = configuration;
			LoadPlugins(configuration);
		}

		private void LoadPlugins(IConfiguration configuration)
		{
			Console.WriteLine("Getting target plugin list...");
			var toUse = configuration.GetSection("UsePlugins").Get<List<string>>();
			if (toUse == null)
				toUse = new List<string>();
			if (toUse.Count == 0)
			{
				Console.WriteLine("No plugins is set to load");
				return;
			}

			Console.WriteLine($"Plugin namespaces to search ({PluginNamespaces.Count} in total):");
			foreach (var nameSpace in PluginNamespaces)
				Console.WriteLine($"\t{nameSpace}");

			Console.WriteLine("Getting all assemblies in current domain...");
			var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
			var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();
			Console.WriteLine($"A total of {loadedAssemblies.Count} assemblies exist");

			Console.WriteLine("Removing all from the list that is not in the plugin namespace...");
			var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").ToList();
			Console.WriteLine($"A total of {referencedPaths.Count} assemblies referenced");
			referencedPaths.RemoveAll(x => !PluginNamespaces.Any(y => x.StartsWith(y)));
			var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

			Console.WriteLine("Removing all from the list that is not in the plugin list...");
			toLoad.RemoveAll(x => !toUse.Any(y => x.EndsWith($"{y}.dll")));

			Console.WriteLine($"A total of {toLoad.Count} plugin assemblies to load.");
			toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

			// Instantiate Plugins
			Console.WriteLine("Instantiating all plugins...");
			List<Type> plugins = new List<Type>();
			foreach(var nameSpace in PluginNamespaces)
				plugins.AddRange(GetTypesInNamespace(nameSpace));
			plugins.RemoveAll(x => !x.IsAssignableTo(typeof(IUniAPIPlugin)));
			foreach (var type in plugins)
				if (Activator.CreateInstance(type) is IUniAPIPlugin plugin)
					_plugins.Add(plugin);

			Console.WriteLine($"A total of {_plugins.Count} plugins instantiated");

			// Allow the plugins to configure themselfs
			Console.WriteLine($"Configuring all plugins");
			foreach (var plugin in _plugins)
				plugin.ConfigureConfiguration(configuration);
			Console.WriteLine($"Dynamic loading complete!");
		}

		private List<Type> GetTypesInNamespace(string nameSpace)
		{
			var total = new List<Type>();
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
				if (assembly.FullName != null && assembly.FullName.StartsWith(nameSpace))
					total.AddRange(assembly.GetTypes().Where(t => t.Namespace != null && t.Namespace.StartsWith(nameSpace)));
			return total;
		}

		/// <summary>
		/// Base implementation to configure services
		/// </summary>
		/// <param name="services"></param>
		public virtual void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers(options =>
			{
				options.Filters.Add(new BaseExceptionFilter());
				options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "The field is required");
			});

			services.AddSingleton(new PluginsService(_plugins));

			ConfigurePlugins(services);
		}

		internal void ConfigurePlugins(IServiceCollection services)
		{
			foreach (var plugin in _plugins)
				plugin.ConfigureServices(services);
		}

		/// <summary>
		/// Base implementation to configure the platform
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		/// <param name="loggerFactory"></param>
		public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
