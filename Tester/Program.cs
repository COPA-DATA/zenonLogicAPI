using System;
using System.Linq;
using System.Xml.Linq;

using zenonApi.Logic;
using zenonApi.Extensions;

namespace Tester
{
  class Program
  {
    static void Main(string[] args)
    {
      // Ugly, but better than excluding this from every commit or changing it everytime afterwards
      XDocument test = XDocument.Load($@"C:\Users\{Environment.UserName}\Desktop\TemplateBbLogicProjectExport.xml");
    
      LogicProject proj = LogicProject.Import(test.Element("K5project"));
      LogicFolder folder = proj.ApplicationTree.Folders.First();
      LogicProgram prog = folder.Programs.FirstOrDefault();

      var tree = folder.Parent;

      var pouTest = proj.ApplicationTree.Folders[2].Programs.First();
      pouTest.Remove();

      XElement testresult = proj.Export();
    }
  }
}
