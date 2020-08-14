using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
using zenonApi.Zenon;
using zenonApi.Zenon.Helper;
using zenonApi.Zenon.K5Prp;

namespace zenonApi.Logic
{
  public class LazyLogicProject : Lazy<LogicProject>
  {
    /// <summary>
    /// Name of the logic project
    /// </summary>
    public string LogicProjectName { get; set; }

    public LazyLogicProject(string name, LogicProject project) : base(() =>  project )
    {
      LogicProjectName = name;

    }

    public LazyLogicProject(string name, string logicPath) : base(() =>
    {
      K5ToolSet k5ToolSet = new K5ToolSet(logicPath);
      string randomXmlFilePath = TemporaryFileCreator.GetRandomTemporaryFilePathWithExtension("xml");
      k5ToolSet.ExportZenonLogicProjectAsXml(randomXmlFilePath);

      XDocument logicProjectXmlExport = XDocument.Load(randomXmlFilePath);
      // initialization of the zenon logic project data model
      LogicProject logicProject = LogicProject.Import(logicProjectXmlExport.Element(Strings.K5XmlExportRootNodeName));

      return logicProject;
    })
    {
      LogicProjectName = name;
    }

  }
}
