using Scada.AddIn.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using zenonApi.Logic.Integration.Helper;
using zenonApi.Logic.Integration.K5Prp;
using zenonApi.Logic.Integration.StratonUtilities;

namespace zenonApi.Logic.Integration
{
  [DebuggerDisplay("zenon project name: {" + nameof(ZenonProjectName) + "}")]
  public abstract class ZenonLogicIntegration
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
    public IEnumerable<LogicProject> LogicProjects { get; private set; }

    protected ZenonLogicIntegration(IProject zenonProject)
    {
      ZenonProject = zenonProject ?? throw new ArgumentNullException(string.Format(Strings.ZenonProjectReferenceNull,
                       nameof(zenonProject)));

      GetZenonProjectInformation(ZenonProject);

      // if this folder path does not exist there can not be a zenon Logic project to load
      if (Directory.Exists(ZenonLogicDirectory))
      {
        LogicProjects = LoadZenonLogicProjects();
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
        LogicProject logicProject = LogicProject.Import(logicProjectXmlExport.Element("K5project")); //TODO: hide .Element call

        yield return logicProject;
      }
    }

    private void GetZenonProjectInformation(IProject zenonProject)
    {
      ZenonProjectName = zenonProject.Name;
      ZenonProjectGuid = zenonProject.ProjectId;
      ZenonProjectDirectory = zenonProject.Path;
    }
  }
}
