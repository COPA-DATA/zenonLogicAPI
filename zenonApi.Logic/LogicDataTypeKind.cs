using zenonApi.Serialization;

namespace zenonApi.Logic
{
  /// <summary>
  /// The kind of a <see cref="LogicDataType"/>.
  /// </summary>
  public enum LogicDataTypeKind
  {
    /// <summary>
    /// Use this value to indicate, that the kind of a <see cref="LogicDataType"/> is a
    /// basic data type.
    /// </summary>
    [zenonSerializableEnum("BASIC")]
    Basic,
    /// <summary>
    /// Use this value to indicate, that the kind of a <see cref="LogicDataType"/> is a
    /// standard function block.
    /// </summary>
    [zenonSerializableEnum("STDFB")]
    StandardFunctionBlock,
    /// <summary>
    /// Use this value to indicate, that the kind of a <see cref="LogicDataType"/> is a
    /// "C" function block.
    /// </summary>
    [zenonSerializableEnum("CFB")]
    CFunctionBlock,
    /// <summary>
    /// Use this value to indicate, that the kind of a <see cref="LogicDataType"/> is a
    /// user defined function block.
    /// </summary>
    [zenonSerializableEnum("UDFB")]
    UserDefinedFunctionBlock,
    /// <summary>
    /// Use this value to indicate, that the kind of a <see cref="LogicDataType"/> is a
    /// data structure.
    /// </summary>
    [zenonSerializableEnum("STRUCT")]
    DataStructure,
    /// <summary>
    /// Indicates that a used data type is a bitfield.
    /// </summary>
    [zenonSerializableEnum("BITFIELD")]
    BitField,
    /// <summary>
    /// Indicates that a used data type is an enumeration.
    /// </summary>
    [zenonSerializableEnum("ENUM")]
    Enumeration
  }
}
