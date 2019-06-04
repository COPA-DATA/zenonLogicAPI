using System;
using System.Diagnostics;
using zenonApi.Logic.Resources;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  [DebuggerDisplay("{" + nameof(Name) + "}={" + nameof(Value) + "}")]
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
        throw new ArgumentException(string.Format(Strings.LogicOptionTupleConstructorArgumentException,
          nameof(LogicOptionTuple)));
      }

      Name = name; // TODO: Check for correct naming? (StringExtensions.IsValidZenonLogicName)
      Value = value;
    }

    [zenonSerializableAttribute("name", AttributeOrder = 0)]
    public string Name { get; protected set; }

    [zenonSerializableAttribute("value", AttributeOrder = 1)]
    public string Value { get; set; }
  }
}
