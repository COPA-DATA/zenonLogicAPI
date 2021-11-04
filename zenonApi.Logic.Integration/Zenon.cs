using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using zenonApi.Logic;
using zenonApi.Logic.Ini;
using zenonApi.Zenon.Helper;
using zenonApi.Zenon.K5Prp;
using zenonApi.Zenon.StratonUtilities;


namespace zenonApi.Zenon
{
  [DebuggerDisplay("{" + nameof(ZenonProjectName) + "}")]
  public class Zenon : IDisposable
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
    ///   Sequence of lazy loaded zenon Logic projects.
    ///   To add a new project, use <see cref="LazyLogicProjects"/>.
    /// </summary>
    public ObservableCollection<LazyLogicProject> LazyLogicProjects { get; private set; } = new ObservableCollection<LazyLogicProject>();

    /// <summary>
    ///   Sequence of loaded zenon Logic projects.
    /// </summary>
    public IReadOnlyList<LogicProject> LogicProjects => LazyLogicProjects.Select(x => x.Value).ToList();

    public static HashSet<uint> AllUsedPorts { get; private set; } = new HashSet<uint>();

    public Zenon(IProject zenonProject)
    {
      ZenonProject = zenonProject ?? throw new ArgumentNullException(string.Format(Strings.ZenonProjectReferenceNull,
                       nameof(zenonProject)));

      GetZenonProjectInformation(ZenonProject);

      // if this folder path does not exist there can not be a zenon Logic project to load
      if (Directory.Exists(ZenonLogicDirectory))
      {
        LazyLogicProjects = new ObservableCollection<LazyLogicProject>(LoadZenonLogicProjects());
        foreach (var logicProject in LazyLogicProjects.Where(project => File.Exists(project.K5DbxsIniFilePath)))
        {
          AllUsedPorts.Add(logicProject.MainPort);
        }
      }
      else
      {
        // to make sure that the ...\straton\... folder exists which is not existing in a default zenon project directory
        Directory.CreateDirectory(ZenonLogicDirectory);
      }

      LazyLogicProjects.CollectionChanged += UpdateStratonDirectoryOfPathOnItemAdded;
    }

    /// <summary>
    ///   Imports the zenon Logic project with specified project name into zenon.
    /// </summary>
    /// <param name="zenonLogicProjectName">The name of the zenon Logic project.</param>
    /// <param name="reloadZenonProject">Specifies if the current zenon project shall be reloaded after the import.</param>
    /// <param name="options">Specifies options on how to import the projects into zenon.</param>
    public void ImportLogicProjectIntoZenonByName(
      string zenonLogicProjectName,
      bool reloadZenonProject = true,
      ImportOptions options = ImportOptions.Default)
    {
      if (string.IsNullOrWhiteSpace(zenonLogicProjectName))
      {
        throw new ArgumentNullException(string.Format(Strings.MethodArgumentNullException,
          nameof(zenonLogicProjectName), nameof(ImportLogicProjectIntoZenonByName)));
      }

      IEnumerable<LogicProject> logicProjectsWithSearchedNames
        = LazyLogicProjects
          .Where(lazyLogicProject => lazyLogicProject.ProjectName?.Equals(zenonLogicProjectName) ?? false)
          .Select(project => project.Value);

      if (!logicProjectsWithSearchedNames.Any())
      {
        throw new InstanceNotFoundException(
          string.Format(Strings.LogicProjectWithSpecifiedProjectNameNotFound, zenonLogicProjectName));
      }

      ImportLogicProjectsIntoZenon(logicProjectsWithSearchedNames, reloadZenonProject, options);
    }

    /// <summary>
    /// Imports all zenon Logic projects which are stored in <see cref="LogicProjects"/> into zenon.
    /// </summary>
    /// <param name="reloadZenonProject">Specifies if the current zenon project shall be reloaded after the import.</param>
    /// <param name="options">Specifies options on how to import the project into zenon.</param>
    public void ImportLogicProjectsIntoZenon(bool reloadZenonProject = true, ImportOptions options = ImportOptions.Default)
    {
      ImportLogicProjectsIntoZenon(LogicProjects, reloadZenonProject, options);
    }

