﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Reflection;
using UNI.API.Exceptions;
using UNI.API.Models;
using UNI.API.Services;

namespace UNI.API
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		private readonly string _pluginNamespace = "Helvion.Helvware.Plugin";
		private List<IHelvwarePlugin> _plugins;

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			LoadPlugins(configuration);
		}

		private void LoadPlugins(IConfiguration configuration)
		{
			_plugins = new List<IHelvwarePlugin>();

			Console.WriteLine("Getting target plugin list...");
			var toUse = configuration.GetSection("UsePlugins").Get<List<string>>();
			if (toUse == null)
				toUse = new List<string>();
			if (toUse.Count == 0)
			{
				Console.WriteLine("No plugins is set to load");
				return;
			}

			Console.WriteLine("Getting all assemblies in current domain...");
			var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
			var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();
			Console.WriteLine($"A total of {loadedAssemblies.Count} assemblies exist");

			Console.WriteLine("Removing all from the list that is not in the plugin namespace...");
			var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").ToList();
			referencedPaths.RemoveAll(x => !x.Contains(_pluginNamespace));
			var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

			Console.WriteLine("Removing all from the list that is not in the plugin list...");
			toLoad.RemoveAll(x => !toUse.Any(y => x.EndsWith($"{y}.dll")));

			Console.WriteLine($"A total of {toLoad.Count} plugin assemblies to load.");
			toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

			// Instantiate Plugins
			Console.WriteLine("Instantiating all plugins...");
			var plugins = GetTypesInNamespace(_pluginNamespace);
			plugins.RemoveAll(x => !x.IsAssignableTo(typeof(IHelvwarePlugin)));
			foreach (var type in plugins)
				if (Activator.CreateInstance(type) is IHelvwarePlugin plugin)
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

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Helvware API", Version = "v1" });
				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
			});

			services.AddControllers(options =>
			{
				options.Filters.Add(new BaseExceptionFilter());
				options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
				options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
				options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "The field is required");
			});

			services.AddSingleton(new PluginsService(_plugins));

			foreach (var plugin in _plugins)
				plugin.ConfigureServices(services);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
