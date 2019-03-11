using zenonApi.Serialization;

namespace zenonApi.Logic.Network
{
  /// <summary>
  /// This class fully describes a public or extern variable for binding configuration.
  /// </summary>
  public class LogicBindingItem : zenonSerializable<LogicBindingItem>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "binditem";
    #endregion

    /// <summary>
    /// Numerical ID for the bound item.
    /// This attribute is mandatory.
    /// </summary>
    [zenonSerializableAttribute("id", AttributeOrder = 0)]
    public string Id { get; set; }

    /// <summary>
    /// Kind of exchange related to this item.
    /// </summary>
    [zenonSerializableAttribute("ope", AttributeOrder = 1)]
    public BindingItemExchangeTypes Ope { get; set; }

    /// <summary>
    /// Symbol of the bound variable in the project.
    /// </summary>
    [zenonSerializableAttribute("symbol", AttributeOrder = 2)]
    public string Symbol { get; set; }

    /// <summary>
    /// Positive hysteresis
    /// This attribute is optionnal and appears only for public variables.
    /// </summary>
    [zenonSerializableAttribute("hystp", AttributeOrder = 3)]
    public string PosHysteresis { get; set; }

    /// <summary>
    /// Negative hysteresis
    /// This attribute is optionnal and appears only for public variables.
    /// </summary>
    [zenonSerializableAttribute("", AttributeOrder = 4)]
    public string NegHysteresis { get; set; }
  }
}
