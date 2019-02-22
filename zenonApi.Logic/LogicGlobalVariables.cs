using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class LogicGlobalVariables : zenonSerializable<LogicGlobalVariables, LogicProject, LogicProject>, ILogicVariableGroupContainer
  {
    #region zenonSerializable Implementation

    IZenonSerializable IZenonSerializable<IZenonSerializable, LogicProject>.Parent
    {
      get => this.Parent;
    }

    protected override string NodeName => "variables";
    #endregion  

    /// <summary>
    /// This tag groups variables of a same variable group.
    /// There is one variable group for each listed group.
    /// </summary>
    [zenonSerializableNode("vargroup", NodeOrder = 0)]
    public ContainerAwareObservableCollection<LogicVariableGroup> VariableGroups { get; protected set; }
  }
}
