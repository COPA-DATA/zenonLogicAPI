using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class LogicVariableGroup : zenonSerializable<LogicVariableGroup>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "vargroup";
    #endregion  

    /// <summary>
    /// Name of the group.
    /// This attribute is mendatory.
    /// </summary>
    [zenonSerializableAttribute("name", AttributeOrder = 0)]
    public string Name { get; set; }

    /// <summary>
    /// Kind of variable group.
    /// This attribute is mandatory.
    /// </summary>
    [zenonSerializableAttribute("kind", AttributeOrder = 1)]
    public LogicVariableKind Kind { get; set; }

    /// <summary>
    /// List of variables of the group.
    /// </summary>
    [zenonSerializableNode("var", NodeOrder = 2)]
    public ExtendedObservableCollection<LogicVariable> Variables { get; set; }
  }
}