    /// <summary>
    /// Returns zenon Logic project reference for the stated project name.
    /// </summary>
    /// <param name="zenonLogicProjectName">The zenon Logic project to load.</param>
    public LogicProject GetLogicProjectByName(string zenonLogicProjectName)
    {
      if (string.IsNullOrEmpty(zenonLogicProjectName))
      {
        throw new ArgumentNullException(
          string.Format(Strings.ErrorGettingZenonProjektByNameArgumentNull, nameof(zenonLogicProjectName)));
      }

      IEnumerable<LazyLogicProject> foundLogicProjectsByName
        = LazyLogicProjects.Where(project => string.Equals(project.ProjectName, zenonLogicProjectName));

      if (!foundLogicProjectsByName.Any())
      {
        throw new NullReferenceException(string.Format(Strings.ErrorNoZenonLogicProjectFoundForName, zenonLogicProjectName));
      }

      if (foundLogicProjectsByName.Count() > 1)
      {
        throw new InvalidDataException(string.Format(Strings.ErrorDuplicateLogicProjectFoundForName, zenonLogicProjectName));
      }

      return foundLogicProjectsByName.First().Value;
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
    /// As an import requires an existing project it tries to create a default project first.
    /// </summary>
    /// <param name="zenonLogicProjectsToImport">The zenon Logic projects to import.</param>
    /// <param name="reloadZenonProject">Specifies if the current zenon project shall be reloaded after the import.</param>
    /// <param name="options">Specifies options on how to import the <paramref name="zenonLogicProjectsToImport"/> into zenon.</param>
    private void ImportLogicProjectsIntoZenon(
      IEnumerable<LogicProject> zenonLogicProjectsToImport,
      bool reloadZenonProject,
      ImportOptions options)
    {
      EnsureSupportedVersion(options);
      foreach (LogicProject logicProject in zenonLogicProjectsToImport)
      {
        if (!logicProject.Path.Contains(this.ZenonProjectGuid))
        {
          logicProject.ModifyStratonDirectoryPartOfPath(this.ZenonLogicDirectory);
        }

        K5ToolSet k5ToolSet = new K5ToolSet(logicProject.Path);

        if (!logicProject.Path.Contains(this.ZenonProjectGuid))
        {
          logicProject.ModifyStratonDirectoryPartOfPath(this.ZenonLogicDirectory);
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

        k5ToolSet.ImportZenonLogicProject(logicProject, options);
        // Due to a change in zenon Logic 10, the compiler settings and further other options need to be set explicitly.
        k5ToolSet.TryApplyCompilerSettings(logicProject, options);
        k5ToolSet.TryApplyOnlineChangeSettings(logicProject, options);
      }

      if (reloadZenonProject)
      {
        // The following line is just necessary to force zenon to reload the logic projects within the displayed list.
        var workspace = ZenonProject.Parent.Parent.Workspace;
        workspace.UnloadProject(ZenonProjectGuid, true);
        workspace.LoadProject(ZenonProjectGuid);
      }
    }

    private void EnsureSupportedVersion(ImportOptions options)
    {
      if (options == ImportOptions.Default)
      {
        return;
      }

      var version = ZenonProject.Parent.Parent.VersionNumber;
      if (version >= 10000)
      {
        return;
      }

      throw new ArgumentException(
        $"The given import option requires zenon in version 10 or higher. "
        + $"Use {nameof(ImportOptions)}.{nameof(ImportOptions.Default)} instead for lower versions.",
        nameof(options));
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

      IDriver newStratonNgDriver = this.ZenonProject.DriverCollection
        .Create($"zenon Logic: {zenonLogicProjectName}", "STRATONNG", false);

      newStratonNgDriver.InitializeConfiguration();
      newStratonNgDriver.CreateDynamicProperty("DrvConfig.Connections");

      newStratonNgDriver.SetDynamicProperty("DrvConfig.Connections.ConnectionName", zenonLogicProjectName);
      newStratonNgDriver.SetDynamicProperty("DrvConfig.Connections.PrimaryTCPPort", nextFreeZenonLogicMainPort);

      newStratonNgDriver.SetDynamicProperty("Description", Strings.StratonNgDriverDescription);

      newStratonNgDriver.EndConfiguration(true);

      return newStratonNgDriver.GetDynamicProperty("DriverId").ToString();
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

    /// <summary>
    /// Gets a lazy loaded list of all logic projects.
    /// </summary>
    /// <returns>List of lazy loaded logic projects</returns>
    private IEnumerable<LazyLogicProject> LoadZenonLogicProjects()
    {
      List<LazyLogicProject> projects = new List<LazyLogicProject>();
      foreach (var zenonLogicProjectDirectory in StratonFolderPath.GetZenonLogicProjectFolderPaths(this.ZenonLogicDirectory))
      {
        LazyLogicProject lazyProject = new LazyLogicProject(StratonFolderPath.GetZenonLogicProjectFolderName(zenonLogicProjectDirectory), zenonLogicProjectDirectory);
        projects.Add(lazyProject);
      }

      return projects;
    }

    private void GetZenonProjectInformation(IProject zenonProject)
    {
      ZenonProjectName = zenonProject.Name;
      ZenonProjectGuid = zenonProject.ProjectId;
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
      string driverConfigFilePath = Path.Combine(ZenonProject.GetFolderPath(FolderPath.Drivers),
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
        LazyLogicProjects.CollectionChanged -= UpdateStratonDirectoryOfPathOnItemAdded;
      }
    }
  }
}
