using System.Collections.Generic;
using System.IO;

namespace zenonApi.Zenon.StratonUtilities
{
  internal static class StratonFolderPath
  {
    /// <summary>
    /// Gets a sequence of directory paths of zenon Logic projects which are located in the specified straton directory path.
    /// </summary>
    /// <param name="stratonDirectory"> Example value: C:\ProgramData\COPA-DATA\SQL2012\"zenon Project GUID"\FILES\straton" </param>
    /// <returns> Example return value of the sequence: C:\ProgramData\COPA-DATA\SQL2012\"zenon Project GUID"\FILES\straton\"zenon Logic project name" </returns>
    internal static IEnumerable<string> GetZenonLogicProjectFolderPaths(string stratonDirectory)
    {
      if (!Directory.Exists(stratonDirectory))
      {
        throw new DirectoryNotFoundException(string.Format(Strings.DirectoryNotFoundInGetZenonLogicProjectFolderPaths,
          stratonDirectory));
      }

      DirectoryInfo stratonDirectoryInfo = new DirectoryInfo(stratonDirectory);
      foreach (DirectoryInfo subDirectories in stratonDirectoryInfo.GetDirectories())
      {
        string directoryFullName = subDirectories.FullName;
        // global folder of each project can be skipped
        if (directoryFullName.EndsWith("_Global"))
        {
          continue;
        }

        yield return subDirectories.FullName;
      }
    }
  }
}
