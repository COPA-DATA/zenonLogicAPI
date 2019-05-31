using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;
using zenonApi.Collections;
using zenonApi.Logic.Internal;
using zenonApi.Logic.Network;
using zenonApi.Logic.Resources;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  /// <summary>
  /// The root of a K5 project structure.
  /// </summary>
  [DebuggerDisplay("{" + nameof(ProjectName) + "}")]
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

    /// <summary>
    /// Returns instance of <see cref="LogicProject"/> with the loaded information of the k5XmlExport and the
    /// specified zenon Logic project name.
    /// </summary>
    /// <param name="k5XmlExport">K5 XML export of a zenon Logic project</param>
    /// <param name="zenonLogicProjectName">Name which should be set for the zenon Logic project</param>
    /// <param name="stratonDirectoryOfZenonProject">Directory path of the straton folder of a zenon project.
    /// Example path: "C:\ProgramData\COPA-DATA\SQL2012\"zenon project GUID"\FILES\straton"</param>
    /// <returns></returns>
    public static LogicProject Import(XDocument k5XmlExport, string stratonDirectoryOfZenonProject = null, string zenonLogicProjectName = null)
    {
      var zenonLogicProject = LogicProject.Import(k5XmlExport.Element(Strings.K5XmlExportRootNodeName));
      if (zenonLogicProjectName != null)
      {
        zenonLogicProject.ProjectName = zenonLogicProjectName;
      }

      if (stratonDirectoryOfZenonProject != null)
      {
        zenonLogicProject.ModifyStratonDirectoryPartOfPath(stratonDirectoryOfZenonProject);
      }

      return zenonLogicProject;
    }

    #region zenonSerializable Implementation
    public override string NodeName => "K5project";
    #endregion

    #region Specific properties
    /// <summary>
    /// zenon Logic project name
    /// Note that this property gets the value from the last part of the <see cref="Path"/> property.
    /// Changes made to this property will affect the according part of the <see cref="Path"/> property.
    /// </summary>
    public string ProjectName
    {
      // gets the name of the zenon Logic project from the last part of the k5Project path xml attribute
      get => string.IsNullOrEmpty(this.Path) ? "Unknown" 
        : System.IO.Path.GetFileName(this.Path.TrimEnd(System.IO.Path.DirectorySeparatorChar));
      // writes the new name on the according position of the k5project path xml attribute
      set
      {
        string currentProjectPath = this.Path.TrimEnd(System.IO.Path.DirectorySeparatorChar);
        string[] splitResult = currentProjectPath.Split(System.IO.Path.DirectorySeparatorChar);

        splitResult[splitResult.Length - 1] = value;
        this.Path = $"{string.Join(System.IO.Path.DirectorySeparatorChar.ToString(), splitResult)}";
      }
    }

    /// <summary>
    /// Sets the part of the directory stored in the <see cref="Path"/> property which belongs to the
    /// zenon project´s straton folder.
    /// </summary>
    /// <param name="stratonDirectoryOfZenonProject">Example value:
    /// "C:\ProgramData\COPA-DATA\SQL2012\"zenon project GUID"\FILES\straton"
    /// </param>
    public void ModifyStratonDirectoryPartOfPath(string stratonDirectoryOfZenonProject)
    {
      if (string.IsNullOrWhiteSpace(stratonDirectoryOfZenonProject))
      {
        throw new ArgumentNullException(String.Format(Strings.GeneralMethodArgumentNullException, 
          nameof(ModifyStratonDirectoryPartOfPath), nameof(stratonDirectoryOfZenonProject)));
      }

      this.Path = System.IO.Path.Combine(stratonDirectoryOfZenonProject, this.ProjectName);
    }

    /// <summary>
    /// The mandatory version of the K5 project.
    /// </summary>
    [zenonSerializableAttribute("version", AttributeOrder = 0)]
    public string Version { get; protected set; } = "1.1";

    /// <summary>
    /// The pathname of the K5 project's folder.
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
