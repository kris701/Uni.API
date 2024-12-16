using Uni.API.Models;

namespace Uni.API.Services
{
	public class PluginsService
	{
		public List<IUniAPIPlugin> Plugins { get; }

		public PluginsService(List<IUniAPIPlugin> plugins)
		{
			Plugins = plugins;
		}
	}
}
