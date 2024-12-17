using Microsoft.AspNetCore.Mvc;
using Uni.API.Models;

namespace Uni.API.Controllers
{
	/// <summary>
	/// Plugin controller
	/// </summary>
	[ApiController]
	public class PluginsController : ControllerBase
	{
		private readonly PluginsModel _pluginService;

		/// <summary>
		/// Main constructor
		/// </summary>
		/// <param name="pluginService"></param>
		public PluginsController(PluginsModel pluginService)
		{
			_pluginService = pluginService;
		}

		/// <summary>
		/// Gets the currently active plugins.
		/// </summary>
		/// <returns></returns>
		[HttpGet("plugins")]
		public IActionResult Get_CurrentPlugins()
		{
			return Ok(_pluginService.Plugins);
		}
	}
}
