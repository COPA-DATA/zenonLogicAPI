using Scada.Common.DynPropertyParser;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CopaData.Tools.AuthorIt.AitConnector;
using zenonApi.MetaDescription.Parser.AdapterAnalysis;
using zenonApi.MetaDescription.Parser.OdlWrapperClasses;
using zenonApi.MetaDescription.Parser.XML;
using G = Scada.Common.OdlParser;

namespace zenonApi.MetaDescription.Parser
{
  class Program
  {

    public static ZenonCom _zenonCom;

    static void Main(string[] args)
    {
      Globals.ZenonVersion = "820";
      Globals.FilePaths.CopyMonoCecilFromG();
      Setup.Login(Globals.Ait.Server, Globals.Ait.Database+"_"+Globals.ZenonVersion, Globals.Ait.User, Globals.Ait.Password);

      Assembly assembly;
      IOrderedEnumerable<IScadaNamespace> orderedNamespaces = Adapter.Adapt();
      var DynDic = Scada.Common.DynPropertyParser.Parser.FromSourceFolder(
        new ParserConfiguration(
          Globals.Tfs.BasePath,
          Globals.Tfs.UserAnalyzerPath,
          Globals.Tfs.CustomSolutionsBasePath,
          Globals.Ait.Server,
          Globals.Ait.Database,
          Globals.Ait.Password,
          Globals.Ait.User,
          DynPropertyTarget.ComAndEmbeddedDriverHelp)
      );
      G.ComHandler comHandler = new G.ComHandler();
      var zenonComWithoutDynProps = comHandler.ReadZenonOdlFile(Globals.Tfs.BasePath);
      _zenonCom = new ZenonCom(zenonComWithoutDynProps, DynDic, orderedNamespaces);

      
      var sortedNamespaces = Adapter.OrderNamespacesIntoDict(orderedNamespaces, Globals.FilePaths.ContratcsDllFilePath);



      Namespaces namespacesForXml = WrappedNamespaces.Parse(sortedNamespaces);
      namespacesForXml.ExportAsFile(@"C:\Users\Lukas.Rieser\OneDrive - COPA-DATA\Documents\XML Api\metadata_generated_all.xml");
    }


    

  }
}
