using System;
using System.IO;

namespace zenonApi.MetaDescription.Parser
{
  public static class Globals
  {
    /*~~~~~~~~~~~~~~~~ Edit with every new version ~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    public static string CurrentZenonVersionNumber = "1000";
    public static string CurrrentAnalzerVersion = "400";
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    public enum HelpType { Addin, Com };
    /// <summary>Shorter name for Environment.NewLine. Used internally in this class.</summary>
    private static string nl = Environment.NewLine;

    public static string NamespaceAddinDll { get; set; } = "Scada.AddIn.Contracts";


    private static string zenonVersion { get; set; }
    public static string ZenonVersion
    {
      get
      {
        if (zenonVersion == "main") return CurrentZenonVersionNumber;
        else return zenonVersion;
      }
      set
      {
        zenonVersion = value;
      }
    }

    private static string analyzerVersion;
    public static string AnalyzerVersion
    {
      get { return analyzerVersion; }
      set { analyzerVersion = value; }
    }

    public static class Tfs
    {
      /// <summary>
      /// Basepath to supervisor folder of currently selected zenon version.
      /// </summary>
      public static string BasePath => TfsFolderPathResolver.Contracts.ResolvePathForVersionAndProduct(zenonVersion, TfsFolderPathResolver.Contracts.Folder.Supervisor);

      public static string UserAnalyzerPath
      {
        get
        {
          var bp = TfsFolderPathResolver.Contracts.ResolvePathForFolder("Scrum");
          return $"{bp}\\Analyzer\\{CurrrentAnalzerVersion}";
        }
      }

      public static string CustomSolutionsBasePath => TfsFolderPathResolver.Contracts.ResolvePathForFolder("zenon") + "\\CustomSolutions";
    }

    /// <summary>
    /// Provides default values and constants required to use the ODL-Parser.
    /// </summary>
    public static class Odl
    {
      /// <summary>The path to the ODL file within the <see cref="BasePath"/>.</summary>
      public static string ZenRt32Path { get; set; } = @"\SOURCE\ZENON\zenOn\RtVbap32\zenrt32.odl";

      /// <summary>The topic ID of the topic in the Author-It database, which holds the translation between DynProperty descriptors
      /// and the display name (required by the ODL parser).</summary>
      public static int ObjectDescriptionCppTopicId { get; set; } = 44611;

    }
    public static class Ait
    {
      // Credentials
      public static string Server { get; set; } = "AuthorIt";
      public static string Database { get; set; } = "CD_Docu";
      public static string User { get; set; } = "DocToolSuite";
      public static string Password { get; set; } = "DocToolSuite";

      // Essential folders
      public static int RootFodler { get; set; } = 2650;
      public static int DynPropRootFolder { get; } = 1328;
      public static int DrvPropRootFolder { get; } = 2635; // TODO: To be changed when on live ait database
      public static int DataTypeLookUpTopic { get; } = 184886;
    }
    public static class FilePaths
    {
      public static void CopyMonoCecilFromG()
      {
        if (ZenonVersion == "800" || ZenonVersion == "760")
        {
          File.Copy($@"\\atszg-fs02\prog_g$\zenon{ZenonVersion}\BIN\PC\PROGRAMFILES32\Release\AddInFramework\Mono.Cecil.dll", "Mono.Cecil.dll", true);
        }
        else
        {
          File.Copy($@"\\atszg-fs02\prog_g$\zenon{ZenonVersion}\BETA\BIN\PC\PROGRAMFILES32\Release\AddInFramework\Mono.Cecil.dll", "Mono.Cecil.dll", true);
        }
      }

      public static string AdapterAnalysisDllFilePath
      {
        get
        {
          if (ZenonVersion == "800" || ZenonVersion == "760")
          {
            File.Copy($@"\\atszg-fs02\prog_g$\zenon{ZenonVersion}\BIN\PC\PROGRAMFILES32\Release\AddInFramework\Scada.Internal.AddIn.Adapter.Analysis.dll", "Scada.Internal.AddIn.Adapter.Analysis2.dll", true);
            return "Scada.Internal.AddIn.Adapter.Analysis2.dll";
          }
          else
          {
            File.Copy($@"\\atszg-fs02\prog_g$\zenon{ZenonVersion}\BETA\BIN\PC\PROGRAMFILES32\Release\AddInFramework\Scada.Internal.AddIn.Adapter.Analysis.dll", "Scada.Internal.AddIn.Adapter.Analysis2.dll", true);
            return "Scada.Internal.AddIn.Adapter.Analysis2.dll";
          }
        }
      }
      public static string ContratcsDllFilePath
      {
        get
        {
          if (ZenonVersion == "800" || ZenonVersion == "760")
          {
            File.Copy($@"\\atszg-fs02\prog_g$\zenon{ZenonVersion}\BIN\PC\PROGRAMFILES32\Release\AddInFramework\Scada.AddIn.Contracts.dll", "Scada.AddIn.Contracts.dll", true);
            return "Scada.AddIn.Contracts.dll";
          }
          else
          {
            File.Copy($@"\\atszg-fs02\prog_g$\zenon{ZenonVersion}\BETA\BIN\PC\PROGRAMFILES32\Release\AddInFramework\Scada.AddIn.Contracts.dll", "Scada.AddIn.Contracts.dll", true);
            return "Scada.AddIn.Contracts.dll";
          }
        }
      }
      public static string InteropDllFilePath
      {
        get
        {
          if (ZenonVersion == "800" || ZenonVersion == "760")
          {
            File.Copy($@"\\atszg-fs02\prog_g$\zenon{ZenonVersion}\BIN\PC\PROGRAMFILES32\Release\AddInFramework\Interop.zenOn.signed.dll", "Interop.zenOn.signed.dll", true);
            return "Interop.zenOn.signed.dll";
          }
          else
          {
            File.Copy($@"\\atszg-fs02\prog_g$\zenon{ZenonVersion}\BETA\BIN\PC\PROGRAMFILES32\Release\AddInFramework\Interop.zenOn.signed.dll", "Interop.zenOn.signed.dll", true);
            return "Interop.zenOn.signed.dll";
          }
        }
      }

      // Translation XML is named diffrentyl across diffrent Versions, add more cases with next Version and check for translation xml file name
      public static string TranslationXmlPath
      {
        get
        {
          if (ZenonVersion == "760")
          {
            File.Copy($@"\\atszg-fs02\prog_g$\zenon{ZenonVersion}\BIN\PC\PROGRAMFILES32\Release\AddInFramework\translation.xml", "translation.xml", true);
            return "translation.xml";
          }
          else if (ZenonVersion == "800")
          {
            File.Copy($@"\\atszg-fs02\prog_g$\zenon{ZenonVersion}\BIN\PC\PROGRAMFILES32\Release\AddInFramework\translation2.xml", "translation2.xml", true);
            return "translation2.xml";
          }
          else
          {
            File.Copy($@"\\atszg-fs02\prog_g$\zenon{ZenonVersion}\BETA\BIN\PC\PROGRAMFILES32\Release\AddInFramework\translation2.xml", "translation2.xml", true);
            return "translation2.xml";
          }
        }
      }
      public static string SandcastleStyleFile { get; set; } = @"C:\Program Files (x86)\EWSoftware\Sandcastle Help File Builder\PresentationStyles\VS2013\styles\branding.css";
    }
    public static class AdapterAnalysis
    {
      public static string ScadaInterfaceAnalyzer = "Scada.Internal.AddIn.Adapter.Analysis.ScadaInterfaceAnalyzer";
      public static string Analyze = "Analyze";
      public static string Namespaces = "Namespaces";
    }
    public static string GetConceptualTopicsFolder(HelpType helptype)
    {
      string sandcastlebasepath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Sandcastle\";
      if (!Directory.Exists(sandcastlebasepath)) Directory.CreateDirectory(sandcastlebasepath);

      string scandversion = $@"{sandcastlebasepath}{ZenonVersion}\";
      if (!Directory.Exists(scandversion)) Directory.CreateDirectory(scandversion);

      if (helptype == HelpType.Addin)
      {
        string scvandtypeplusdate = $@"{scandversion}\addin_{DateTime.Today.ToString("yyMMdd")}";
        if (!Directory.Exists(scvandtypeplusdate))
          Directory.CreateDirectory(scvandtypeplusdate);

        string conceptualTopicsFodler = $@"{scvandtypeplusdate}\ConceptualContent\";
        if (!Directory.Exists(conceptualTopicsFodler))
          Directory.CreateDirectory(conceptualTopicsFodler);

        return conceptualTopicsFodler;
      }
      else
      {
        string scvandtypeplusdate = $@"{scandversion}\com_{DateTime.Today.ToString("yyMMdd")}";
        if (!Directory.Exists(scvandtypeplusdate))
          Directory.CreateDirectory(scvandtypeplusdate);

        string conceptualTopicsFodler = $@"{scvandtypeplusdate}\ConceptualContent\";
        if (!Directory.Exists(conceptualTopicsFodler))
          Directory.CreateDirectory(conceptualTopicsFodler);

        return conceptualTopicsFodler;
      }
    }
  }
}
