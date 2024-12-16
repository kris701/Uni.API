using Microsoft.AspNetCore.Mvc;
using UNI.API.Services;

namespace UNI.API.Controllers
{
#if RELEASE
	[Authorize]
#endif
	[ApiController]
	public class PluginsController : ControllerBase
	{
		private readonly PluginsService _pluginService;

		public PluginsController(PluginsService pluginService)
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
