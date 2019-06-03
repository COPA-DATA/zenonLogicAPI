using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using zenonApi.Logic.Resources;

namespace zenonApi.Logic.Helper
{
  /// <summary>
  /// Class for handling a .ini file.
  /// Provides read and write functionality for properties and sections.
  /// </summary>
  public class IniFile
  {
    internal string Path { get; private set; }

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section,
      string key, string val, string filePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section,
      string key, string def, StringBuilder retVal,
      int size, string filePath);

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="iniFilePath"></param>
    internal IniFile(string iniFilePath)
    {
      if (!File.Exists(iniFilePath))
      {
        throw new ArgumentNullException(string.Format(Strings.IniFileNotFoundException, iniFilePath));
      }

      Path = iniFilePath;
    }

    /// <summary>
    /// Write Data to the INI File
    /// </summary>
    /// <param name="pSection">Section</param>
    /// <param name="pKey">Key</param>
    /// <param name="pValue">Value</param>
    internal void WriteValueToFile(string pSection, string pKey, string pValue)
    {
      WritePrivateProfileString(pSection, pKey, pValue, Path);
    }

    /// <summary>
    /// Read Data Value From the Ini File
    /// </summary>
    /// <param name="pSection">Section</param>
    /// <param name="pKey">Key</param>
    /// <returns>Value</returns>
    internal string ReadValueFromFile(string pSection, string pKey)
    {
      StringBuilder returnMessage = new StringBuilder(255);
      GetPrivateProfileString(pSection, pKey, "", returnMessage, 255, this.Path);
      return returnMessage.ToString();
    }
  }
}
