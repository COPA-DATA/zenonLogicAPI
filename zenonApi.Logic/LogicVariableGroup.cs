using System.Collections.Generic;
using zenonApi.Core;

namespace zenonApi.Logic
{
  public class LogicVariableGroup : zenonSerializable<LogicVariableGroup, ILogicVariableGroupContainer, LogicProject>
  {
    #region zenonSerializable Implementation

    protected override string NodeName => "vargroup";
    public override ILogicVariableGroupContainer Parent { get; protected set; }
    public override LogicProject Root { get; protected set; }

    #endregion  

    [zenonSerializableAttribute("name")]
    public string Name { get; set; }

    [zenonSerializableAttribute("kind")]
    public LogicVariableKind Kind { get; set; }

    [zenonSerializableNode("var")]
    public List<LogicVariable> Variables { get; set; } 
  }
}
