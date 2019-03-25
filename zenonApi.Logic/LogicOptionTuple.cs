using System.Diagnostics;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  [DebuggerDisplay("Option name = {Name}")]
  public class LogicOptionTuple : zenonSerializable<LogicOptionTuple>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "opt";
    #endregion

    internal LogicOptionTuple() { }
    
    public LogicOptionTuple(string name, string value)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        //throw new ArgumentException(string.Format(Strings.LogicOptionTupleConstrucArgExcp, nameof(LogicOptionTuple)));
      }

      Name = name; // TODO: Check for correct naming? (StringExtensions.IsValidZenonLogicName)
      Value = value;
    }

    [zenonSerializableAttribute("name", AttributeOrder = 0)]
    public string Name { get; protected set; }

    [zenonSerializableAttribute("value", AttributeOrder = 1)]
    public string Value { get; protected set; }
  }
}
