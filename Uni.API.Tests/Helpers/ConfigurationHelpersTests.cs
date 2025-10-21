using Microsoft.Extensions.Configuration;
using Uni.API.Helpers;

namespace Uni.API.Tests.Helpers
{
	[TestClass]
	public class ConfigurationHelpersTests
	{
		[TestMethod]
		[DataRow("TestFiles/input1.json", "nonexisting", false)]
		[DataRow("TestFiles/input1.json", "someconfig", true)]
		[DataRow("TestFiles/input1.json", "someconfig2", true)]
		[DataRow("TestFiles/input1.json", "someconfig2awdasdf", false)]
		public void Can_CheckIfConfigExist(string inputFile, string target, bool shouldExist)
		{
			// ARRANGE
			var config = new ConfigurationBuilder()
				.AddJsonFile(inputFile)
				.Build();

			// ACT
			var exists = config.DoesSectionExist(target);

			// ASSERT
			Assert.AreEqual(shouldExist, exists);
		}

		[TestMethod]
		[DataRow("TestFiles/input1.json", "someconfig2", "works", false)]
		public void Can_GetSectionKeyValue_bool(string inputFile, string section, string key, bool expected)
		{
			// ARRANGE
			var config = new ConfigurationBuilder()
				.AddJsonFile(inputFile)
				.Build();

			// ACT
			var value = config.GetSectionValue<bool>(section, key);

			// ASSERT
			Assert.AreEqual(expected, value);
		}

		[TestMethod]
		[DataRow("TestFiles/input1.json", "someconfig", "test", "a")]
		public void Can_GetSectionKeyValue_string(string inputFile, string section, string key, string expected)
		{
			// ARRANGE
			var config = new ConfigurationBuilder()
				.AddJsonFile(inputFile)
				.Build();

			// ACT
			var value = config.GetSectionValue<string>(section, key);

			// ASSERT
			Assert.AreEqual(expected, value);
		}

		[TestMethod]
		[DataRow("TestFiles/input1.json", "someconfig", "some", 212313)]
		public void Can_GetSectionKeyValue_int(string inputFile, string section, string key, int expected)
		{
			// ARRANGE
			var config = new ConfigurationBuilder()
				.AddJsonFile(inputFile)
				.Build();

			// ACT
			var value = config.GetSectionValue<int>(section, key);

			// ASSERT
			Assert.AreEqual(expected, value);
		}
	}
}
