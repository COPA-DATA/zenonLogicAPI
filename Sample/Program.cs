using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using zenonApi.Logic;
using zenonApi.Serialization;

namespace Sample
{

  public abstract class TestBaseClass : zenonSerializable<TestBaseClass>
  {
    [zenonSerializableAttribute(nameof(SomeValue), OmitIfNull = false)]
    public string SomeValue { get; set; }
  }

  public class TestChildA : TestBaseClass
  {
    [zenonSerializableNode(nameof(SpecificPropertyA1))]
    public string SpecificPropertyA1 { get; set; }

    [zenonSerializableAttribute(nameof(SpecificPropertyA2))]
    public string SpecificPropertyA2 { get; set; }
  }

  public class TestChildB : TestBaseClass
  {
    public override string NodeName => "TestChildB";

    [zenonSerializableNode(nameof(SpecificPropertyB1))]
    public string SpecificPropertyB1 { get; set; }

    [zenonSerializableAttribute(nameof(SpecificPropertyB2))]
    public string SpecificPropertyB2 { get; set; }
  }

  public class Container : zenonSerializable<Container>
  {
    public override string NodeName { get => "Container"; }

    [zenonSerializableNode(nameof(Elements), typeof(Resolver), EncapsulateChildsIfList = false)]
    public List<TestBaseClass> Elements { get; set; } = new List<TestBaseClass>();

    [zenonSerializableNode("OtherName", EncapsulateChildsIfList = false)]
    public List<TestChildA> Problem1 { get; set; } = new List<TestChildA>();

    [zenonSerializableNode("AgainAnotherName", EncapsulateChildsIfList = false)]
    public List<TestChildA> Problem2 { get; set; } = new List<TestChildA>();

    [zenonSerializableNode(nameof(WithBaseClassA), EncapsulateChildsIfList = true)]
    public TestChildA WithBaseClassA { get; set; }
  }

  public class Resolver : IZenonSerializableResolver
  {
    public string GetNodeNameForSerialization(PropertyInfo targetProperty, Type targetType, object value, int index)
    {
      return targetType.Name + "TestAppended";
    }

    public Type GetTypeForDeserialization(string nodeName, int index)
    {
      var types = new[] { typeof(TestChildA), typeof(TestChildB) };
      return types.FirstOrDefault(x => nodeName.StartsWith(x.Name + "T"));
    }
  }

  public class Resolver2 : IZenonSerializableResolver
  {
    public string GetNodeNameForSerialization(PropertyInfo targetProperty, Type targetType, object value, int index)
    {
      return targetType.Name + "VerySpecific";
    }

    public Type GetTypeForDeserialization(string nodeName, int index)
    {
      if (nodeName.EndsWith("VerySpecific"))
      {
        return typeof(TestChildA);
      }

      var types = new[] { typeof(TestChildA), typeof(TestChildB) };
      return types.FirstOrDefault(x => nodeName.StartsWith(x.Name));
    }
  }

  class Program
  {
    private const string ZenonRuntimeComObjectName = "zenOn.ApplicationED";

