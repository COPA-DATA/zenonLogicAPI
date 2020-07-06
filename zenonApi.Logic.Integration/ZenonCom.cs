using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Xml.Linq;
using zenOn;
using zenonApi.Logic;
using zenonApi.Logic.Ini;
using zenonApi.Zenon.Helper;
using zenonApi.Zenon.K5Prp;
using zenonApi.Zenon.StratonUtilities;

namespace zenonApi.Zenon
{
  [DebuggerDisplay("{" + nameof(ZenonProjectName) + "}")]
  public class ZenonCom : IDisposable
  {
    public IProject ZenonProject { get; private set; }
    public string ZenonProjectName { get; private set; }
    public string ZenonProjectGuid { get; private set; }
    /// <summary>
    /// Directory of the zenon editor project.
    /// </summary>
    /// <example> C:\ProgramData\COPA-DATA\SQL2012\83fe2bc7-6182-4652-9e48-3b71257b9851\FILES </example>
    public string ZenonProjectDirectory { get; private set; }
    /// <summary>
    /// Directory of the zenon Logic projects which belong to the zenon project.
    /// </summary>
    /// <example> C:\ProgramData\COPA-DATA\SQL2012\83fe2bc7-6182-4652-9e48-3b71257b9851\FILES\straton </example>
    public string ZenonLogicDirectory => Path.Combine(ZenonProjectDirectory, "straton");

    /// <summary>
    /// Sequence of loaded zenon Logic projects.
    /// </summary>
    public ObservableCollection<LogicProject> LogicProjects { get; private set; } = new ObservableCollection<LogicProject>();
    public static HashSet<uint> AllUsedPorts { get; private set; } = new HashSet<uint>();

    public ZenonCom(IProject zenonProject)
    {
      ZenonProject = zenonProject ?? throw new ArgumentNullException(string.Format(Strings.ZenonProjectReferenceNull,
                       nameof(zenonProject)));

      GetZenonProjectInformation(ZenonProject);

      // if this folder path does not exist there can not be a zenon Logic project to load
      if (Directory.Exists(ZenonLogicDirectory))
      {
        LogicProjects = new ObservableCollection<LogicProject>(LoadZenonLogicProjects());
        foreach (LogicProject logicProject in LogicProjects.Where(project => File.Exists(project.K5DbxsIniFilePath)))
        {
          AllUsedPorts.Add(logicProject.MainPort);
        }
      }
      else
      {
        // to make sure that the ...\straton\... folder exists which is not existing in a default zenon project directory
        Directory.CreateDirectory(ZenonLogicDirectory);
      }

      LogicProjects.CollectionChanged += UpdateStratonDirectoryOfPathOnItemAdded;
    }

    /// <summary>
    /// Imports the zenon Logic project with specified project name into zenon
    /// </summary>
    /// <param name="zenonLogicProjectName"></param>
    public void ImportLogicProjectIntoZenonByName(string zenonLogicProjectName)
    {
      if (string.IsNullOrWhiteSpace(zenonLogicProjectName))
      {
        throw new ArgumentNullException(string.Format(Strings.MethodArgumentNullException,
          nameof(zenonLogicProjectName), nameof(ImportLogicProjectIntoZenonByName)));
      }

      IEnumerable<LogicProject> logicProjectsWithSearchedNames = LogicProjects.Where(logicProject =>
        logicProject.ProjectName.Equals(zenonLogicProjectName));

      if (!logicProjectsWithSearchedNames.Any())
      {
        throw new InstanceNotFoundException(string.Format(Strings.LogicProjectWithSpecifiedProjectNameNotFound,
          zenonLogicProjectName));
      }

      ImportLogicProjectsIntoZenon(logicProjectsWithSearchedNames);
    }

    /// <summary>
    /// Imports all zenon Logic projects which are stored in <see cref="LogicProjects"/> into zenon
    /// </summary>
    public void ImportLogicProjectsIntoZenon()
    {
      ImportLogicProjectsIntoZenon(LogicProjects);
    }

