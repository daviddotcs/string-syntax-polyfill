[//]: # (Generated file, do not edit manually. Source: README.source.md)
# String Syntax Polyfill

[![NuGet Package](https://img.shields.io/nuget/v/StringSyntaxPolyfill.svg?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/StringSyntaxPolyfill)
[![GitHub Release](https://img.shields.io/github/v/release/daviddotcs/string-syntax-polyfill?label=GitHub&logo=github&style=for-the-badge)](https://github.com/daviddotcs/string-syntax-polyfill/releases/latest)

Makes [StringSyntaxAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.codeanalysis.stringsyntaxattribute?view=net-7.0) available to target frameworks older than .NET 7. This enables IDE features such as syntax highlighting and code completion within strings, and Visual Studio currently supports `DateTimeFormat`, `Json`, and `Regex` as of version 17.3.5.

## Table of Contents

- [Compatibility](#compatibility)
- [Building the NuGet Package](#building-the-nuget-package)

## Compatibility

Including projects must be compatible with .NET Standard 2.0 and use C# 8 or later. This means projects targeting the following should work:

* .NET/.NET Core 2.0 or greater
* .NET Framework 4.6.1 or greater
* .NET Standard 2.0 or greater

## Building the NuGet Package

* Ensure you have the latest .NET SDK installed via https://dotnet.microsoft.com/en-us/download/dotnet.
* Install [dotnet-script](https://github.com/filipw/dotnet-script).

```
dotnet tool install -g dotnet-script
```

* Within the `src` directory, run the build script with the new build number as an argument, e.g.; 1.2.3.

```
dotnet script build.csx -- 1.2.3
```

* Review the output to ensure that the build succeeded.
