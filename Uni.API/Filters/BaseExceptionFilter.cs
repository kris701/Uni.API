using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Uni.API.Models;

namespace Uni.API.Filters
{
	internal class BaseExceptionFilter : IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			var error = new ErrorModel
			(
				500,
				context.Exception.Message,
				context.Exception.StackTrace?.ToString()
			);

			context.Result = new JsonResult(error)
			{
				StatusCode = error.StatusCode
			};
		}
	}
}