    /// <summary>
    /// Returns zenon Logic project reference for the stated project name.
    /// </summary>
    /// <param name="zenonLogicProjectName"></param>
    /// <returns></returns>
    public LogicProject GetLogicProjectByName(string zenonLogicProjectName)
    {
      if (string.IsNullOrEmpty(zenonLogicProjectName))
      {
        throw new ArgumentNullException(string.Format(Strings.ErrorGettingZenonProjektByNameArgumentNull,
          nameof(zenonLogicProjectName)));
      }

      IEnumerable<LogicProject> foundLogicProjectsByName = LogicProjects.Where(project => string.Equals(project.ProjectName,
        zenonLogicProjectName));

      if (!foundLogicProjectsByName.Any())
      {
        throw new NullReferenceException(string.Format(Strings.ErrorNoZenonLogicProjectFoundForName, zenonLogicProjectName));
      }

      if (foundLogicProjectsByName.Count() > 1)
      {
        throw new InvalidDataException(string.Format(Strings.ErrorDuplicateLogicProjectFoundForName, zenonLogicProjectName));
      }

      return foundLogicProjectsByName.First();
    }

    /// <summary>
    /// Compiles the stated zenon Logic project.
    /// </summary>
    /// <param name="zenonLogicProjectToCompile"></param>
    /// <param name="compilerOutputText">
    /// Contains the output messages of the compilation process. Null if the if retrieving the compiler output failed.
    /// </param>
    public void CompileZenonLogicProject(LogicProject zenonLogicProjectToCompile, out IEnumerable<string> compilerOutputText)
    {
      if (!Directory.Exists(zenonLogicProjectToCompile.Path))
      {
        throw new DirectoryNotFoundException
          ($"{Strings.ZenonLogicProjectDirectoryNotFound} {Strings.ZenonLogicHintForMissingDirectory}");
      }

      K5ToolSet k5ToolSet = new K5ToolSet(zenonLogicProjectToCompile.Path);
      k5ToolSet.CompileZenonLogicProject(zenonLogicProjectToCompile, out compilerOutputText);
    }

    /// <summary>
    /// Imports the stated zenon Logic projects into zenon Logic.
    /// As an import requires an existing project it trys to create a default project first.
    /// </summary>
    /// <param name="zenonLogicProjectsToImport"></param>
    private void ImportLogicProjectsIntoZenon(IEnumerable<LogicProject> zenonLogicProjectsToImport, bool reloadZenonProject = true)
    {
      foreach (LogicProject logicProject in zenonLogicProjectsToImport)
      {
        K5ToolSet k5ToolSet = new K5ToolSet(logicProject.Path);

        if (!logicProject.Path.Contains(this.ZenonProjectGuid))
        {
          logicProject.ModifyStratonDirectoryPartOfPath(Path.Combine(this.ZenonLogicDirectory, logicProject.ProjectName));
        }
        // as there is no built in solution to check if a project exists this check is used to determine if a 
        // certain project already exists in zenon Logic
        if (!Directory.Exists(logicProject.Path))
        {
          // create default zenon Logic project as XML import requires a project to exist
          k5ToolSet.CreateDefaultZenonLogicProject();

          string nextFreeZenonLogicMainPort = GetNextFreeZenonLogicMainPort();
          // free main port has to be used as parameter here as it is not yet set for the new zenon Logic project in the 
          // k5dbxs ini file.
          string newStratonNgDriverId = CreateStratonNgDriverForZenonLogicProject(logicProject.ProjectName,
            nextFreeZenonLogicMainPort);

          K5DbxsIniFile.CreateK5DbxsIniFile(this.ZenonProjectGuid, logicProject.K5DbxsIniFilePath, nextFreeZenonLogicMainPort,
            newStratonNgDriverId);
        }

        k5ToolSet.ImportZenonLogicProject(logicProject);
      }

      if (reloadZenonProject)
      {
        // The following line is just necessary to force zenon to reload the logic projects within the displayed list.
        ZenonProject.Parent.Parent.MyWorkspace.LoadProject(ZenonProjectGuid);
      }
    }

    /// <summary>
    /// Creates a StratonNG driver for the current zenon project and configures it by setting the specified parameters
    /// in the config file.
    /// </summary>
    /// <param name="zenonLogicProjectName"></param>
    /// <param name="nextFreeZenonLogicMainPort"></param>
    /// <returns>Returns the driver ID of the created StratonNG driver.</returns>
    private string CreateStratonNgDriverForZenonLogicProject(string zenonLogicProjectName, string nextFreeZenonLogicMainPort)
    {
      // If a driver configuration file with the same name as it would get created here already exists it has to be
      // deleted manually beforehand. This scenario occurs when a zenon Logic project gets deleted via the GUI as zenon
      // does not remove the driver configuration file which belongs to a driver that belongs to a zenon Logic project.
      DeleteExistingStratonNgDriverConfigFile(zenonLogicProjectName);

      IDriver newStratonNgDriver = ZenonProject.Drivers().CreateDriverEx($"zenon Logic: {zenonLogicProjectName}", "STRATONNG", false);

      newStratonNgDriver.OpenConfig();
      newStratonNgDriver.CreateDynProperty("DrvConfig.Connections");

      newStratonNgDriver.DynProperties["DrvConfig.Connections.ConnectionName"] = zenonLogicProjectName;
      newStratonNgDriver.DynProperties["DrvConfig.Connections.PrimaryTCPPort"] = nextFreeZenonLogicMainPort;

      newStratonNgDriver.DynProperties["Description"] = Strings.StratonNgDriverDescription;
      string ret = newStratonNgDriver.DynProperties["DriverId"].ToString();
      newStratonNgDriver.CloseConfig(true);
      return ret;
    }

