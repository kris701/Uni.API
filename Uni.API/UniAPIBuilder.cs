using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Uni.API
{
	/// <summary>
	/// A default <seealso cref="IHostBuilder"/> for the Uni.API
	/// </summary>
	public static class UniAPIBuilder
	{
		/// <summary>
		/// Create a new instance of a <seealso cref="IHostBuilder"/> based on the startup <typeparamref name="T"/>
		/// This also loads a json config file with the nae 'configuration.json', if it exists.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args"></param>
		/// <returns></returns>
		public static IHostBuilder CreateUniAPIBuilder<T>(string[] args) where T : UniAPIStartup
		{
			var builder = Host.CreateDefaultBuilder(args);
			builder.ConfigureAppConfiguration((context, config) =>
			{
				if (File.Exists("configuration.json"))
					config.AddJsonFile("configuration.json");
			});
			builder.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<T>();
			});
			return builder;
		}
	}
}
