using zenonApi.Serialization;

namespace zenonApi.Logic
{
  //TODO talk with StefanH about this class, the values are not clear for me
  public class LogicVariableInfo : zenonSerializable<LogicVariableInfo, LogicVariable, LogicProject>
  {
    protected override string NodeName => "varinfo";

    /// <summary>
    /// Type of information contained in the Data attribute.
    /// TODO add information here about possible values - the doku differs from the example file
    /// </summary>
    [zenonSerializableAttribute("type", AttributeOrder = 0)]
    public int Type { get; set; }

    /// <summary>
    /// Data in text format specified in the Type attribute.
    /// </summary>
    [zenonSerializableAttribute("data", AttributeOrder = 1)]
    public string Data { get; set; }
  }
}
