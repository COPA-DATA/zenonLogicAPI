using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;
using zenonApi.Collections;
using zenonApi.Logic.Internal;
using zenonApi.Logic.Network;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  /// <summary>
  /// The root of a K5 project structure.
  /// </summary>
  [DebuggerDisplay("zenon Logic project name: {" + nameof(ProjectName) + "}")]
  public class LogicProject : zenonSerializable<LogicProject, IZenonSerializable, LogicProject>
  {
    private LogicProject()
    {
      // Initialize members which require the current object in their ctor-parameters:
      this.Settings = new LogicProjectSettings(this);
      this.DataTypes = new ExtendedObservableCollection<LogicDataType>();
      this.LogicDefinitions = new LogicDefinitions(this);
      this.GlobalVariables = new LogicGlobalVariables(this);
      this.Networks = new LogicNetwork(this);
      this.Programs = new _LogicProgramsCollection(this);
      this.ApplicationTree = new ApplicationTree(this);
    }

    public LogicProject(string projectName) : this()
    {
      // TODO: Set path with project name, also check for valid name
      this.Path = System.IO.Path.Combine("TODO_InsertRealPath", projectName); // TODO
    }

    #region zenonSerializable Implementation
    public override string NodeName => "K5project";
    #endregion

    #region Specific properties
    public string ProjectName => string.IsNullOrEmpty(this.Path) ? "<Unknown>" : 
      System.IO.Path.GetFileName(this.Path.TrimEnd(System.IO.Path.DirectorySeparatorChar));
    
    /// <summary>
    /// The mandatory version of the K5 project.
    /// </summary>
    [zenonSerializableAttribute("version", AttributeOrder = 0)]
    public string Version { get; protected set; } = "1.1";

    /// <summary>
    /// The original pathname of the K5 project's folder.
    /// </summary>
    [zenonSerializableAttribute("path", AttributeOrder = 1, OmitIfNull = false)]
    public string Path { get; protected set; }

    //TODO: discuss about default constructor calls/init for this property and subproperties
    /// <summary>
    /// This tag groups all the settings of the project.
    /// </summary>
    [zenonSerializableNode("settings", NodeOrder = 2)]
    public LogicProjectSettings Settings { get; protected set; }

    //TODO: Ask StefanH about this property (not in docu)
    [zenonSerializableRawFormat("libraries", NodeOrder = 3)]
    public XElement Libraries { get; set; }

    /// <summary>
    /// The tag groups all the defined data types.
    /// </summary>
    [zenonSerializableNode("types", NodeOrder = 4, EncapsulateChildsIfList = true)]
    public ExtendedObservableCollection<LogicDataType> DataTypes { get; protected set; }

    /// <summary>
    /// This tag groups the COMMON and GOLBAL definitions.
    /// </summary>
    [zenonSerializableNode("definitions", NodeOrder = 5)]
    public LogicDefinitions LogicDefinitions { get; protected set; }

    //TODO: Ask StefanH about this property (not in docu)
    [zenonSerializableRawFormat("IOs", NodeOrder = 6)]
    public XElement Ios { get; set; }

    /// <summary>
    /// This tag groups all the global scope variable groups of the project.
    /// </summary>
    [zenonSerializableNode("variables", NodeOrder = 7)]
    public LogicGlobalVariables GlobalVariables { get; protected set; }

    /// <summary>
    /// This tag describes all network configuration.
    /// </summary>
    [zenonSerializableNode("networks", NodeOrder = 8)]
    public LogicNetwork Networks { get; protected set; }

    [zenonSerializableRawFormat("fieldbus", NodeOrder = 9)]
    public XElement FieldBus { get; set; }

    /// <summary>
    /// Lists all programs, sub-programs and UDFBs of the project.
    /// It is not intended to be manipulated by users of this API directly.
    /// </summary>
    [Browsable(false)]
    [zenonSerializableNode("programs", NodeOrder = 10)]
    internal _LogicProgramsCollection Programs { get; private set; }

    [zenonSerializableRawFormat("spylists", NodeOrder = 11)]
    public XElement SpyLists { get; set; }

    [zenonSerializableRawFormat("recipes", NodeOrder = 12)]
    public XElement Recipes { get; set; }

    [zenonSerializableRawFormat("graphics", NodeOrder = 13)]
    public XElement Graphics { get; set; }

    [zenonSerializableRawFormat("gridresources", NodeOrder = 14)]
    public XElement GridResources { get; set; }

    [zenonSerializableRawFormat("K5HMI", NodeOrder = 15)]
    public XElement HumanMachineInterface { get; set; }

    [zenonSerializableRawFormat("files", NodeOrder = 16)]
    public XElement Files { get; set; }
    
    /// <summary>
    /// Contains the logical folder structure of the programs and UDFBs.
    /// </summary>
    [zenonSerializableNode("Appli", NodeOrder = 17)]
    public ApplicationTree ApplicationTree { get; protected set; }
    #endregion
  }
}
