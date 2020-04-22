using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using zenonApi.Logic;
using zenonApi.MetaDescription;
using zenonApi.Xml.Variable;

namespace Tester
{
  class Program
  {
    static void Main(string[] args)
    {
      XDocument demoProject = XDocument.Load($@"C:\Users\Lukas.Rieser\OneDrive - COPA-DATA\Documents\XML Api\Metafile\metadata_generated.xml");

      Definitions definitions = Definitions.Import(demoProject.Root);


      //  // TODO: Write a sample app, which demonstrates the usage of the API and remove this whole test project

      //  // Import the project from the XML
      LogicProject project = LogicProject.Import(demoProject.Element("K5project"));

      //  var varGrp = project.GlobalVariables.VariableGroups.FirstOrDefault();
      //  for (int i = 0; i < 50; i++)
      //  {
      //    var asdf = new LogicVariable()
      //    {
      //      InitialValue = "5",
      //      MaxStringLength = "255",
      //      Type = "STRING",
      //      Name = "MyVariable" + i,
      //    };
      //    asdf.VariableInfos.Add(new LogicVariableInfo()
      //    {
      //      Data = "<syb>",
      //      Type = LogicVariableInformationTypeKind.Embed
      //    });

      //    asdf.VariableInfos.Add(new LogicVariableInfo()
      //    {
      //      Data = "STRATON",
      //      Type = LogicVariableInformationTypeKind.Profile
      //    });

      //    varGrp.Variables.Add(asdf);
      //  }

      //  LogicFolder folder = project.ApplicationTree
      //    .Folders.FirstOrDefault();

      //  folder.Name = "RenamedTestFolder";

      //  LogicProgram program = folder.Programs.FirstOrDefault();
      //  program.Name = "RenamedMyProgram";
      //  program.SourceCode += "\n// Second Comment";

      //  // Navigate to the application tree
      //  var folderAgain = program.Parent;

      //  // Change the cycle timing
      //  project.Settings.TriggerTime.CycleTime = 12345;
      //  project.Settings.CompilerSettings.CompilerOptions["warniserr"] = "OFF";

      //  // Modify variables
      //  var variable = program.VariableGroups.FirstOrDefault()?.Variables.FirstOrDefault();
      //  variable.Name = "RenamedVariable";
      //  variable.Attributes.In = true;
      //  variable.Attributes.Out = true;
      //  variable.VariableInfos.Add(new LogicVariableInfo() { Type = LogicVariableInformationTypeKind.Embed, Data = "<syb>" });

      //  // Remove a folder
      //  project.ApplicationTree.Folders.Where(x => x.Name == "Signals").FirstOrDefault()?.Remove();

      //  // Export and save the project again
      //  XElement modifiedProject = project.ExportAsXElement();
      //  XDocument document = new XDocument
      //  {
      //    Declaration = new XDeclaration("1.0", "iso-8859-1", "yes")
      //  };

      //  document.Add(modifiedProject);
      //  using (XmlTextWriter writer = new XmlTextWriter($@"C:\Users\{Environment.UserName}\Desktop\DemoProjectModified.xml",
      //    Encoding.GetEncoding("iso-8859-1")))
      //  {
      //    writer.Indentation = 3;
      //    writer.Formatting = Formatting.Indented;
      //    document.Save(writer);
      //  }
    }
  }
}
