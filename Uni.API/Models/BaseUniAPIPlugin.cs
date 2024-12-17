using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.API.Models
{
	public abstract class BaseUniAPIPlugin : IUniAPIPlugin
	{
		public Guid ID { get; }
		public string Name { get; }
		public bool IsActive { get; private set; } = false;
		public List<Guid> Requires { get; }

		protected BaseUniAPIPlugin(Guid iD, string name)
		{
			ID = iD;
			Name = name;
			Requires = new List<Guid>();
		}

		protected BaseUniAPIPlugin(Guid iD, string name, List<Guid> requires) : this(iD, name)
		{
			Requires = requires;
		}

		public virtual void ConfigureConfiguration(IConfiguration configuration)
		{

		}

		public virtual void ConfigureServices(IServiceCollection services)
		{
			IsActive = true;
		}
	}
}
