using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Uni.API
{
	/// <summary>
	/// A default <seealso cref="WebApplication"/> for the Uni.API
	/// </summary>
	public static class UniAPIBuilder
	{
		/// <summary>
		/// Create a new instance of a <seealso cref="WebApplication"/> based on the startup <typeparamref name="T"/>
		/// This also loads a json config file with the nae 'configuration.json', if it exists.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args"></param>
		/// <param name="configFile"></param>
		/// <returns></returns>
		public static WebApplication CreateUniAPIApplication<T>(string[] args, string configFile = "configuration.json") where T : UniAPIStartup
		{
			var builder = WebApplication.CreateBuilder(args);
			if (File.Exists(configFile))
				builder.Configuration.AddJsonFile(configFile);

			var startup = Activator.CreateInstance<T>();

			startup.LoadPlugins(builder.Configuration);

			startup.ConfigureServices(builder.Services, builder.Configuration);

			var app = builder.Build();

			startup.Configure(app, app.Environment);

			return app;
		}
	}
}
