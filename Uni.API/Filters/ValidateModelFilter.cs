using Microsoft.AspNetCore.Http;
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
				var errorList = context.ModelState.ToDictionary(
					kvp => kvp.Key,
					kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
				);

				var response = new ErrorDictModel(
					StatusCodes.Status422UnprocessableEntity,
					"One or more validation errors occured",
					errorList);
				context.Result = new BadRequestObjectResult(response);
			}
		}
	}
}
