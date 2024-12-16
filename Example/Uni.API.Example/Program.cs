using Microsoft.Extensions.Hosting;

namespace Uni.API.Example
{
	public class Program
	{
		public static void Main(string[] args)
		{
			UniAPIBuilder.CreateUniAPIBuilder<UniAPIStartup>(args).Build().Run();
		}
	}
}