namespace Uni.API.Models
{
	/// <summary>
	/// Basic implementation of a return error model
	/// </summary>
	public class ErrorDictModel
	{
		/// <summary>
		/// Error ID
		/// </summary>
		public int StatusCode { get; set; }
		/// <summary>
		/// Overall message
		/// </summary>
		public string? Message { get; set; }
		/// <summary>
		/// Further details on the error
		/// </summary>
		public Dictionary<string, string[]> Details { get; set; }

		/// <summary>
		/// Main constructor
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="message"></param>
		/// <param name="details"></param>
		public ErrorDictModel(int statusCode, string? message, Dictionary<string, string[]> details)
		{
			StatusCode = statusCode;
			Message = message;
			Details = details;
		}
	}
}
