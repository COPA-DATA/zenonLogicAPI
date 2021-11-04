using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using zenonApi.Logic;
using zenonApi.Zenon.Helper;
using zenonApi.Zenon.K5Srv;

namespace zenonApi.Zenon.K5Prp
{
  /// <summary>
  /// Toolset for more convenient work with the K5Prp.dll and K5B.exe
  /// </summary>
  internal class K5ToolSet
  {
    /// <summary>
    /// Encoding which is used for zenon Logic XML import/export files
    /// </summary>
    private const string LogicXmlEncoding = "iso-8859-1";

    /// <summary>
    /// File name which contains all output of the compilation steps.
    /// Located within zenon Logic project folder after first ever compilation.
    /// </summary>
    private const string ZenonLogicCompileLogFileName = "__build.log";

    static K5ToolSet()
    {
      K5PCall = (K5PRPCall)LoadFunction<K5PRPCall>("K5PRPCall");
      var zenonDirectory = GetActivatedZenonVersionPath();
      if (!string.IsNullOrWhiteSpace(zenonDirectory))
      {
        SetDllDirectory(zenonDirectory);
      }
    }

    public K5ToolSet(string zenonLogicProjectDirectory)
    {
      if (string.IsNullOrWhiteSpace(zenonLogicProjectDirectory))
      {
        throw new ArgumentNullException(string.Format(
          Strings.MethodArgumentNullException,
          nameof(zenonLogicProjectDirectory),
          nameof(K5ToolSet)));
      }

      ZenonLogicProjectDirectory = zenonLogicProjectDirectory;
      InitializeEnvironmentPathVariable();
    }

    /// <summary>
    /// Delegate for the K5PRPCall
    /// </summary>
    private delegate IntPtr K5PRPCall(string szProject, string szCommand, ref uint dwOk, ref uint dwDataIn, ref uint dwDataOut);

    private static readonly K5PRPCall K5PCall;

