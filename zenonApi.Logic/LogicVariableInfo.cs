using System.Diagnostics;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  //TODO talk with StefanH about this class, the values are not clear for me
  [DebuggerDisplay("Type = {Type}")]
  public class LogicVariableInfo : zenonSerializable<LogicVariableInfo>
  {
    public override string NodeName => "varinfo";

    /// <summary>
    /// Type of information contained in the Data attribute.
    /// </summary>
    [zenonSerializableAttribute("type", AttributeOrder = 0)]
    public LogicVariableInformationTypeKind Type { get; set; }

    /// <summary>
    /// Data in text format specified in the Type attribute.
    /// </summary>
    [zenonSerializableAttribute("data", AttributeOrder = 1)]
    public string Data { get; set; }
  }
}
