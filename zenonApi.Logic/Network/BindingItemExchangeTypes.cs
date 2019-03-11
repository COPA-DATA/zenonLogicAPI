using zenonApi.Serialization;

namespace zenonApi.Logic.Network
{
  /// <summary>
  /// Kind of exchange related to binding items.
  /// </summary>
  public enum BindingItemExchangeTypes
  {
    [zenonSerializableEnum("XV")]
    DataExchange,
    /// <summary>
    /// Connection status (for extern variables only)
    /// </summary>
    [zenonSerializableEnum("CS")]
    ConnectionStatus,
    /// <summary>
    /// Variable status (for extern variables only)
    /// </summary>
    [zenonSerializableEnum("VS")]
    VariableStatus,
    /// <summary>
    /// Variable change time stamp (for extern variables only)
    /// </summary>
    [zenonSerializableEnum("VD")]
    VariableChangeTimeStamp,
    /// <summary>
    /// Variable change date stamp (for extern variables only)
    /// </summary>
    [zenonSerializableEnum("VT")]
    VariableChangeDateStamp
  }
}
