// See https://aka.ms/new-console-template for more information

using System.CommandLine;
using FileExtractor.CLI.Commands;

var extractCommand = ExtractCommand.GetExtractCommand();

var rootCommand = new RootCommand("Utility to extract files from a directory")
{
  extractCommand,
};

await rootCommand.InvokeAsync(args);
