using Uni.API;

namespace ExampleAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			UniAPIBuilder.CreateUniAPIApplication<SampleStartup>(args).Run();
		}
	}
}