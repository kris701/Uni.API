using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Uni.API.Services
{
	// Modified version of https://stackoverflow.com/a/61325554
	internal class CustomControllerFeatureProvider : ControllerFeatureProvider
	{
		private readonly List<string> _allowedNamespaces;

		public CustomControllerFeatureProvider(List<string> allowedNamespaces)
		{
			_allowedNamespaces = allowedNamespaces;
		}

		protected override bool IsController(TypeInfo typeInfo)
		{
			var isController = base.IsController(typeInfo);

			if (isController)
				if (typeInfo.Namespace != null)
					isController = _allowedNamespaces.Any(x => typeInfo.Namespace.Contains(x));

			return isController;
		}
	}
}
