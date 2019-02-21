using zenonApi.Core;

namespace zenonApi.Logic
{
  public class LogicVariable : zenonSerializable<LogicVariable, LogicVariableGroup, LogicProject>
  {
    #region zenonSerializable Implementation

    protected override string NodeName => "var";
    public override LogicVariableGroup Parent { get; protected set; }
    public override LogicProject Root { get; protected set; }

    #endregion  

    /// <summary>
    /// Symbol of the variable.
    /// This attribute is mandatory.
    /// </summary>
    [zenonSerializableAttribute("name")]
    public string Name { get; set; }

    /// <summary>
    /// Name of the data type of the variable.
    /// This attribute is mandatory.
    /// </summary>
    [zenonSerializableAttribute("type")]
    public string Type { get; set; }

    /// <summary>
    /// Maximum length if the data type is STRING.
    /// This attribute is mandatory for STRING variables, and should not appear
    /// for other data types.
    /// </summary>
    [zenonSerializableAttribute("len")]
    public string MaxStringLength { get; set; } = null;

    /// <summary>
    /// Dimension(s) if the variable is an array.
    /// There are at most 3 dimensions, seperated by commas.
    /// This attribute is optional.
    /// </summary>
    [zenonSerializableAttribute("dim", Converter = typeof(CoordinateConverter))]
    public (int X, int Y, int Z)? ArrayDimension { get; set; } = null;

    /// <summary>
    /// Attributes of the variable, seperated by comas.
    /// This attribute is optional.
    /// </summary>
    [zenonSerializableAttribute("attr")]
    public LogicVariableAttributes Attributes { get; set; } = null;

    /// <summary>
    /// Initial value of the variable.
    /// Must be a valid constant expression that fits the data type.
    /// This attribute is optional.
    /// </summary>
    //TODO: how should we convert this?!, i would suggest to let it as object/string
    [zenonSerializableAttribute("init")]
    public object InitialValue { get; set; } = null;
  }
}
