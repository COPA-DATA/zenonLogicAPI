using System.IO;
using System.Runtime.InteropServices;
using System.Text;

// TODO: @Mike Thomas: Remove this whole class, it destroys the platform independence! Also "Helper" is not a good name.

namespace zenonApi.Logic.Ini
{
  /// <summary>
  /// Class for handling a .ini file.
  /// Provides read and write functionality for properties and sections.
  /// </summary>
  public class IniFile
  {
    internal string Path { get; private set; }

    [DllImport("kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool WritePrivateProfileString(string section,
      string key, string val, string filePath);

    [DllImport("kernel32")]
    private static extern uint GetPrivateProfileString(string section,
      string key, string def, StringBuilder retVal,
      uint size, string filePath);

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="iniFilePath"></param>
    internal IniFile(string iniFilePath)
    {
      if (!File.Exists(iniFilePath))
      {
        File.Create(iniFilePath).Close();
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
