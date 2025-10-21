using Microsoft.Extensions.Configuration;

namespace Uni.API.Helpers
{
	/// <summary>
	/// Simple helper class to get values from an IConfiguration instance
	/// </summary>
	public static class ConfigurationHelpers
	{
		/// <summary>
		/// Check if a given section exists
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public static bool DoesSectionExist(this IConfiguration configuration, string section) => configuration.GetSection(section).Exists();

		/// <summary>
		/// Get a value from a section and key
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="configuration"></param>
		/// <param name="section"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static T GetSectionValue<T>(this IConfiguration configuration, string section, string key)
		{
			var value = configuration.GetSection(section).GetSection(key).Get<T>();
			if (value == null)
				throw new Exception($"'{key}' for section '{section}' was not set in IConfiguration!");
			return value;
		}
	}
}