    /// <summary>
    /// Gets the next available mainport number by querying the already taken port numbers and returning the next
    /// highest port number.
    /// </summary>
    /// <returns></returns>
    private string GetNextFreeZenonLogicMainPort()
    {
      // if none of the stated logic projects really exist in zenon we asume that all of them were configured within
      // this application session and have to be created. In the case that there is no zenon logic project we can
      // return with 1200 because this is the default main port which gets used for the first created zenon logic
      // project in a zenon project
      if (!AllUsedPorts.Any())
      {
        AllUsedPorts.Add(1200);
        return 1200.ToString();
      }
      uint highestTakenPortNumber = AllUsedPorts.Max();
      uint nextFreePortNumber = highestTakenPortNumber + 1;
      AllUsedPorts.Add(nextFreePortNumber);
      return nextFreePortNumber.ToString();
    }

    private IEnumerable<LogicProject> LoadZenonLogicProjects()
    {
      foreach (var zenonLogicProjectDirectory in StratonFolderPath.GetZenonLogicProjectFolderPaths(this.ZenonLogicDirectory))
      {
        K5ToolSet k5ToolSet = new K5ToolSet(zenonLogicProjectDirectory);
        string randomXmlFilePath = TemporaryFileCreator.GetRandomTemporaryFilePathWithExtension("xml");
        k5ToolSet.ExportZenonLogicProjectAsXml(randomXmlFilePath);

        XDocument logicProjectXmlExport = XDocument.Load(randomXmlFilePath);
        // initialization of the zenon logic project data model
        LogicProject logicProject = LogicProject.Import(logicProjectXmlExport.Element(Strings.K5XmlExportRootNodeName)); //TODO: hide .Element call

        yield return logicProject;
      }
    }

    private void GetZenonProjectInformation(IProject zenonProject)
    {
      ZenonProjectName = zenonProject.Name;
      ZenonProjectGuid = zenonProject.Guid;
      ZenonProjectDirectory = zenonProject.Path;
    }

    /// <summary>
    /// Updates the zenon Logic project path property so it is compliant with the path of the zenon project to which
    /// it was assigned.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateStratonDirectoryOfPathOnItemAdded(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (!(e.Action is NotifyCollectionChangedAction.Add))
      {
        return;
      }

      foreach (var newItem in e.NewItems)
      {
        if (newItem is LogicProject newZenonLogicProject)
        {
          newZenonLogicProject.ModifyStratonDirectoryPartOfPath(ZenonLogicDirectory);
          if (File.Exists(newZenonLogicProject.K5DbxsIniFilePath))
          {
            AllUsedPorts.Add(newZenonLogicProject.MainPort);
          }
        }
      }
    }

    /// <summary>
    /// Deletes the configuration file of a straton NG driver which belongs to the stated zenon Logic project.
    /// </summary>
    /// <param name="zenonLogicProjectName"></param>
    private void DeleteExistingStratonNgDriverConfigFile(string zenonLogicProjectName)
    {
      // the whitespace character between the ZenonStratonNgDriverConfigFilePrefix and the zenonLogicProjectName
      // is required
      string driverConfigFilePath = Path.Combine(ZenonProject.ModulePath[tpModulePaths.tpPathDrivers],
        $"{Strings.ZenonStratonNgDriverConfigFilePrefix} {zenonLogicProjectName}{Strings.TextFileExtension}");
      if (File.Exists(driverConfigFilePath))
      {
        File.Delete(driverConfigFilePath);
      }
    }

    public void Dispose()
    {
      // delete temporary files which were created during lifetime
      TemporaryFileCreator.CleanupTemporaryFiles();
      // detach event handler
      if (LogicProjects != null)
      {
        LogicProjects.CollectionChanged -= UpdateStratonDirectoryOfPathOnItemAdded;
      }
    }
  }
}
