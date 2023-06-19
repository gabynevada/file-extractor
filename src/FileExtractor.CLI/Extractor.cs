using System.IO.Compression;
using FileExtractor.CLI.Models;

namespace FileExtractor.CLI;

public static class Extractor
{
  public static List<ExtractedFile> GetFilesFromDirectory(
    ref int fileNumber,
    DirectoryInfo rootDirectory,
    DirectoryInfo extractFromDirectory,
    DirectoryInfo extractToDirectory,
    FileInfo? compressedFileParent = null)
  {
    var extractedFiles = new List<ExtractedFile>();

    var files = extractFromDirectory.EnumerateFiles();
    foreach (var file in files)
    {
      var isCompressedFile = file.Extension is ".zip";
      if (isCompressedFile)
      {
        var temporaryDirectory = new DirectoryInfo(Path.Combine(extractToDirectory.FullName, $"{fileNumber}_temp"));
        ExtractCompressedFile(file, temporaryDirectory);

        extractedFiles.AddRange(GetFilesFromDirectory(ref fileNumber, rootDirectory, temporaryDirectory,
          extractToDirectory, file));

        temporaryDirectory.Delete(true);
      }
      else
      {
        var destinationFilePath = Path.Combine(extractToDirectory.FullName, $"{fileNumber}_{file.Name}");
        var originalFilePath = compressedFileParent is null ? file.FullName : compressedFileParent.FullName;
        // Remove extractFromDirectory.FullName from originalFilePath
        originalFilePath = originalFilePath.Replace(rootDirectory.FullName, "");

        file.CopyTo(destinationFilePath);
        extractedFiles.Add(new ExtractedFile(fileNumber, file.Name, file.Extension, file.Length, originalFilePath));
        fileNumber++;
      }
    }

    var directories = extractFromDirectory.EnumerateDirectories();
    foreach (var directory in directories)
    {
      extractedFiles.AddRange(GetFilesFromDirectory(ref fileNumber, rootDirectory, directory, extractToDirectory));
    }

    return extractedFiles;
  }

  private static void ExtractCompressedFile(FileInfo compressedFile, DirectoryInfo extractToDirectory)
  {
    extractToDirectory.Create();
    var compressedFileExtension = compressedFile.Extension;
    var compressedFileExtensionIsZip = compressedFileExtension is ".zip";
    if (compressedFileExtensionIsZip)
    {
      ZipFile.ExtractToDirectory(compressedFile.FullName, extractToDirectory.FullName);
    }
  }
}