    private static void Main(string[] args)
    {
      Container testContainer = new Container()
      {
        WithBaseClassA = new TestChildA() { SpecificPropertyA1 = "interesting", SpecificPropertyA2 = "lets see" },
        Elements = new List<TestBaseClass>()
        {
          new TestChildA() { SpecificPropertyA1 = "s1", SpecificPropertyA2 = "s2", SomeValue = "Child A" },
          new TestChildB() { SpecificPropertyB1 = "b1", SpecificPropertyB2 = "b2", SomeValue = "Child B" }
        },
        Problem1 = new List<TestChildA>()
        {
          new TestChildA(){ SpecificPropertyA1 = "prob1", SpecificPropertyA2 = "prob2"},
          new TestChildA(){ SpecificPropertyA1 = "prob2", SpecificPropertyA2 = "prob4"}
        },
        Problem2 = new List<TestChildA>()
        {
          new TestChildA(){ SpecificPropertyA1 = "prob5", SpecificPropertyA2 = "prob6"},
          new TestChildA(){ SpecificPropertyA1 = "prob7", SpecificPropertyA2 = "prob8"}
        }
      };

      testContainer.ExportAsFile(@"C:\Users\Mathias.Lackner\Desktop\apiNew.xml");

      var result = Container.Import(XElement.Load(@"C:\Users\Mathias.Lackner\Desktop\apiNew.xml"));

      return;

      // NOTE: No error handling etc. is included here, this sample is just intended to give you a starting point on
      // handling zenon Logic projects via code,
      // IMPORTANT: To avoid side effects, you should make sure that the Logic workbench is not running,
      // which is not shown here.

      // This sample accesses zenon via COM, therefore we need to connect first.
      var zenonEditor = Marshal.GetActiveObject(ZenonRuntimeComObjectName) as zenOn.ApplicationED;
      if (zenonEditor == null)
      {
        Console.WriteLine("Cannot connect to an instance of zenon Editor.");
        return;
      }

      // The active zenon editor project is used for our sample.
      var zenonProject = zenonEditor.MyWorkspace.ActiveDocument;
      if (zenonProject == null)
      {
        Console.WriteLine("No active instance of a zenon Editor project can be received.");
        return;
      }

      // NOTE: For this example, you need a reference to zenon.Interop.dll in your .csproj.
      // We added it from version 7.60 to a binaries folder.
      //
      // You can use alternative approaches to the one which is shown here, i.e.:
      // - modifying the project as pure XML
      //   Required:      References to zenonApi.Core and zenonApi.Logic.
      //   Advantage:     No dependency on Windows or zenon.
      //   Disadvantage:  If you want to import the object model to Logic, you need to do this on your own.
      // - modifying the project via a COM reference to zenon, as done in this example
      //   Required:      Reference to zenon.Interop.dll, references to zenonApi.Core, zenonApi.Logic and zenonApi.Zenon
      //   Advantage:     Easy to import and modify Logic projects from within a zenon context.
      //   Disadvantage:  Can only be done with a running zenon Editor instance on Windows
      // - modifying the project not via the Add-In framework
      //   Required:      Reference to Scada.AddIn.Contracts, zenonApi.Core, zenonApi.Logic, zenonApi.Zenon
      //   Advantage:     Same as for COM
      //   Disadvantage:  Same as for COM
      // - Just create projects and do basic modifications without any of our APIs
      //   Required:      -
      //   Advantage:     No dependencies, except K5B.exe and/or K5Prp.dll
      //   Disadvantage:  Just XML, no typed object model, hard to modify/maintain, etc.
      // What you choose simply depends on what you are developing and using.
      // The easiest way might be to work with the provided APIs we use here for COM and Add-In.

      // If you use the Add-In Framework, use "new zenonApi.Zenon.Zenon(zenonProject)
      var wrapper = new zenonApi.Zenon.ZenonCom(zenonProject);

      // We want to modify or create a Logic project named "Sample":
      var logicProjects = wrapper.LogicProjects;
      var sampleProjectToBeEdited = logicProjects.FirstOrDefault(x => x.ProjectName == "Sample");
      if (sampleProjectToBeEdited == null)
      {
        // "Sample" does not exist, we need to create it with the following.
        // All changes you make in the API will only take affect, after you call ImportLogicProjectsIntoZenon
        // (see the end of this file)
        sampleProjectToBeEdited = new LogicProject("Sample");
        logicProjects.Add(sampleProjectToBeEdited);
      }

      // Access the logic options etc. via a object model. The following shows some examples.
      sampleProjectToBeEdited.Settings.TriggerTime.CycleTime = 10000;
      sampleProjectToBeEdited.Settings.CompilerSettings.CompilerOptions["warniserr"] = "OFF";

      // Get the first global variable group ("(GLOBAL)" and "(RETAIN)" always exist).
      var variableGroup = sampleProjectToBeEdited.GlobalVariables.VariableGroups.FirstOrDefault();
      // The appropriate group can also be accessed directly like this:
      variableGroup = sampleProjectToBeEdited.GlobalVariables[LogicVariableKind.Global];

      // Sample for creating some string variables in our "Sample" project:
      for (int i = 0; i < 10; i++)
      {
        var variableSample = new LogicVariable()
        {
          InitialValue = "5",
          MaxStringLength = "255",
          Type = "STRING",
          Name = "MyVariable" + i,
        };
        variableSample.VariableInfos.Add(new LogicVariableInfo()
        {
          // Set to be visible in zenon (SYB Flag)
          Data = "<syb>",
          Type = LogicVariableInformationTypeKind.Embed
        });

        variableSample.VariableInfos.Add(new LogicVariableInfo()
        {
          Data = "STRATON",
          Type = LogicVariableInformationTypeKind.Profile
        });

        variableGroup.Variables.Add(variableSample);
      }

      // Get the first folder of your application tree in Logic and rename it
      LogicFolder folder = sampleProjectToBeEdited.ApplicationTree.Folders.FirstOrDefault();
      if (folder == null)
      {
        // Does not exist, create it instead.
        folder = new LogicFolder("RenamedTestFolder");
        sampleProjectToBeEdited.ApplicationTree.Folders.Add(folder);
      }

      // Renaming is possible.
      folder.Name = "RenamedTestFolder";

      // Same for the first program:
      LogicProgram program = folder.Programs.FirstOrDefault();
      if (program == null)
      {
        program = new LogicProgram("RenamedTestProgram");
        folder.Programs.Add(program);
      }
      program.Name = "RenamedMyProgram";
      // Modify the source code of a program:
      program.SourceCode += "\n// Some Comment";

      // Navigate back to the application tree when only having the program (useful when working with multiple logic
      // projects at once):
      var folderAgain = program.Parent;

      // Change the cycle timing and other project settings
      sampleProjectToBeEdited.Settings.TriggerTime.CycleTime = 12345;
      sampleProjectToBeEdited.Settings.CompilerSettings.CompilerOptions["warniserr"] = "OFF";

      // Modify variables (if it exists)
      var variable = program.VariableGroups.FirstOrDefault()?.Variables.FirstOrDefault();
      if (variable != null)
      {
        variable.Name = "RenamedVariable";
        variable.Attributes.In = true;
        variable.Attributes.Out = true;
        variable.VariableInfos.Add(new LogicVariableInfo() { Type = LogicVariableInformationTypeKind.Embed, Data = "<syb>" });
      }

      // Remove a folder if it exists
      sampleProjectToBeEdited.ApplicationTree.Folders.FirstOrDefault(x => x.Name == "Signals")?.Remove();

      // Sample for exporting a project object model to a file:
      string sampleFile = $@"C:\Users\{Environment.UserName}\Desktop\DemoProjectModified.xml";
      sampleProjectToBeEdited.ExportAsFile(sampleFile, Encoding.GetEncoding("iso-8859-1"));

      // Sample for reading a project object model from a file:
      var projectFromXmlFile = LogicProject.Import(XElement.Load(sampleFile));

      // Sample for convert the project object model to XElements and save it manually to the same file
      XElement modifiedProject = sampleProjectToBeEdited.ExportAsXElement();

      XDocument document = new XDocument
      {
        Declaration = new XDeclaration("1.0", "iso-8859-1", "yes")
      };
      document.Add(modifiedProject);
      using (XmlTextWriter writer = new XmlTextWriter($@"C:\Users\{Environment.UserName}\Desktop\DemoProjectModified.xml",
        Encoding.GetEncoding("iso-8859-1")))
      {
        writer.Indentation = 3;
        writer.Formatting = Formatting.Indented;
        document.Save(writer);
      }

      // Import and commit logic projects with the changes we made:
      wrapper.ImportLogicProjectsIntoZenon();
      wrapper.Dispose();
    }
  }
}
