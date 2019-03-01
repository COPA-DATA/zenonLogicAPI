using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Represents a user defined corner in a FBD diagram.
  /// </summary>
  public class FunctionBlockDiagramCorner : zenonSerializable<FunctionBlockDiagramCorner, FunctionBlockDiagramDefinition, LogicProject>
  {
    public override string NodeName => "FBDcorner";

    /// <summary>
    /// The ID of the FBD object, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("id", AttributeOrder = 10)]
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
  }
}
