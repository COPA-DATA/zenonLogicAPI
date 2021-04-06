using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// The kind of a contact in a FBD diagram.
  /// </summary>
  public enum FunctionBlockContactKind
  {
    /// <summary>
    /// A "direct" contact.
    /// </summary>
    [zenonSerializableEnum("D")]
    Direct,
    /// <summary>
    /// A negated contact.
    /// </summary>
    [zenonSerializableEnum("I")]
    Negated,
    /// <summary>
    /// A contact with rising edge detection.
    /// </summary>
    [zenonSerializableEnum("P")]
    RisingEdgeDetection,
    /// <summary>
    /// A contact with falling edge detection.
    /// </summary>
    [zenonSerializableEnum("N")]
    FallingEdgeDetection
  }
}
