using System;
using System.Diagnostics;
using System.IO;
using zenonApi.Logic.Resources;

namespace zenonApi.Logic.Helper
{
  /// <summary>
  /// Class for handling of the K5dbxs.ini file of a zenon Logic project.
  /// The K5dbxis.ini file stores settings for the connection between zenon and zenon Logic.
  /// </summary>
  [DebuggerDisplay("{" + nameof(Path) + "}")]
  public class K5DbxsIniFile : IniFile
  {
    /// <summary>
    /// Factory method to create a K5dbxs.ini file with default settings and the settings specified as parameters.
    /// Note that an empty file has to exists.
    /// </summary>
    /// <param name="zenonProjectGuid">Project GUID of a zenon project.</param>
    /// <param name="k5DbxsFilePath">K5dbxs.ini file which should be configured.</param>
    /// <param name="mainPortNumber">Free main port number which should be used by the zenon Logic project for the
    /// communication with zenon.</param>
    /// <param name="driverId">ID of the driver which should be used by the zenon Logic project for the
    /// communication with zenon.</param>
    /// <returns></returns>
    public static K5DbxsIniFile CreateK5DbxsIniFile(string zenonProjectGuid, string k5DbxsFilePath,
      string mainPortNumber, string driverId)
    {
      K5DbxsIniFile iniFile = new K5DbxsIniFile(k5DbxsFilePath);

      iniFile.WriteDefaultSettingsToK5DbxsIniFile(iniFile, zenonProjectGuid, mainPortNumber, driverId);

      return iniFile;
    }

    /// <summary>
    /// Constructor for internal configuration purposes of the K5dbxs.ini file
    /// </summary>
    /// <param name="k5DbxsIniFilePath"></param>
    internal K5DbxsIniFile(string k5DbxsIniFilePath) : base(k5DbxsIniFilePath)
    {
      // ReSharper disable once PossibleNullReferenceException : check is done in base constrcutor
      if (!System.IO.Path.GetFileName(k5DbxsIniFilePath)
        .Equals(Strings.K5DbxsIniFileName, StringComparison.CurrentCultureIgnoreCase))
      {
        throw new InvalidDataException(String.Format(Strings.K5DbxsIniFileConstructorInvalidDataException,
          nameof(K5DbxsIniFile)));
      }
    }
   
    private const string CmdSection = "CMD";
    private const string MainPortPropertyKey = "MAINPORT";

    private string _mainPort;
    /// <summary>
    /// Mainport which is used by the zenon Logic project to communicate with zenon.
    /// Mainport configuration has to be distinct for each zenon Logic project within a zenon project.
    /// </summary>
    internal string MainPort
    {
      get
      {
        if (!string.IsNullOrEmpty(_mainPort))
        {
          return _mainPort;
        }

        _mainPort = this.ReadValueFromFile(CmdSection, MainPortPropertyKey);
        return _mainPort;
      }
      set
      {
        _mainPort = value;
        this.WriteValueToFile(CmdSection, MainPortPropertyKey, _mainPort);
      }
    }

    /// <summary>
    /// Writes default settings to the stated K5dbxs.ini file which are required to set up the communication between
    /// zenon Logic and zenon.
    /// </summary>
    /// <param name="iniFileToConfigure"></param>
    /// <param name="zenonProjectGuid"></param>
    /// <param name="mainPortNumber"></param>
    /// <param name="driverId"></param>
    private void WriteDefaultSettingsToK5DbxsIniFile(K5DbxsIniFile iniFileToConfigure, string zenonProjectGuid,
  string mainPortNumber, string driverId)
    {
      // + 7800 because the default zenon logic primary port value is 1200
      // the default bindport value is 9000, the values typically get increased by 1 for each project 
      int newCalculatedBindPort = int.Parse(mainPortNumber) + 7800;

      iniFileToConfigure.WriteValueToFile(CmdSection, "HOST", "localhost");
      iniFileToConfigure.WriteValueToFile(CmdSection, MainPortPropertyKey, $"{mainPortNumber}");

      iniFileToConfigure.WriteValueToFile(CmdSection, "BINDPORT", $"{newCalculatedBindPort}");
      iniFileToConfigure.WriteValueToFile(CmdSection, "START", "1");
      iniFileToConfigure.WriteValueToFile(CmdSection, "STEP", "0");
      iniFileToConfigure.WriteValueToFile(CmdSection, "PRIO", "0");
      iniFileToConfigure.WriteValueToFile(CmdSection, "REDUNDANCY", "0");
      iniFileToConfigure.WriteValueToFile(CmdSection, "VMWDGTO", "30");
      iniFileToConfigure.WriteValueToFile(CmdSection, "VMFKTTO", "<No function linked>");
      iniFileToConfigure.WriteValueToFile(CmdSection, "RETAINBYNAME", "1");

      iniFileToConfigure.WriteValueToFile("SETTINGS", "SETONLINE", $"localhost:{mainPortNumber}(10)");
      iniFileToConfigure.WriteValueToFile("SETTINGS", "DRVONLINE", "K5NET5.DLL");

      iniFileToConfigure.WriteValueToFile("XS", "Active", "1");
      iniFileToConfigure.WriteValueToFile("XS", "Project", $"PROJECT={zenonProjectGuid};HOST=localhost;");
      iniFileToConfigure.WriteValueToFile("XS", "StartTyp", "0");
      iniFileToConfigure.WriteValueToFile("XS", "StartArea", "0");
      iniFileToConfigure.WriteValueToFile("XS", "DriverID", $"{driverId}");
      iniFileToConfigure.WriteValueToFile("XS", "UsePrefix", "1");
      iniFileToConfigure.WriteValueToFile("XS", "UseUDFBPrefix", "1");
      iniFileToConfigure.WriteValueToFile("XS", "EquipmentModelCount", "0");
    }
  }
}
