# String Syntax Polyfill

[![NuGet Package](https://img.shields.io/nuget/v/StringSyntaxPolyfill.svg?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/StringSyntaxPolyfill)
[![GitHub Release](https://img.shields.io/github/v/release/daviddotcs/string-syntax-polyfill?label=GitHub&logo=github&style=for-the-badge)](https://github.com/daviddotcs/string-syntax-polyfill/releases/latest)

Makes [StringSyntaxAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.codeanalysis.stringsyntaxattribute?view=net-7.0) available to target frameworks older than .NET 7. This enables IDE features such as syntax highlighting and code completion within strings, and Visual Studio currently supports `DateTimeFormat`, `Json`, and `Regex` as of version 17.2.

## Compatibility

Including projects must be compatible with .NET Standard 1.0 and use C# 8 or later. This means projects targeting the following should work:

* .NET/.NET Core 1.0 or greater
* .NET Framework 4.5 or greater
* .NET Standard 1.0 or greater

## Alternatives

You might instead consider [PolySharp](https://github.com/Sergio0694/PolySharp) which uses a source generator to provide polyfills for `[StringSyntax]` among other types in order to enable various new C# language features on older runtimes.

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
