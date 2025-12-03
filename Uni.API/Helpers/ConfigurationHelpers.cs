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
			if (!configuration.DoesSectionExist(section))
				throw new Exception($"Section '{section}' was not set in IConfiguration!");
			var valueSection = configuration.GetSection(section).GetSection(key);
			var valueSectionExists = valueSection.Exists();

			// Just return an empty list, dont throw
			var tType = typeof(T);
			if (!valueSectionExists && tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(List<>))
			{
				var emptyList = Activator.CreateInstance(tType);
				if (emptyList == null)
					throw new Exception($"'{key}' for section '{section}' was an empty list, however the instantiation of it created a null object!");
				return (dynamic)emptyList;
			}

			if (!valueSectionExists)
				throw new Exception($"'{key}' for section '{section}' was not set in IConfiguration!");

			var value = valueSection.Get<T>();
			if (value == null)
				throw new Exception($"The key value was null for the key '{key}' and section '{section}' in IConfiguration!");
			return value;
		}
	}
}
