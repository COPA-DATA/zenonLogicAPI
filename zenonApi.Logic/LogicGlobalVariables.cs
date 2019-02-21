using System.Collections.Generic;
using zenonApi.Core;

namespace zenonApi.Logic
{
  public class LogicGlobalVariables : zenonSerializable<LogicGlobalVariables, LogicProject, LogicProject>, ILogicVariableGroupContainer
  {
    #region zenonSerializable Implementation

    IZenonSerializable IZenonSerializable<IZenonSerializable, LogicProject>.Parent
    {
      get => this.Parent;
    }
    public override LogicProject Parent { get; protected set; }
    public override LogicProject Root { get; protected set; }

    protected override string NodeName => "variables";

    #endregion  

    [zenonSerializableNode("vargroup")]
    public List<LogicVariableGroup> VariableGroups { get; protected set; }

  }
}
