using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Uni.API.Models
{
	/// <summary>
	/// Interface that a Uni.API plugin must implement
	/// </summary>
	public interface IUniAPIPlugin
	{
		/// <summary>
		/// ID of the plugin.
		/// This must be a unique ID
		/// </summary>
		public Guid ID { get; }
		/// <summary>
		/// Display name of the plugin.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// Indicator if the plugin is active or not
		/// </summary>
		public bool IsActive { get; }
		/// <summary>
		/// List of other plugin IDs that must be loaded before this.
		/// </summary>
		public List<Guid> Requires { get; }
		/// <summary>
		/// Namespace that this plugin resides in.
		/// </summary>
		public string NameSpace { get; set; }

		/// <summary>
		/// Configure the plugin with the <paramref name="configuration"/>
		/// </summary>
		/// <param name="configuration"></param>
		public void ConfigureConfiguration(IConfiguration configuration);
		/// <summary>
		/// Configure the services for the plugins
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services);
	}
}
