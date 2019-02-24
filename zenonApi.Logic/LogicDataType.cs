using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class LogicDataType : zenonSerializable<LogicDataType, LogicDataTypesCollection, LogicProject>
  {
    #region zenonSerializable Implementation
    protected override string NodeName => "type";
    #endregion

    #region Specific properties

    /// <summary>
    /// Name of the data type.
    /// </summary>
    [zenonSerializableAttribute("name", AttributeOrder = 0)]
    public string Name { get; set; }

    /// <summary>
    /// Indicates the kind of data type.
    /// </summary>
    [zenonSerializableAttribute("kind", AttributeOrder = 1)]
    public LogicDataTypeKind Kind { get; set; }

    /// <summary>
    /// Each of the data type variables in this list describes a parameter of a function block
    /// or a member of a structure. Basic data types do not have any data type variables.
    /// </summary>
    [zenonSerializableNode("var", NodeOrder = 3)]
    public List<LogicVariable> DataTypeVariables { get; set; }

    #endregion  

  }
}
