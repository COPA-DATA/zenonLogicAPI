using System.Diagnostics;
using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  [DebuggerDisplay("Name = {Name}")]
  public class LogicDataType : zenonSerializable<LogicDataType>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "type";
    #endregion

    #region Specific properties

    /// <summary>
    /// Name of the data type.
    /// </summary>
    [zenonSerializableAttribute("name", AttributeOrder = 0)]
    public string Name { get; set; } // TODO: Check for correct naming?

    /// <summary>
    /// Indicates the kind of data type.
    /// </summary>
    [zenonSerializableAttribute("kind", AttributeOrder = 1)]
    public LogicDataTypeKind Kind { get; set; }

    [zenonSerializableAttribute("lge", AttributeOrder = 2)] // TODO: Should only appear for Kind = UDFBs
    public LogicProgramLanguage Type { get; set; }

    /// <summary>
    /// Each of the data type variables in this list describes a parameter of a function block
    /// or a member of a structure. Basic data types do not have any data type variables.
    /// </summary>
    [zenonSerializableNode("var", NodeOrder = 3)]
    public ExtendedObservableCollection<LogicVariable> DataTypeVariables { get; protected set; }
      = new ExtendedObservableCollection<LogicVariable>();
    #endregion  

  }
}
