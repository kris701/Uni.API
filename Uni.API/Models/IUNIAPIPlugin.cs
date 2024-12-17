using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Uni.API.Models
{
	public interface IUniAPIPlugin
	{
		public Guid ID { get; }
		public string Name { get; }
		public bool IsActive { get; }
		public List<Guid> Requires { get; }

		public void ConfigureConfiguration(IConfiguration configuration);
		public void ConfigureServices(IServiceCollection services);
	}
}
