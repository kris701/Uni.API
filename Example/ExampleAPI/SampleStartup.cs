﻿using Microsoft.OpenApi.Models;
using Uni.API;

namespace ExampleAPI
{
	/// <summary>
	/// Simple extension to the default <seealso cref="UniAPIStartup"/> that adds another plugin namespace as well as enabling swagger.
	/// </summary>
	public class SampleStartup : UniAPIStartup
	{
		public SampleStartup(IConfiguration configuration) : base(
			configuration,
			new List<string>()
			{
				DefaultPluginNamespace,
				"ExampleAPI.Plugins"
			})
		{
		}

		public override void ConfigureServices(IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Example API", Version = "v1" });
			});

			base.ConfigureServices(services);
		}

		public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			app.UseSwagger();
			app.UseSwaggerUI();

			base.Configure(app, env, loggerFactory);
		}
	}
}
