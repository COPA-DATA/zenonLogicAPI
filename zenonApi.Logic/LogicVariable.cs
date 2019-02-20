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

    [zenonSerializableAttribute("name")]
    public string Name { get; set; }

    [zenonSerializableAttribute("type")]
    public string Type { get; set; }

    [zenonSerializableAttribute("len")]
    public string MaxStringLength { get; set; }

    [zenonSerializableAttribute("dim")]
    public string ArrayDimension { get; set; }

    [zenonSerializableAttribute("attr")]
    public object Attributes { get; set; }



  }
}
