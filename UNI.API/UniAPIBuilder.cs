using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.API
{
	public static class UniAPIBuilder
	{
		public static IHostBuilder CreateUniAPIBuilder<T>(string[] args) where T : UniAPIStartup
		{
			var builder = Host.CreateDefaultBuilder(args);
			builder.ConfigureAppConfiguration((context, config) =>
			{
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
