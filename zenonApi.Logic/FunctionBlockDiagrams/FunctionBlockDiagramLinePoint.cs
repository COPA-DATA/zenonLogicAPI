using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Represents a significant point for drawing of a FBD connection line.
  /// </summary>
  public class FunctionBlockDiagramLinePoint : zenonSerializable<FunctionBlockDiagramLinePoint, FunctionBlockDiagramLine, LogicProject>
  {
    public override string NodeName => "FBDlinepoint";

    /// <summary>
    /// The X coordinate in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("xcell", AttributeOrder = 0)]
    public int X { get; set; }

    /// <summary>
    /// The Y coordinate in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("ycell", AttributeOrder = 1)]
    public int Y { get; set; }

    /// <summary>
    /// The X offset within the grid cell (mandatory).
    /// </summary>
    [zenonSerializableAttribute("xincell", AttributeOrder = 2)]
    public int XOffset { get; set; }

    /// <summary>
    /// The Y offset within the grid cell (mandatory).
    /// </summary>
    [zenonSerializableAttribute("yincell", AttributeOrder = 3)]
    public int YOffset { get; set; }
  }
}
