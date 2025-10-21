using ExampleAPI.Plugins.ExamplePlugin.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uni.API.Helpers;
using Uni.API.Models;

namespace ExampleAPI.Plugins.ExamplePlugin
{
	/// <summary>
	/// Simple example plugin, that has a single controller with a single endpoint that returns the value set in the APIs configuration.
	/// </summary>
	public class ExamplePluginPlugin : BaseUniAPIPlugin
	{
		private string _someImportantConfigValue = "";

		public ExamplePluginPlugin() : base(
			new Guid("fb1c42a2-770b-4bb0-a2b6-8f6736857022"),
			"Some example plugin")
		{
		}

		public override void ConfigureConfiguration(IConfiguration configuration)
		{
			// Do whatever configuration you need
			_someImportantConfigValue = configuration.GetSectionValue<string>("ExamplePluginSetup", "value1");
			base.ConfigureConfiguration(configuration);
		}

		public override void ConfigureServices(IServiceCollection services)
		{
			// Configure plugin services here
			services.AddSingleton(new SomePluginConfiguration()
			{
				Value1 = _someImportantConfigValue
			});
			base.ConfigureServices(services);
		}
	}
}
