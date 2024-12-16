using UNI.API.Models;

namespace UNI.API.Services
{
	public class PluginsService
	{
		public List<IHelvwarePlugin> Plugins { get; }

		public PluginsService(List<IHelvwarePlugin> plugins)
		{
			Plugins = plugins;
		}
	}
}