    /// <summary>
    /// Function to set directory from where the dlls should get loaded
    /// </summary>
    [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool SetDllDirectory(string lpPathName);


    /// <summary>
    /// Function to load a library and get the address of it
    /// </summary>
    [DllImport("Kernel32.dll", SetLastError = true)]
    private static extern IntPtr LoadLibraryEx(string path, IntPtr hFile, uint dwFlags);

    private bool SetOption(string option, string value)
    {
      return ExecuteK5PrpCommand("SETOPTION " + option + " " + value, out _, out _);
    }

    /// <summary>
    /// Function to get the address of the function which is defined in procName
    /// </summary>
    [DllImport("Kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    /// <summary>
    /// Gets the used zenon version over the registry and gets the delegate of the function which is defined in procName
    /// </summary>
    private static Delegate LoadFunction<T>(string functionName)
    {
      var zenonDir = GetActivatedZenonVersionPath();

      if (zenonDir == null) { return null; }

      string k5pPath = Path.Combine(zenonDir, "K5PRP.dll");

      //0x00000008 stands for LOAD_WITH_ALTERED_SEARCH_PATH
      //https://docs.microsoft.com/de-de/windows/win32/api/libloaderapi/nf-libloaderapi-loadlibraryexa
      IntPtr hModule = LoadLibraryEx(k5pPath, IntPtr.Zero, 0x000000008);

      if (hModule == IntPtr.Zero) { return null; }

      IntPtr functionAddress = GetProcAddress(hModule, functionName);

      if (functionAddress == IntPtr.Zero) { return null; }
      return Marshal.GetDelegateForFunctionPointer(functionAddress, typeof(T));
    }

    private static string GetActivatedZenonVersionPath()
    {
      RegistryKey registryDir;
      try
      {
        registryDir = Registry.LocalMachine.OpenSubKey(Strings.ZenonRegistrySoftwareDataDirPath);
      }
      catch
      {
        return null;
      }

      if (registryDir == null) { return null; }

      string currentVersion = (string)registryDir.GetValue(Strings.ZenonRegistryCurrentVersionKey);
      string zenonDir = null;

      if (IntPtr.Size == 4)
      {
        zenonDir = (string)registryDir.GetValue(Strings.ZenonRegistryCurrentProgramDir32Prefix + currentVersion);
      }
      else if (IntPtr.Size == 8)
      {
        zenonDir = (string)registryDir.GetValue(Strings.ZenonRegistryCurrentProgramDir64Prefix + currentVersion);
      }

      return zenonDir;
    }

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

        _k5BexeFilePath = Path.Combine(GetZenonX86InstallationDirectory, Strings.K5BexeFileName);

        if (!File.Exists(_k5BexeFilePath))
        {
          throw new FileNotFoundException(string.Format(Strings.K5BexeFileNoutFoundException, _k5BexeFilePath));
        }

        return _k5BexeFilePath;
      }
    }

    /// <summary>
    /// Directory of the zenon Logic projects which belong to the zenon project.
    /// </summary>
    /// <example> C:\ProgramData\COPA-DATA\SQL2012\"zenon project GUID"\FILES\straton\"zenon Logic project name" </example>
    internal string ZenonLogicProjectDirectory { get; private set; }

    /// <summary>
    /// Gets the x86 components installation directory of zenon
    /// </summary>
    private string GetZenonX86InstallationDirectory
    {
      get
      {
        RegistryKey zenonRegistryDataDirKey = GetZenonRegistryDataDirKey;
        return (string)zenonRegistryDataDirKey
          .GetValue($"{Strings.ZenonRegistryCurrentProgramDir32Prefix}{GetRegisteredZenonVersion}");
      }
    }

    /// <summary>
    /// Gets the x64 components installation directory of zenon
    /// </summary>
    private string GetZenonX64InstallationDirectory
    {
      get
      {
        RegistryKey zenonRegistryDataDirKey = GetZenonRegistryDataDirKey;
        return (string)zenonRegistryDataDirKey
          .GetValue($"{Strings.ZenonRegistryCurrentProgramDir64Prefix}{GetRegisteredZenonVersion}");
      }
    }

    /// <summary>
    /// Gets the current registered zenon version
    /// </summary>
    /// <example>returns 8000 for zenon version 8.00</example>
    private string GetRegisteredZenonVersion
    {
      get
      {
        RegistryKey zenonRegistryDataDirKey = GetZenonRegistryDataDirKey;
        string currentRegisteredZenonVersionDirectory = (string)zenonRegistryDataDirKey
          .GetValue(Strings.ZenonRegistryCurrentVersionKey);
        if (string.IsNullOrWhiteSpace(currentRegisteredZenonVersionDirectory))
        {
          throw new NullReferenceException(Strings.ZenonRegistryCurrentRegistredVersionEntryNotFound);
        }

        return currentRegisteredZenonVersionDirectory;
      }
    }

    /// <summary>
    /// Gets the Copa-Data DataDir registry node
    /// </summary>
    private RegistryKey GetZenonRegistryDataDirKey
    {
      get
      {
        RegistryKey localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        RegistryKey zenonRegistryDataDirKey = localMachine.OpenSubKey(Strings.ZenonRegistrySoftwareDataDirPath, false);
        if (zenonRegistryDataDirKey == null)
        {
          throw new NullReferenceException(Strings.ZenonRegistryPathNotFound);
        }

        return zenonRegistryDataDirKey;
      }
    }

    /// <summary>
    /// Imports the stated <see cref="LogicProject"/> into zenon Logic.
    /// </summary>
    /// <param name="zenonLogicProject">The project to import into zenon.</param>
    /// <param name="options">Specifies options on how to import the <paramref name="zenonLogicProject"/> into zenon.</param>
    internal bool ImportZenonLogicProject(LogicProject zenonLogicProject, ImportOptions options)
    {
      string xmlFilePathToImport = SerializeZenonLogicProjectToXmlFile(zenonLogicProject);
      string option = "XMLMERGE";
      if (options.HasFlag(ImportOptions.ReCreateVariables))
      {
        option += "-RV";
      }

      if (options.HasFlag(ImportOptions.DoNotMerge))
      {
        option += "-NM";
      }

      string arguments = $"{option} {this.ZenonLogicProjectDirectory} {xmlFilePathToImport}";

      // Import via K5B.exe
      ProcessStartInfo startInfo = new ProcessStartInfo(K5BexeFilePath, arguments)
      {
        CreateNoWindow = false,
        WindowStyle = ProcessWindowStyle.Hidden
      };

      using (Process stratonXmlImportProcess = new Process { StartInfo = startInfo })
      {
        try
        {
          stratonXmlImportProcess.Start();
          stratonXmlImportProcess.WaitForExit();
        }
        catch (Exception e)
        {
          throw new InvalidOperationException(Strings.K5BXmlImportFailedException, e);
        }
      }

      // needed for import using K5B.exe as these parts get not imported
      WriteAppliXmlFile(zenonLogicProject);
      WriteGlobalDefinesFile(zenonLogicProject);

      return true;
    }

    internal bool TryApplyCompilerSettings(LogicProject zenonLogicProject, ImportOptions options = ImportOptions.Default)
    {
      if (zenonLogicProject?.Settings == null)
      {
        return false;
      }

      var settings = zenonLogicProject.Settings.CompilerSettings.CompilerOptions.OptionTuples ?? Enumerable.Empty<LogicOptionTuple>();
      bool allSucceeded = TryApplyCompilerSettings(settings);

      // According to CD-FR the following three setting sections shall never be set manually.
      //allSucceeded
      //  = TryApplySettings(zenonLogicProject.Settings.CompilerSettings.SimulationCodeOptions.OptionTuples ?? Enumerable.Empty<LogicOptionTuple>())
      //  && allSucceeded;
      //allSucceeded
      //  = TryApplySettings(zenonLogicProject.Settings.CompilerSettings.TargetCodeOptions.OptionTuples ?? Enumerable.Empty<LogicOptionTuple>())
      //  && allSucceeded;

      uint cycleTime = zenonLogicProject.Settings.TriggerTime.CycleTime;
      if (cycleTime == 0)
      {
        cycleTime = 10000; // 10 seconds as the default for invalid values
      }

      allSucceeded = SetOption("CycleTime", cycleTime.ToString()) && allSucceeded;

      if (!options.HasFlag(ImportOptions.ApplyOnlineSettings))
      {
        return allSucceeded;
      }

      return allSucceeded;
    }

    private bool TryApplyCompilerSettings(IEnumerable<LogicOptionTuple> options)
    {
      bool allSucceeded = true;
      foreach (var optionTuple in options)
      {
        if (optionTuple.Name == "comment" || optionTuple.Name == "target")
        {
          // Seems to never work
          continue;
        }

        if (!string.IsNullOrWhiteSpace(optionTuple.Name) && !string.IsNullOrWhiteSpace(optionTuple.Value))
        {
          allSucceeded = SetOption(optionTuple.Name, optionTuple.Value) && allSucceeded;
        }
      }

      return allSucceeded;
    }

    internal void TryApplyOnlineChangeSettings(LogicProject zenonLogicProject, ImportOptions options = ImportOptions.Default)
    {
      if (!options.HasFlag(ImportOptions.ApplyOnlineSettings))
      {
        return;
      }

      var optionTuples = zenonLogicProject.Settings.OnlineChangeSettings.OptionTuples ?? Enumerable.Empty<LogicOptionTuple>();

      const uint callback = 0x400;
      IntPtr hwnd = Process.GetCurrentProcess().MainWindowHandle;
      string clientName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
      string hotSizeBuffer = string.Empty;
      bool enableHot = false;
      bool hasHotOptionDefined = false;

      using (K5SrvWrapper srv = K5SrvWrapper.TryConnect(hwnd, callback, zenonLogicProject.Path, clientName, K5SrvConstants.K5DbSelfNotif))
      {
        while (!srv.IsReady)
        {
          Thread.Sleep(100);
        }

        foreach (var optionTuple in optionTuples)
        {
          if (!string.IsNullOrWhiteSpace(optionTuple.Name) && !string.IsNullOrWhiteSpace(optionTuple.Value))
          {
            if (optionTuple.Name.StartsWith("size_"))
            {
              hotSizeBuffer = hotSizeBuffer + optionTuple.Name.Replace("size_", "") + "=" + optionTuple.Value + ",";
            }

            else if (optionTuple.Name.Equals("enable"))
            {
              hasHotOptionDefined = true;
              if (int.TryParse(optionTuple.Value, out var i))
              {
                enableHot = i > 0;
              }
              else if (bool.TryParse(optionTuple.Value, out var b))
              {
                enableHot = b;
              }
              else if (optionTuple.Value.Equals("ON", StringComparison.OrdinalIgnoreCase))
              {
                enableHot = true;
              }
              else if (optionTuple.Value.Equals("OFF", StringComparison.OrdinalIgnoreCase))
              {
                enableHot = false;
              }
            }
          }
        }

        if (hasHotOptionDefined)
        {
          srv.SetHot(enableHot);
        }

        if (hotSizeBuffer.Length > 0)
        {
          srv.SetHotSizing(hotSizeBuffer);
        }
      }
    }


    private bool SetSrvOption(K5SrvWrapper srv, K5SrvConstants.K5DbProperty property, string value)
    {
      return srv.SetProperty(srv.ProjectHandle, property, value);
    }

    /// <summary>
    /// Writes the appli.xml content directly into the file.
    /// </summary>
    /// <remarks>
    /// This is done because import via k5b.exe omitts the folder structures in the application tree.
    /// </remarks>
    /// <param name="zenonLogicProject"></param>
    private void WriteAppliXmlFile(LogicProject zenonLogicProject)
    {
      string appliXmlFilePathToImport = TemporaryFileCreator.GetRandomTemporaryFilePathWithExtension("xml");
      zenonLogicProject.ApplicationTree.ExportAsFile(appliXmlFilePathToImport, LogicXmlEncoding);

      var appliFilePath = Path.Combine(this.ZenonLogicProjectDirectory, "appli.xml");

      try
      {
        if (File.Exists(appliFilePath))
        {
          File.Delete(appliFilePath);
        }

        File.Copy(appliXmlFilePathToImport, appliFilePath);
      }
      catch (IOException e)
      {
        throw new InvalidOperationException(string.Format(Strings.AppliFileWriteIOException, appliFilePath), e);
      }
      catch (Exception e)
      {
        throw new InvalidOperationException(String.Format(Strings.AppliFileWriteException, appliFilePath), e);
      }
    }

    /// <summary>
    /// Writes the appli.EQV content directly into the file.
    /// </summary>
    /// <remarks>
    /// This is done because the import via k5b.exe omitts global definitions.
    /// </remarks>
    /// <param name="zenonLogicProject"></param>
    private void WriteGlobalDefinesFile(LogicProject zenonLogicProject)
    {
      string globalDefinesFilePath = TemporaryFileCreator.GetRandomTemporaryFilePathWithExtension("EQV");
      LogicDefine globalDefine = zenonLogicProject.LogicDefinitions.Defines
        .FirstOrDefault(define => define.Name.Equals(Strings.GlobalDefineName));

      if (globalDefine == null || string.IsNullOrWhiteSpace(globalDefine.DefineContent))
      {
        return;
      }

      File.WriteAllText(globalDefinesFilePath, globalDefine.DefineContent);

      var appliEqvFilePath = Path.Combine(this.ZenonLogicProjectDirectory, "appli.EQV");
      if (File.Exists(appliEqvFilePath))
      {
        File.Delete(appliEqvFilePath);
      }

      File.Copy(globalDefinesFilePath, appliEqvFilePath);
    }

    private string SerializeZenonLogicProjectToXmlFile(LogicProject zenonLogicProject)
    {
      string xmlFilePathToImport = TemporaryFileCreator.GetRandomTemporaryFilePathWithExtension("xml");
      zenonLogicProject.ExportAsFile(xmlFilePathToImport, LogicXmlEncoding);
      return xmlFilePathToImport;
    }

    /// <summary>
    /// Creates a default zenon Logic project
    /// </summary>
    /// <returns></returns>
    internal void CreateDefaultZenonLogicProject()
    {
      bool commandSuccessful = ExecuteK5PrpCommand("CreateProject", out string returnMessage, out _);
      if (!commandSuccessful)
      {
        throw new Exception(returnMessage);
      }

      // It is crucial to call this here, otherwise the logic project gets corrupted if a K5Srv access is attempted
      commandSuccessful = ExecuteK5PrpCommand("CloseProject", out returnMessage, out _);
      if (!commandSuccessful)
      {
        throw new Exception(returnMessage);
      }
    }

    /// <summary>
    /// XML export of the zenon Logic project to the stated XML file.
    /// </summary>
    /// <param name="xmlExportFilePath">XML file path which is used for export.</param>
    /// <returns>Exception if error occurs during export.</returns>
    internal bool ExportZenonLogicProjectAsXml(string xmlExportFilePath)
    {
      // export via K5Prp.dll call
      //bool commandSuccessful = ExecuteK5PrpCommand($"XmlExport {xmlExportFilePath}", out string returnMessage, out _);

      //if (!commandSuccessful)
      //{
      //  throw new Exception(returnMessage);
      //}

      //return true;

      // export via K5B.exe
      ProcessStartInfo startInfo = new ProcessStartInfo(K5BexeFilePath,
          $"X {this.ZenonLogicProjectDirectory} {Strings.K5BxmlExportFormatString} {xmlExportFilePath}")
      { CreateNoWindow = false, WindowStyle = ProcessWindowStyle.Hidden };

      using (Process stratonXmlExportProcess = new Process { StartInfo = startInfo })
      {
        try
        {
          stratonXmlExportProcess.Start();
          stratonXmlExportProcess.WaitForExit();
        }
        catch (Exception e)
        {
          throw new InvalidOperationException(Strings.K5BXmlExportFailedException, e);
        }
      }

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
        Debug.Write("DBG: K5PRP Call: " + k5Command + ":");

        uint dwOk = 0;
        uint dwDataIn = 0;
        uint dwDataOut = 0;

        IntPtr commandResult = K5PCall(ZenonLogicProjectDirectory, k5Command, ref dwOk, ref dwDataIn, ref dwDataOut);

        returnMessage = Marshal.PtrToStringAnsi(commandResult);
        Marshal.FreeBSTR(commandResult);
        bool executedSuccessfully = Convert.ToBoolean(dwOk);
        outHandle = dwDataOut;

        Debug.WriteLine($"DBG: Success: {executedSuccessfully}: Message: {returnMessage}");
        return executedSuccessfully;
      }
      catch (Exception ex)
      {
        Debug.WriteLine("DBG: Call failed");

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
      if (fileName.Contains("AddIn")) // We are in the Debugger...
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

    /// <summary>
    /// Compiles the stated zenon Logic project.
    /// </summary>
    /// <param name="zenonLogicProject"></param>
    /// <param name="compilerOutputText">
    /// Contains the output messages of the compilation process. Null if the if retrieving the compiler output failed.
    /// </param>
    internal void CompileZenonLogicProject(LogicProject zenonLogicProject, out IEnumerable<string> compilerOutputText)
    {
      ProcessStartInfo startInfo = new ProcessStartInfo(K5BexeFilePath, $"BUILD {this.ZenonLogicProjectDirectory}")
      {
        CreateNoWindow = false,
        WindowStyle = ProcessWindowStyle.Hidden
      };

      using (Process stratonCompileProcess = new Process { StartInfo = startInfo })
      {
        try
        {
          stratonCompileProcess.Start();
          stratonCompileProcess.WaitForExit();

          compilerOutputText = ReadCompileLogFileOfZenonLogicProject(zenonLogicProject);
        }
        catch (Exception e)
        {
          throw new InvalidOperationException(string.Format(Strings.K5BCompileFailedException, zenonLogicProject.Path), e);
        }
      }
    }

    /// <summary>
    /// Reads the __build.log file of the stated zenon Logic project.
    /// The file contains the output messages of the last compilation process.
    /// </summary>
    /// <param name="zenonLogicProject"></param>
    /// <returns>Returns null if __build.log does not exist. Happens if the projects was never compiled.</returns>
    private IEnumerable<string> ReadCompileLogFileOfZenonLogicProject(LogicProject zenonLogicProject)
    {
      string compilerOutputFilePath = Path.Combine(ZenonLogicProjectDirectory, ZenonLogicCompileLogFileName);
      if (!File.Exists(compilerOutputFilePath))
      {
        return null;
      }

      try
      {
        return File.ReadAllLines(compilerOutputFilePath);
      }
      catch (Exception e)
      {
        throw new FileLoadException(string.Format(Strings.ZenonLogicCompileLogFileReadException, zenonLogicProject.Path), e);
      }
    }
  }
}
