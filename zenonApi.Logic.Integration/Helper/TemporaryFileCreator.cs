using System;
using System.IO;

namespace zenonApi.Logic.Integration.Helper
{
  /// <summary>
  /// Class provides the functionality to get a file path with random name and specified file extension
  /// located in the current user´s temporary folder.
  /// </summary>
  internal static class TemporaryFileCreator
  {
    /// <summary>
    /// Returns file path with random file name and specified file extension located in the current user´s temporary
    /// folder.
    /// </summary>
    /// <param name="fileExtension"></param>
    /// <returns></returns>
    internal static string GetRandomTemporaryFilePathWithExtension(string fileExtension)
    {
      string userTemporaryFolderDirectory = Path.GetTempPath();
      string fileNameWithExtension = $"{Guid.NewGuid()}.{RemoveDotOfExtension(fileExtension)}";
      return Path.Combine(userTemporaryFolderDirectory, fileNameWithExtension);
    }

    private static string RemoveDotOfExtension(string fileExntensionWithDot) =>
      string.IsNullOrWhiteSpace(fileExntensionWithDot) ? string.Empty : fileExntensionWithDot.TrimStart('.');
  }
}
