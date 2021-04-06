using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Represents a vertical bar (rail) in a FBD diagram.
  /// </summary>
  public class FunctionBlockDiagramVerticalRail : zenonSerializable<FunctionBlockDiagramVerticalRail>
  {
    public override string NodeName => "FBDvrail";

    /// <summary>
    /// The kind of the rail (mandatory).
    /// </summary>
    [zenonSerializableAttribute("kind", AttributeOrder = 0)]
    public FunctionBlockDiagramVerticalRailKind Kind { get; set; }

    /// <summary>
    /// The ID of the FBD object, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("id", AttributeOrder = 10)]
    // ReSharper disable once InconsistentNaming : Using .NET Framework Design Guidelines for the naming here.
    public string ID { get; set; } // TODO: Can this be an int? / uint?

    /// <summary>
    /// X coordinate of the box, in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("dx", AttributeOrder = 11)]
    public int X { get; set; }

    /// <summary>
    /// Y coordinate of the box, in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("dy", AttributeOrder = 12)]
    public int Y { get; set; }

    /// <summary>
    /// Height of the box, in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("sy", AttributeOrder = 14)]
    public int Height { get; set; }
  }
}