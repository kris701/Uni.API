using ExampleAPI.Plugins.ExamplePlugin.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uni.API.Models;

namespace ExampleAPI.Plugins.ExamplePlugin
{
	public class ExamplePluginPlugin : IUniAPIPlugin
	{
		public Guid ID { get; } = new Guid("b3590575-5c45-472f-9a0a-fb30fc39378a");
		public string Name { get; } = "Example Plugin";
		public bool IsActive { get; private set; } = false;

		private string _someImportantConfigValue = "";

		public void ConfigureConfiguration(IConfiguration configuration)
		{
			// Do whatever configuration you need
			_someImportantConfigValue = configuration.GetSection("ExamplePluginSetup").GetValue<string>("value1");
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// Configure plugin services here
			services.AddSingleton(new SomePluginConfiguration()
			{
				Value1 = _someImportantConfigValue
			});
			IsActive = true;
		}
	}
}
