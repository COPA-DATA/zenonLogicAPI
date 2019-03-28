using System;
using System.Linq;
using System.Xml.Linq;
using zenonApi.Logic;

namespace Tester
{
  class Program
  {
    static void Main(string[] args)
    {
      XDocument demoProject = XDocument.Load($@"C:\Users\{Environment.UserName}\Desktop\DemoProject.xml");
    
      // Import the project from the XML
      LogicProject project = LogicProject.Import(demoProject.Element("K5project"));
      LogicFolder folder = project.ApplicationTree
        .Folders.FirstOrDefault(x => x.Name == "Programs")
        .Folders.FirstOrDefault(x => x.Name == "testFolder");

      folder.Name = "RenamedTestFolder";

      LogicProgram program = folder.Programs.FirstOrDefault();
      program.Name = "RenamedMyProgram";
      program.SourceCode += "\n// Second Comment";

      // Navigate to the application tree
      var applicationTree = program.Parent;

      // Remove the program from its current container
      program.Remove();

      // Insert the program to this new location
      applicationTree.Programs.Add(program);
      
      // Export and save the project again
      XElement modifiedProject = project.Export();
      modifiedProject.Save($@"C:\Users\{Environment.UserName}\Desktop\DemoProjectModified.xml");
    }
  }
}
