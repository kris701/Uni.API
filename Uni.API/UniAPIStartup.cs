using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;
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
		/// <summary>
		/// Default namespace to find plugins in.
		/// </summary>
		public static string DefaultPluginNamespace = "Uni.API.Plugins";
		/// <summary>
		/// Configuration for the API.
		/// </summary>
		public IConfiguration Configuration { get; }
		/// <summary>
		/// List of plugin namespaces to look for plugins in.
		/// </summary>
		public List<string> PluginNamespaces { get; set; }

		private readonly List<IUniAPIPlugin> _plugins;
		private readonly ILogger<UniAPIStartup> _logger;

		/// <summary>
		/// Constructor with override for plugin namespaces
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="pluginNamespace"></param>
		public UniAPIStartup(IConfiguration configuration, List<string> pluginNamespace)
		{
			_plugins = new List<IUniAPIPlugin>();
			Configuration = configuration;
			PluginNamespaces = pluginNamespace;
			using var loggerFactory = LoggerFactory.Create(builder =>
			{
				builder.SetMinimumLevel(LogLevel.Information);
				builder.AddConsole();
				builder.AddEventSourceLogger();
			});
			_logger = loggerFactory.CreateLogger<UniAPIStartup>();
			LoadPlugins(Configuration);
		}

		/// <summary>
		/// Main constructor
		/// </summary>
		/// <param name="configuration"></param>
		[ActivatorUtilitiesConstructor]
		public UniAPIStartup(IConfiguration configuration) : this(configuration, new List<string>() { DefaultPluginNamespace })
		{
		}

		private void LoadPlugins(IConfiguration configuration)
		{
			_logger.LogInformation("Getting target plugin list...");
			var toUse = configuration.GetSection("UsePlugins").Get<List<string>>();
			if (toUse == null)
				toUse = new List<string>();
			if (toUse.Count == 0)
			{
				_logger.LogWarning("No plugins is set to load");
				return;
			}

			_logger.LogInformation($"Plugin namespaces to search ({PluginNamespaces.Count} in total):");
			foreach (var nameSpace in PluginNamespaces)
				_logger.LogInformation($"\t{nameSpace}");

			_logger.LogInformation("Getting all assemblies in current domain...");
			var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
			var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();
			_logger.LogInformation($"A total of {loadedAssemblies.Count} assemblies exist");

			_logger.LogInformation("Removing all from the list that is not in the plugin namespace...");
			var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").ToList();
			_logger.LogInformation($"A total of {referencedPaths.Count} assemblies referenced");
			referencedPaths.RemoveAll(x => !PluginNamespaces.Any(y => x.Contains(y)));
			_logger.LogInformation($"A total of {referencedPaths.Count} assemblies referenced that is within a plugin namespace.");
			var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).Select(x => new FileInfo(x)).ToList();
			_logger.LogInformation($"A total of {toLoad.Count} plugin assemblies to load");

			_logger.LogInformation("Removing all from the list that is not in the plugin list...");
			toLoad.RemoveAll(x => !toUse.Any(y => x.Name.EndsWith($"{y}.dll")));
			var orderedToLoad = new List<FileInfo>();
			foreach (var target in toUse)
				orderedToLoad.Add(toLoad.First(x => x.Name.EndsWith($"{target}.dll")));

			_logger.LogInformation($"A total of {orderedToLoad.Count} plugin assemblies to load.");
			orderedToLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path.FullName))));
			if (toUse.Count != orderedToLoad.Count)
				_logger.LogWarning("Not all targeted plugins could be found!");

			// Instantiate Plugins
			_logger.LogInformation("Instantiating all plugins...");
			List<Type> plugins = new List<Type>();
			foreach (var nameSpace in PluginNamespaces)
				plugins.AddRange(GetTypesInNamespace(nameSpace));
			plugins.RemoveAll(x => !x.IsAssignableTo(typeof(IUniAPIPlugin)));
			foreach (var type in plugins)
			{
				if (type.Namespace == null)
					continue;
				if (Activator.CreateInstance(type) is IUniAPIPlugin plugin)
				{
					plugin.NameSpace = type.Namespace;
					_plugins.Add(plugin);
				}
			}

			_logger.LogInformation($"A total of {_plugins.Count} plugins instantiated");

			_logger.LogInformation($"Checking if plugin requirements are present");
			for (int i = 0; i < _plugins.Count; i++)
			{
				var plugin = _plugins[i];
				if (plugin.Requires.Count > 0)
				{
					var previous = _plugins.GetRange(0, i);
					if (!plugin.Requires.All(x => previous.Any(y => y.ID == x)))
						throw new Exception($"Bad load order detected! Plugin '{plugin.Name}' is missing required plugins: {string.Join(',', plugin.Requires.Where(x => !previous.Any(y => y.ID == x)))}! Reorder the plugins so that the required plugins are loaded before this plugin.");
				}
			}

			// Allow the plugins to configure themselfs
			_logger.LogInformation($"Configuring all plugins");
			foreach (var plugin in _plugins)
				plugin.ConfigureConfiguration(configuration);
			_logger.LogInformation($"Uni API plugin loading complete!");
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
			}).ConfigureApplicationPartManager(manager =>
			{
				var existing = manager.FeatureProviders.OfType<ControllerFeatureProvider>().FirstOrDefault();
				if (existing != null)
					manager.FeatureProviders.Remove(existing);
				var allowed = _plugins.Select(x => x.NameSpace).ToList();
				allowed.Add("Uni.API.Controllers");
				manager.FeatureProviders.Add(new CustomControllerFeatureProvider(allowed));
			}).AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
				options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			});

			services.AddSingleton(new PluginsModel(_plugins));

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
