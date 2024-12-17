using ExampleAPI.Plugins.ExamplePlugin.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Plugins.ExamplePlugin.Controllers
{
	public class SomeController : ControllerBase
	{
		private readonly SomePluginConfiguration _config;

		public SomeController(SomePluginConfiguration config)
		{
			_config = config;
		}

		[HttpGet("test")]
		public IActionResult TestEndpoint()
		{
			return Ok($"Configured value: {_config.Value1}");
		}
	}
}
