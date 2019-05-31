using Scada.AddIn.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Xml.Linq;
using zenonApi.Logic.Integration.Helper;
using zenonApi.Logic.Integration.K5Prp;
using zenonApi.Logic.Integration.StratonUtilities;

namespace zenonApi.Logic.Integration
{
  [DebuggerDisplay("{" + nameof(ZenonProjectName) + "}")]
  public class Zenon : IDisposable
  {
    private IProject ZenonProject { get; }
    protected string ZenonProjectName { get; private set; }
    protected string ZenonProjectGuid { get; private set; }
    /// <summary>
    /// Directory of the zenon editor project.
    /// </summary>
    /// <example> C:\ProgramData\COPA-DATA\SQL2012\83fe2bc7-6182-4652-9e48-3b71257b9851\FILES </example>
    protected string ZenonProjectDirectory { get; private set; }
    /// <summary>
    /// Directory of the zenon Logic projects which belong to the zenon project.
    /// </summary>
    /// <example> C:\ProgramData\COPA-DATA\SQL2012\83fe2bc7-6182-4652-9e48-3b71257b9851\FILES\straton </example>
    protected string ZenonLogicDirectory => Path.Combine(ZenonProjectDirectory, "straton");

    /// <summary>
    /// Sequence of loaded zenon Logic projects.
    /// </summary>
    public ObservableCollection<LogicProject> LogicProjects { get; private set; } = new ObservableCollection<LogicProject>();

    public Zenon(IProject zenonProject)
    {
      ZenonProject = zenonProject ?? throw new ArgumentNullException(string.Format(Strings.ZenonProjectReferenceNull,
                       nameof(zenonProject)));

      GetZenonProjectInformation(ZenonProject);

      // if this folder path does not exist there can not be a zenon Logic project to load
      if (Directory.Exists(ZenonLogicDirectory))
      {
        LogicProjects = new ObservableCollection<LogicProject>(LoadZenonLogicProjects().ToList());
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
    /// Imports the stated zenon Logic projects into zenon Logic.
    /// As an import requires an existing project it trys to create a default project first.
    /// </summary>
    /// <param name="zenonLogicProjectsToImport"></param>
    private void ImportLogicProjectsIntoZenon(IEnumerable<LogicProject> zenonLogicProjectsToImport)
    {
      foreach (LogicProject logicProject in zenonLogicProjectsToImport)
      {
        K5ToolSet k5ToolSet = new K5ToolSet(logicProject.Path);

        // as there is no built in solution to check if a project exists this check is used to determine if a 
        // certain project already exists in zenon Logic
        if (!Directory.Exists(logicProject.Path))
        {
          // create default zenon Logic project as XML import requires a project to exist
          k5ToolSet.CreateDefaultZenonLogicProject();
        }

        k5ToolSet.ImportZenonLogicProject(logicProject);
      }
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
        }
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
