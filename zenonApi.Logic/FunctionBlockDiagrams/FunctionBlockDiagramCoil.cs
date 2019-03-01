using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Represents a coil in a function block diagram.
  /// </summary>
  public class FunctionBlockDiagramCoil : FunctionBlockDiagramObject<FunctionBlockDiagramCoil>
  {
    public override string NodeName => "FBDcoil";

    /// <summary>
    /// The kind of a function block diagram coil (mandatory).
    /// </summary>
    [zenonSerializableAttribute("kind", AttributeOrder = 0)]
    public FunctionBlockDiagramCoilKind Kind { get; set; }

    /// <summary>
    /// The symbol of the attached variable (mandatory).
    /// </summary>
    [zenonSerializableAttribute("symbol", AttributeOrder = 20)]
    public string Symbol { get; set; }
  }
}
