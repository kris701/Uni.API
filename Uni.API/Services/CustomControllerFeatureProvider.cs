using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
			{
				isController = _allowedNamespaces.Any(x => typeInfo.Namespace.Contains(x));
			}

			return isController;
		}
	}
}
