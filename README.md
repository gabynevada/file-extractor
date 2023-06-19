# File Extractor

## Description
CLI program that extracts files from a directory and its subdirectories and generates a csv file with the following columns:

| Column Name      | Type   | Description |
|------------------|--------|-------------|
| Id               | number | Unique id.  |
| FileName         | string | Name of file. |
| FileExtension    | string | File extension. |
| FileSize         | number | File size in bytes. |
| OriginalFilePath | string | Full path of file. |

## Usage

### Extract files from directory
```zsh
file-extractor extract <extract-from-directory-path>
```
This uses the basic configuration in which the program will extract all files from the directory and its subdirectories. If a compressed file is found, it will be extracted and the files inside will be added to the list of files to be extracted.

* It will save a copy of all files in the ExtractedFiles folder in the current directory.
* It will generate a csv file with the extracted files metadata in the ExtractedFiles folder in the current directory.

### Extract files from directory with custom configuration
```zsh
file-extractor extract <extract-from-directory-path> -o <output-directory-path> --extracted-files-list-output <extracted-files-list-output-file-path> --extracted-files-output <extracted-files-output-directory-path>
```
