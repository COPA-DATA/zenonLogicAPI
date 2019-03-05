using System.ComponentModel;
using System.Xml.Linq;
using zenonApi.Logic.Internal;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  /// <summary>
  /// The root of a K5 project structure.
  /// </summary>
  public class LogicProject : zenonSerializable<LogicProject, IZenonSerializable, LogicProject>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "K5project";
    #endregion

    #region Specific properties

    /// <summary>
    /// The mandatory version of the K5 project.
    /// </summary>
    [zenonSerializableAttribute("version", AttributeOrder = 0)]
    public string Version { get; protected set; }

    /// <summary>
    /// The original pathname of the K5 project's folder.
    /// </summary>
    [zenonSerializableAttribute("path", AttributeOrder = 1)]
    public string Path { get; protected set; }

    /// <summary>
    /// This tag groups all the settings of the project.
    /// </summary>
    [zenonSerializableNode("settings", NodeOrder = 2)]
    public LogicProjectSettings Settings { get; protected set; }

    ///// <summary>
    ///// The tag groups all the defined data types.
    ///// </summary>
    [zenonSerializableNode("types", NodeOrder = 4)]
    public LogicDataTypesCollection DataTypes { get; protected set; }

    /// <summary>
    /// This tag groups all the global scope variable groups of the project.
    /// </summary>
    [zenonSerializableNode("variables", NodeOrder = 7)]
    public LogicGlobalVariables GlobalVariables { get; protected set; }

    /// <summary>
    /// Lists all programs, sub-programs and UDFBs of the project.
    /// It is not intended to be manipulated by users of this API directly.
    /// </summary>
    [Browsable(false)]
    [zenonSerializableNode("programs", NodeOrder = 10)]
    internal _LogicProgramsCollection Programs { get; set; }

    /// <summary>
    /// Contains the logical folder structure of the programs and UDFBs.
    /// </summary>
    [zenonSerializableNode("Appli", NodeOrder = 17)]
    public ApplicationTree ApplicationTree { get; protected set; }
    #endregion
  }
}
