#r "nuget: Markdig, 0.28.1"

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Markdig;

#nullable enable

var consoleLock = new object();

public static string GetCallerFilePath([CallerFilePath] string path = "") => path;

public string AddMarkdownTableOfContents(string sourceFile)
{
  var text = File.ReadAllText(sourceFile);

  var pipeline = new Markdig.MarkdownPipelineBuilder()
    .UseAdvancedExtensions()
    .Build();

  var document = Markdig.Markdown.Parse(text, pipeline);
  if (document is null)
  {
    return text;
  }

  var headings = document
    .OfType<Markdig.Syntax.HeadingBlock>()
    .Where(x => x.Level > 1)
    .ToArray();

  if (headings.Length == 0)
  {
    return text;
  }

  var plainTextBuilder = new StringBuilder();
  using var plainTextWriter = new StringWriter(plainTextBuilder);
  var plainTextRenderer = new Markdig.Renderers.HtmlRenderer(plainTextWriter)
  {
    EnableHtmlForBlock = false,
    EnableHtmlForInline = false,
    EnableHtmlEscape = false
  };

  var tocBuilder = new StringBuilder();
  var firstHeadingLocation = -1;

  tocBuilder.AppendLine("## Table of Contents").AppendLine();

  foreach (var heading in headings)
  {
    var attributes = Markdig.Renderers.Html.HtmlAttributesExtensions.TryGetAttributes(heading);
    if (attributes is null)
    {
      continue;
    }

    if (firstHeadingLocation == -1)
    {
      firstHeadingLocation = heading.Span.Start;
    }

    plainTextBuilder.Clear();
    plainTextRenderer.Render(heading);

    tocBuilder
      .Append(' ', (heading.Level - 2) * 4)
      .Append("- [")
      .Append(plainTextBuilder.ToString().Trim())
      .Append("](#")
      .Append(attributes.Id)
      .AppendLine(")");
  }

  if (firstHeadingLocation == -1)
  {
    return text;
  }

  tocBuilder.AppendLine();

  var resultBuilder = new StringBuilder(text);
  resultBuilder.Insert(firstHeadingLocation, tocBuilder.ToString());
  resultBuilder.Insert(0, Environment.NewLine);
  resultBuilder.Insert(0, "[//]: # (Generated file, do not edit manually. Source: " + sourceFile + ")");

  return resultBuilder.ToString();
}

public bool IsWorkingDirectoryClean()
{
  var hasOutput = false;
  var command = "git";
  var arguments = "status --porcelain";
  WriteLine($"> {command} {arguments}{Environment.NewLine}");

  var cmd = new Process();
  cmd.StartInfo = new ProcessStartInfo
  {
    FileName = command,
    Arguments = arguments,
    CreateNoWindow = true,
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    UseShellExecute = false,
    WindowStyle = ProcessWindowStyle.Hidden
  };
  cmd.OutputDataReceived += (_, e) =>
  {
    if (e.Data != null)
    {
      hasOutput = true;
      WriteLine(e.Data, isError: false, GetLineColor(e.Data) ?? ConsoleColor.DarkGray);
    }
  };
  cmd.ErrorDataReceived += (_, e) =>
  {
    if (e.Data != null)
    {
      hasOutput = true;
      WriteLine(e.Data, isError: true, ConsoleColor.Red);
    }
  };

  cmd.Start();
  cmd.BeginOutputReadLine();
  cmd.BeginErrorReadLine();
  cmd.WaitForExit();

  return !hasOutput && cmd.ExitCode == 0;
}

public void WriteLine(string? text, bool isError = false, ConsoleColor? color = null)
{
  if (text == null) return;

  lock (consoleLock)
  {
    if (color != null)
    {
      Console.ForegroundColor = color.Value;
    }

    if (isError)
    {
      Console.Error.WriteLine(text);
    }
    else
    {
      Console.WriteLine(text);
    }

    if (color != null)
    {
      Console.ResetColor();
    }
  }
}

public ConsoleColor? GetLineColor(string? text)
{
  if (text is null) return null;

  if (text.StartsWith("Passed!"))
    return ConsoleColor.Green;

  if (text.StartsWith("Failed!") || text.Contains(": error "))
    return ConsoleColor.Red;

  if (text.Contains(": warning "))
    return ConsoleColor.Yellow;

  return null;
}

public int RunCommand(string command, string arguments)
{
  WriteLine($"> {command} {arguments}{Environment.NewLine}");

  var cmd = new Process();
  cmd.StartInfo = new ProcessStartInfo
  {
    FileName = command,
    Arguments = arguments,
    CreateNoWindow = true,
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    UseShellExecute = false,
    WindowStyle = ProcessWindowStyle.Hidden
  };
  cmd.OutputDataReceived += (_, e) => WriteLine(e.Data, isError: false, GetLineColor(e.Data) ?? ConsoleColor.DarkGray);
  cmd.ErrorDataReceived += (_, e) => WriteLine(e.Data, isError: true, ConsoleColor.Red);

  cmd.Start();
  cmd.BeginOutputReadLine();
  cmd.BeginErrorReadLine();
  cmd.WaitForExit();

  return cmd.ExitCode;
}
