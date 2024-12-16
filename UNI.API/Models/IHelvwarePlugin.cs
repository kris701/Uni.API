using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UNI.API.Models
{
	public interface IHelvwarePlugin
	{
		public Guid ID { get; }
		public string Name { get; }
		public bool IsActive { get; }

		public void ConfigureConfiguration(IConfiguration configuration);
		public void ConfigureServices(IServiceCollection services);
	}
}
