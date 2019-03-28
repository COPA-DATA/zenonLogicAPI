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
      LogicProject project = new LogicProject("test");

      // Ugly, but better than excluding this from every commit or changing it everytime afterwards
      XDocument test = XDocument.Load($@"C:\Users\{Environment.UserName}\Desktop\TemplateBbLogicProjectExport.xml");
    
      // TODO: After importing, the original XDocument is changed, therefore we MUST copy the XElement first in our final "Import" method
      // The current method should be kept internal anyway
      LogicProject proj = LogicProject.Import(test.Element("K5project"));
      LogicFolder folder = proj.ApplicationTree.Folders.First();
      LogicProgram prog = folder.Programs.FirstOrDefault();

      var tree = folder.Parent;

      var pouTest = proj.ApplicationTree.Folders[2].Programs.First();
      pouTest.Remove();

      XElement testresult = proj.Export();
      testresult.Save($@"C:\Users\{Environment.UserName}\Desktop\TemplateBbLogicProjectExport2.xml");
    }
  }
}
