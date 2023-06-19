namespace FileExtractor.CLI.Models;

public record ExtractedFile
(
  int Id,
  string FileName,
  string FileExtension,
  long FileSize,
  string OriginalFilePath
);
