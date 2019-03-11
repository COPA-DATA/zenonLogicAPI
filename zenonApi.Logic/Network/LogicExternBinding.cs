using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic.Network
{
  public class LogicExternBinding : zenonSerializable<LogicExternBinding>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "bindextern";
    #endregion

    /// <summary>
    /// List of binding items, one item for each public variable.
    /// </summary>
    [zenonSerializableNode("binditem", NodeOrder = 0)]
    public ExtendedObservableCollection<LogicBindingItem> BindingItems { get; protected set; }
      = new ExtendedObservableCollection<LogicBindingItem>();
  }
}
