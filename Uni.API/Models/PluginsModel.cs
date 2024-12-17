namespace Uni.API.Models
{
	/// <summary>
	/// Plugins model to transfer active plugins
	/// </summary>
	public class PluginsModel
	{
		/// <summary>
		/// List of plugins
		/// </summary>
		public List<IUniAPIPlugin> Plugins { get; }

		/// <summary>
		/// Main constructor
		/// </summary>
		/// <param name="plugins"></param>
		public PluginsModel(List<IUniAPIPlugin> plugins)
		{
			Plugins = plugins;
		}
	}
}
