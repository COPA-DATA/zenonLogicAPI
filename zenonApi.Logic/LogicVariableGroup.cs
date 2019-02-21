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

    /// <summary>
    /// Name of the group.
    /// This attribute is mendatory.
    /// </summary>
    [zenonSerializableAttribute("name")]
    public string Name { get; set; }

    /// <summary>
    /// Kind of variable group.
    /// This attribute is mandatory.
    /// </summary>
    [zenonSerializableAttribute("kind")]
    public LogicVariableKind Kind { get; set; }

    /// <summary>
    /// List of variables of the group.
    /// </summary>
    [zenonSerializableNode("var")]
    public List<LogicVariable> Variables { get; set; } 
  }
}
