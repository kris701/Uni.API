using Uni.API.Models;

namespace ExampleAPI.Plugins.ExamplePlugin2
{
	public class ExamplePluginPlugin2 : BaseUniAPIPlugin
	{
		public ExamplePluginPlugin2() : base(
			new Guid("bdb7ef85-b69e-4df2-b44c-bdd747009ed3"),
			"Some example plugin 2",
			new List<Guid>()
			{
				new Guid("fb1c42a2-770b-4bb0-a2b6-8f6736857022")
			})
		{
		}
	}
}
