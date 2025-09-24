using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Uni.API.Models;

namespace Uni.API.Filters
{
	public class ValidateModelFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			foreach (var arg in context.ActionArguments.Values)
				if (arg != null)
					foreach (var prop in arg.GetType().GetProperties())
						if (prop.HasAttribute<JsonIgnoreAttribute>())
							context.ModelState.Remove(prop.Name);

			if (context.ModelState.IsValid == false)
			{
				var errorList = context.ModelState.ToDictionary(
					kvp => kvp.Key,
					kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
				);
				var sb = new StringBuilder();
				foreach(var key in errorList.Keys)
					foreach(var error in errorList[key])
						sb.AppendLine($"[{key}] {error}");

				var response = new ErrorModel(
					StatusCodes.Status422UnprocessableEntity,
					"One or more validation errors occured",
					sb.ToString());
				context.Result = new BadRequestObjectResult(response);
			}
		}
	}
}
