using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Uni.API
{
	/// <summary>
	/// A default <seealso cref="WebApplicationBuilder"/> for the Uni.API
	/// </summary>
	public static class UniAPIBuilder
	{
		/// <summary>
		/// Create a new instance of a <seealso cref="WebApplicationBuilder"/> based on the startup <typeparamref name="T"/>
		/// This also loads a json config file with the nae 'configuration.json', if it exists.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args"></param>
		/// <param name="configFile"></param>
		/// <returns></returns>
		public static WebApplicationBuilder CreateUniAPIBuilder<T>(string[] args, string configFile = "configuration.json") where T : UniAPIStartup
		{
			var builder = WebApplication.CreateBuilder(args);
			if (File.Exists(configFile))
				builder.Configuration.AddJsonFile(configFile);
			builder.Host.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<T>();
			});
			return builder;
		}
	}
}
