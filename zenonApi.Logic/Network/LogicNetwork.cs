using zenonApi.Serialization;

namespace zenonApi.Logic.Network
{
  public class LogicNetwork : zenonSerializable<LogicNetwork, LogicProject, LogicProject>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "networks";
    #endregion

    //TODO: check definition string from docu - was already used once for definitions tag of proejct
    /// <summary>
    /// This property groups the COMMON and GOLBAL definitions.
    /// </summary>
    [zenonSerializableNode("binding", NodeOrder = 0)]
    public LogicNetworkBinding Binding { get; set; }

    /// <summary>
    /// This property groups the whole MODBUS slave or master configuration.
    /// </summary>
    [zenonSerializableNode("modbus", NodeOrder = 1, OmitIfNull = true)]
    public object Modbus { get; set; }

    /// <summary>
    /// This property describes the whole AS-i configuration.
    /// </summary>
    [zenonSerializableNode("asi", NodeOrder = 2, OmitIfNull = true)]
    public object Asi { get; set; }
  }
}