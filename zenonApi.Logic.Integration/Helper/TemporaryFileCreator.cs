using System;
using System.Collections.Generic;
using System.IO;

namespace zenonApi.Logic.Integration.Helper
{
  /// <summary>
  /// Class provides the functionality to get a file path with random name and specified file extension
  /// located in the current user´s temporary folder.
  /// </summary>
  internal static class TemporaryFileCreator
  { 
    private static readonly HashSet<string> TemporaryFilePathsToCleanup = new HashSet<string>();

    /// <summary>
    /// Returns a file path with random file name and specified file extension located in the current
    /// user´s temporary folder.
    /// </summary>
    /// <param name="fileExtension"></param>
    /// <returns></returns>
    internal static string GetRandomTemporaryFilePathWithExtension(string fileExtension)
    {
      string userTemporaryFolderDirectory = Path.GetTempPath();
      string fileNameWithExtension = $"{Guid.NewGuid()}.{RemoveDotOfFileExtension(fileExtension)}";
      string filePathWithExtension = Path.Combine(userTemporaryFolderDirectory, fileNameWithExtension);

      StoreFilePathForDeletion(filePathWithExtension);
      return filePathWithExtension;
    }

    /// <summary>
    /// Deletes temporary files which were created.
    /// </summary>
    internal static void CleanupTemporaryFiles()
    {
      foreach (var temporaryFilePathToDelete in TemporaryFilePathsToCleanup)
      {
        if (!File.Exists(temporaryFilePathToDelete))
        {
          continue;
        }
        File.Delete(temporaryFilePathToDelete);
      }
    }

    private static string RemoveDotOfFileExtension(string fileExntensionWithDot) =>
      string.IsNullOrWhiteSpace(fileExntensionWithDot) ? string.Empty : fileExntensionWithDot.TrimStart('.');

    /// <summary>
    /// Stores the created temporary files which were created to delete them if they are not needed any more.
    /// </summary>
    /// <param name="createdFilePath"></param>
    private static void StoreFilePathForDeletion(string createdFilePath)
    {
      TemporaryFilePathsToCleanup.Add(createdFilePath);
    }
  }
}
