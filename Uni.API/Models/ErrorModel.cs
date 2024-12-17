namespace Uni.API.Models
{
	/// <summary>
	/// Basic implementation of a return error model
	/// </summary>
	public class ErrorModel
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
		public string? Details { get; set; }

		/// <summary>
		/// Main constructor
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="message"></param>
		/// <param name="details"></param>
		public ErrorModel(int statusCode, string? message, string? details = null)
		{
			StatusCode = statusCode;
			Message = message;
			Details = details;
		}
	}
}
