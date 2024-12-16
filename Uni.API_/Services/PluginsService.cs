using Uni.API.Models;

namespace Uni.API.Services
{
	public class PluginsService
	{
		public List<IUNIAPIPlugin> Plugins { get; }

		public PluginsService(List<IUNIAPIPlugin> plugins)
		{
			Plugins = plugins;
		}
	}
}
