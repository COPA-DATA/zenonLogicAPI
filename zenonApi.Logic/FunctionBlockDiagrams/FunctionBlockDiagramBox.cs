using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Represents a function or function block in a FBD diagram.
  /// </summary>
  public class FunctionBlockDiagramBox : FunctionBlockDiagramObject<FunctionBlockDiagramBox>
  {
    public override string NodeName => "FBDbox";

    /// <summary>
    /// The number of input pins, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("nbi", AttributeOrder = 20)]
    public uint NumberOfInputPins { get; set; }

    /// <summary>
    /// The number of output pins, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("nbo", AttributeOrder = 21)]
    public uint NumberOfOutputPins { get; set; }

    /// <summary>
    /// The kind of the box.
    /// </summary>
    [zenonSerializableAttribute("kind", AttributeOrder = 22)]
    public FunctionBlockDiagramBoxKind Kind { get; set; }

    /// <summary>
    /// The type name of the function block or function/operator name (mandatory).
    /// </summary>
    [zenonSerializableAttribute("type", AttributeOrder = 23)]
    public string Type { get; set; }

    /// <summary>
    /// The name of the instance (only considered if the <see cref="Kind"/> is <see cref="FunctionBlockDiagramBoxKind.FunctionBlock"/>). // TODO: Only consider if the type is FunctionBlock
    /// </summary>
    [zenonSerializableAttribute("inst", AttributeOrder = 24)]
    public string InstanceName { get; set; }
  }
}
