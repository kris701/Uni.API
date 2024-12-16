using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UNI.API.Models;

namespace UNI.API.Exceptions
{
	public class BaseExceptionFilter : IExceptionFilter
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
