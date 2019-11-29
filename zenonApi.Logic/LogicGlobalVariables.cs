using System.Linq;
using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public sealed class LogicGlobalVariables : zenonSerializable<LogicGlobalVariables, LogicProject, LogicProject>
  {
    // ReSharper disable once UnusedMember.Local : Required default constructor for serialization.
    private LogicGlobalVariables() { }

    public LogicGlobalVariables(LogicProject parent) => this.Parent = this.Root = parent;

    #region zenonSerializable Implementation
    public override string NodeName => "variables";
    #endregion  

    /// <summary>
    /// This tag groups variables of a same variable group.
    /// There is one variable group for each listed group.
    /// </summary>
    [zenonSerializableNode("vargroup", NodeOrder = 0)]
    public ExtendedObservableCollection<LogicVariableGroup> VariableGroups { get; set; }
      = new ExtendedObservableCollection<LogicVariableGroup>();

    public LogicVariableGroup this[LogicVariableKind kind] =>
      VariableGroups.FirstOrDefault(variableGroup => variableGroup.Kind == kind);
  }
}