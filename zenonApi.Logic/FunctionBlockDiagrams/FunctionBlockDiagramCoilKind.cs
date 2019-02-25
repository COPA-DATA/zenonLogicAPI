using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Represents a coil in a FBD diagram.
  /// </summary>
  public enum FunctionBlockDiagramCoilKind
  {
    /// <summary>
    /// A normal "direct" coil.
    /// </summary>
    [zenonSerializableEnum("D")]
    Direct,
    /// <summary>
    /// A negated coil.
    /// </summary>
    [zenonSerializableEnum("I")]
    Negated,
    /// <summary>
    /// A "reset" coil.
    /// </summary>
    [zenonSerializableEnum("R")]
    Reset,
    /// <summary>
    /// A "set" coil.
    /// </summary>
    [zenonSerializableEnum("S")]
    Set
  }
}
