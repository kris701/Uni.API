using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uni.API.Models;

namespace Uni.API.Filters
{
	public class ValidateModelFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (context.ModelState.IsValid == false)
			{
				var response = new ErrorModel(
					400,
					"One or more validation errors occured",
					context.ModelState.ToString());
				context.Result = new BadRequestObjectResult(response);
			}
		}
	}
}
