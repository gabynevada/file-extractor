using System.CommandLine;
using System.Globalization;
using nietras.SeparatedValues;

namespace FileExtractor.CLI.Commands;

public static class ExtractCommand
{
  public static Command GetExtractCommand()
  {
    var extractDirectoryPathArgument = new Argument<DirectoryInfo>
      (name: "extractDirectoryPath", description: "The path to the directory to extract files from");

    var extractToOption = new Option<DirectoryInfo>
    (name: "--output", description: "Directory to output files to",
      getDefaultValue: () => new DirectoryInfo("ExtractedFiles"));
    extractToOption.AddAlias("-o");

    var extractedFilesListOutputOption = new Option<FileInfo>
    (name: "--extracted-files-list-output", description: "Path to output a list of extracted files to",
      getDefaultValue: () => new FileInfo("ExtractedFilesList.csv"));

    var command = new Command("extract", "Match records in two files")
    {
      extractDirectoryPathArgument,
      extractToOption,
      extractedFilesListOutputOption,
    };

    command.SetHandler(
      (extractDirectoryPathArgumentValue, extractToOptionValue, extractedFilesListOutputOptionValue) =>
      {
        if (extractToOptionValue.Exists is false)
        {
          extractToOptionValue.Create();
        }

        var fileNumber = 0;

        var extractedFiles = Extractor.GetFilesFromDirectory(
          ref fileNumber,
          extractDirectoryPathArgumentValue,
          extractDirectoryPathArgumentValue,
          extractToOptionValue);

        using var writer = Sep.New(',').Writer().ToFile(extractedFilesListOutputOptionValue.FullName);
        foreach (var extractedFile in extractedFiles)
        {
          using var row = writer.NewRow();
          row["Id"].Set(extractedFile.Id.ToString(CultureInfo.InvariantCulture));
          row["FileName"].Set(extractedFile.FileName);
          row["FileExtension"].Set(extractedFile.FileExtension);
          row["FileSize"].Set(extractedFile.FileSize.ToString(CultureInfo.InvariantCulture));
          row["OriginalFilePath"].Set(extractedFile.OriginalFilePath);
        }
      },
      extractDirectoryPathArgument, extractToOption, extractedFilesListOutputOption);

    return command;
  }
}
