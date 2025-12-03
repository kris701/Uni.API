
<p align="center">
    <img src="https://github.com/user-attachments/assets/b282fe2a-662d-4e73-8076-1a60fd93c67b" width="200" height="200" />
</p>

[![Build and Publish](https://github.com/kris701/Uni.API/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/kris701/Uni.API/actions/workflows/dotnet-desktop.yml)
![Nuget](https://img.shields.io/nuget/v/Uni.API)
![Nuget](https://img.shields.io/nuget/dt/Uni.API)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/kris701/Uni.API/main)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/kris701/Uni.API)
![Static Badge](https://img.shields.io/badge/Platform-Windows-blue)
![Static Badge](https://img.shields.io/badge/Platform-Linux-blue)
![Static Badge](https://img.shields.io/badge/Framework-dotnet--10.0-green)

# Uni.API

This is a project to make it a easier and more compartmentalized to work with ASP.NET API's.
Instead of having a large API thats a mix of controllers, models, services, etc. why not seperate different functions into "plugins" instead?

That is the premise of this project.

The way it works, is that you create seperate projects, that act as small APIs on their own.
All these projects are then combined automatically into one at runtime, depending on what plugins
you want enabled/disabled.

As an example, to create a Uni.API API, simply have a console application like this:
```csharp
public class Program
{
	public static void Main(string[] args)
	{
		UniAPIBuilder.CreateUniAPIBuilder<UniAPIStartup>(args).Build().Run();
	}
}
```

This creates a default Uni.API API, that is configured by the means of a `configuration.json` file that 
must be in the root of the executing project (and be set to be copied. This file will include a definition
of what plugins to use, as well as optional configuration that the plugins could use. An example of this
`configuration.json` can be seen below:

```json
{
  "UsePlugins": [
    "PluginAssemblyName"
  ],

  "SomeConfigurations": {
	"value":true
  }
}
```

To use this project, simply include the nuget package that can be found on the [NuGet Package Manager](https://www.nuget.org/packages/Uni.API).

A complete example can be seen in the Example project.
