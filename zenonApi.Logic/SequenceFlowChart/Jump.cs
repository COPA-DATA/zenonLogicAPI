using zenonApi.Serialization;

namespace zenonApi.Logic.SequenceFlowChart
{
  /// <summary>
  /// Describes a jump symbol to a SFC <see cref="Step"/>.
  /// </summary>
  public class Jump : zenonSerializable<Jump>
  {
    public override string NodeName => "SFCjump";

    /// <summary>
    /// X coordinate in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("dx", AttributeOrder = 0)]
    public int X { get; set; }

    /// <summary>
    /// Y coordinate in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("dy", AttributeOrder = 1)]
    public int Y { get; set; }

    /// <summary>
    /// The numerical reference of the destination step (mandatory).
    /// </summary>
    [zenonSerializableAttribute("ref", AttributeOrder = 2)]
    public string NumericalReference { get; set; }

    /// <summary>
    /// The name of the destination step (e.g. "GS1", mandatory).
    /// </summary>
    [zenonSerializableAttribute("name", AttributeOrder = 3, OmitIfNull = false)]
    public string Name { get; set; }
  }
}
