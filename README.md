# Quartz Windows Service Base Project

[![.NET](https://github.com/dusklight/quartz-windows-service/actions/workflows/build-main.yml/badge.svg)](https://github.com/dusklight/quartz-windows-service/actions/workflows/build-main.yml)

This is a demo base project for creating a Windows Service that uses Quartz.NET.

Demonstrates the following:

* Creating a Windows Service in .NET 7.
* Integrating [Quartz.NET](https://www.quartz-scheduler.net/) into a Windows Service.
* Using the [Options](https://learn.microsoft.com/en-us/dotnet/core/extensions/options) pattern to configure the application.
  * Includes a simple unit test demonstrating how to use `IOptions` in tests. 
* Using [Serilog](https://serilog.net/) to log to a file.
* Using [Inno Setup](https://jrsoftware.org/isinfo.php) to create an installer for the Windows Service.
* Using [GitHub Actions](https://github.com/dusklight/quartz-windows-service/blob/main/.github/workflows/build-main.yml) to:
  * Build the setup package with versioning based on the git commit hash.
  * Run `dotnet format` to check code formatting, generate code coverage report, and publish a release.
