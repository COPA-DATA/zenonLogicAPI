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
      var test = XDocument.Load(@"C:\Users\Mathias\Desktop\TemplateBbLogicProjectExport.xml");
      LogicProject proj = LogicProject.Import(test.Element("K5project"));
      LogicFolder folder = proj.ApplicationTree.Folders.First();
      LogicProgram prog = folder.Programs.FirstOrDefault();

      var tree = folder.Parent;

      XElement testresult = proj.Export();
    }
  }
}
