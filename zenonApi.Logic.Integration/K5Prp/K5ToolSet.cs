using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using zenonApi.Logic;
using zenonApi.Zenon.Helper;

namespace zenonApi.Zenon.K5Prp
{
  /// <summary>
  /// Toolset for more convenient work with the K5Prp.dll and K5B.exe
  /// </summary>
  internal class K5ToolSet
  {
    /// <summary>
    /// Import of extern dll method contained in K5Prp.dll.
    /// Methode represents CLI to set of commands for interaction with straton.
    /// </summary>
    /// <param name="szProject"></param>
    /// <param name="szCommand"></param>
    /// <param name="dwOk"></param>
    /// <param name="dwDataIn"></param>
    /// <param name="dwDataOut"></param>
    /// <returns></returns>
    [DllImport("K5Prp.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern IntPtr K5PRPCall(string szProject, string szCommand, ref uint dwOk, ref uint dwDataIn, ref uint dwDataOut);

    private string _k5BexeFilePath;
    /// <summary>
    /// File path of the K5B.
    /// </summary>
    /// <remarks>
    /// Filepath is created depending on registered version of zenon extracted from registry.
    /// </remarks>
    private string K5BexeFilePath
    {
      get
      {
        if (!string.IsNullOrEmpty(_k5BexeFilePath))
        {
          return _k5BexeFilePath;
        }

        RegistryKey localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        RegistryKey zenonRegistryDataDirKey = localMachine.OpenSubKey(Strings.ZenonRegistrySoftwareDataDirPath, false);
        if (zenonRegistryDataDirKey == null)
        {
          throw new NullReferenceException(Strings.ZenonRegistryPathNotFound);
        }

        string currentRegisteredZenonVersionDirectory = (string)zenonRegistryDataDirKey.GetValue(Strings.ZenonRegistryCurrentRegisteredVersionKey);
        if (string.IsNullOrWhiteSpace(currentRegisteredZenonVersionDirectory))
        {
          throw new NullReferenceException(Strings.ZenonRegistryCurrentRegistredVersionEntryNotFound);
        }

        _k5BexeFilePath = Path.Combine(currentRegisteredZenonVersionDirectory, Strings.K5BexeFileName);
        return _k5BexeFilePath;
      }
    }

    /// <summary>
    /// Directory of the zenon Logic projects which belong to the zenon project.
    /// </summary>
    /// <example> C:\ProgramData\COPA-DATA\SQL2012\"zenon project GUID"\FILES\straton\"zenon Logic project name" </example>
    internal string ZenonLogicProjectDirectory { get; private set; }

    internal K5ToolSet(string zenonLogicProjectDirectory)
    {
      if (string.IsNullOrWhiteSpace(zenonLogicProjectDirectory))
      {
        throw new ArgumentNullException(string.Format(Strings.MethodArgumentNullException,
          nameof(zenonLogicProjectDirectory), nameof(K5ToolSet)));
      }

      ZenonLogicProjectDirectory = zenonLogicProjectDirectory;
      InitializeEnvironmentPathVariable();
    }

    /// <summary>
    /// Imports the stated <see cref="LogicProject"/> into zenon Logic.
    /// </summary>
    /// <param name="zenonLogicProject"></param>
    /// <returns></returns>
    internal bool ImportZenonLogicProject(LogicProject zenonLogicProject)
    {
      string xmlFilePathToImport = SerializeZenonLogicProjectToXmlFile(zenonLogicProject);

      ProcessStartInfo startInfo = new ProcessStartInfo(K5BexeFilePath,
        $"XMLMERGE {this.ZenonLogicProjectDirectory} {xmlFilePathToImport}")
      { CreateNoWindow = false, WindowStyle = ProcessWindowStyle.Hidden };

      Process stratonXmlImportProcess = new Process{StartInfo = startInfo};
      if (!stratonXmlImportProcess.Start())
      {
        return false;
      }

      stratonXmlImportProcess.WaitForExit();

      string appliXmlFilePathToImport = TemporaryFileCreator.GetRandomTemporaryFilePathWithExtension("xml");
      zenonLogicProject.ApplicationTree.ExportAsFile(appliXmlFilePathToImport);

      var appliFilePath = Path.Combine(this.ZenonLogicProjectDirectory, "appli.xml");
      if (File.Exists(appliFilePath))
      {
        File.Delete(appliFilePath);
      }

      File.Copy(appliXmlFilePathToImport, appliFilePath);
      return true;
    }

    private string SerializeZenonLogicProjectToXmlFile(LogicProject zenonLogicProject)
    {
      string xmlFilePathToImport = TemporaryFileCreator.GetRandomTemporaryFilePathWithExtension("xml");
      zenonLogicProject.ExportAsFile(xmlFilePathToImport);
      return xmlFilePathToImport;
    }

    /// <summary>
    /// Creates a default zenon Logic project
    /// </summary>
    /// <returns></returns>
    internal bool CreateDefaultZenonLogicProject()
    {
      bool commandSuccessful = ExecuteK5PrpCommand("CreateProject", out string returnMessage, out _);
      if (!commandSuccessful)
      {
        throw new Exception(returnMessage);
      }
      return true;
    }

    /// <summary>
    /// XML export of the zenon Logic project to the stated XML file.
    /// </summary>
    /// <param name="xmlExportFilePath">XML file path which is used for export.</param>
    /// <returns>Exception if error occurs during export.</returns>
    internal bool ExportZenonLogicProjectAsXml(string xmlExportFilePath)
    {
      ProcessStartInfo startInfo = new ProcessStartInfo(K5BexeFilePath,
          $"X {this.ZenonLogicProjectDirectory} {Strings.K5BxmlExportFormatString} {xmlExportFilePath}")
        { CreateNoWindow = false, WindowStyle = ProcessWindowStyle.Hidden };

      Process stratonXmlExportProcess = new Process{StartInfo = startInfo};

      if (!stratonXmlExportProcess.Start())
      {
        return false;
      }

      stratonXmlExportProcess.WaitForExit();

      return true;
    }

    /// <summary>
    /// Calls the exported CLI method of the K5Prp.dll
    /// </summary>
    /// <param name="k5Command">K5Prp.dll command to execute</param>
    /// <param name="returnMessage">Message which gets returned by the dll</param>
    /// <param name="outHandle">Handle ID which is returned by certain commands.</param>
    /// <returns></returns>
    private bool ExecuteK5PrpCommand(string k5Command, out string returnMessage, out uint outHandle)
    {
      try
      {
        uint dwOk = 0;
        uint dwDataIn = 0;
        uint dwDataOut = 0;

        IntPtr commandResult = K5PRPCall(ZenonLogicProjectDirectory, k5Command, ref dwOk, ref dwDataIn, ref dwDataOut);

        returnMessage = Marshal.PtrToStringAnsi(commandResult);
        bool executedSuccessfully = Convert.ToBoolean(dwOk);
        outHandle = dwDataOut;

        return executedSuccessfully;
      }
      catch (Exception ex)
      {
        returnMessage = string.Format(Strings.ExternalK5PrpCallError, ex.Message);
        outHandle = 0;
        return false;
      }
    }

    private void InitializeEnvironmentPathVariable()
    {
      Process currentProcess = Process.GetCurrentProcess();
      string fileName = currentProcess.MainModule?.FileName;
      if (string.IsNullOrEmpty(fileName))
      {
        throw new NullReferenceException(Strings.CurrentProcessMainModuleFileNameNull);
      }

      string currentProcessFilePath;
      if (fileName.Contains("AddIn")) // Where in the Debugger...
      {
        currentProcessFilePath = new FileInfo(fileName).Directory.Parent.FullName;
      }
      else
      {
        currentProcessFilePath = new FileInfo(fileName).Directory.FullName;
      }

      string pathEnvironmentVariableContent = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
      var splitPathEnvironmentVariableContent = pathEnvironmentVariableContent.Split(';');
      if (splitPathEnvironmentVariableContent.Contains(currentProcessFilePath))
      {
        return;
      }

      pathEnvironmentVariableContent += $";{currentProcessFilePath}";
      Environment.SetEnvironmentVariable("PATH", pathEnvironmentVariableContent);
    }
  }
}
