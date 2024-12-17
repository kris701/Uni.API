using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Uni.API.Models
{
	/// <summary>
	/// Base abstract implementation of the <seealso cref="IUniAPIPlugin"/> interface
	/// </summary>
	public abstract class BaseUniAPIPlugin : IUniAPIPlugin
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
		public bool IsActive { get; private set; } = false;
		/// <summary>
		/// List of other plugin IDs that must be loaded before this.
		/// </summary>
		public List<Guid> Requires { get; }
		/// <summary>
		/// Namespace that this plugin resides in.
		/// </summary>
		public string NameSpace { get; set; } = "";

		/// <summary>
		/// Main constructor
		/// </summary>
		/// <param name="iD"></param>
		/// <param name="name"></param>
		protected BaseUniAPIPlugin(Guid iD, string name)
		{
			ID = iD;
			Name = name;
			Requires = new List<Guid>();
		}

		/// <summary>
		/// Constructor with additional requirements override
		/// </summary>
		/// <param name="iD"></param>
		/// <param name="name"></param>
		/// <param name="requires"></param>
		protected BaseUniAPIPlugin(Guid iD, string name, List<Guid> requires) : this(iD, name)
		{
			Requires = requires;
		}

		/// <summary>
		/// Configure the plugin with the <paramref name="configuration"/>
		/// </summary>
		/// <param name="configuration"></param>
		public virtual void ConfigureConfiguration(IConfiguration configuration)
		{

		}

		/// <summary>
		/// Configure the services for the plugins
		/// </summary>
		/// <param name="services"></param>
		public virtual void ConfigureServices(IServiceCollection services)
		{
			IsActive = true;
		}
	}
}
