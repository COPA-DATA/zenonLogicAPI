using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Abstract class, which is the base for all function block diagram objects.
  /// </summary>
  /// <typeparam name="TSelf"></typeparam>
  public abstract class FunctionBlockDiagramObject<TSelf> : zenonSerializable<TSelf>
    where TSelf : zenonSerializable<TSelf>
  {
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

    /// <summary>
    /// Width of the box, in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("sx", AttributeOrder = 13)]
    public int Width { get; set; }

    /// <summary>
    /// Height of the box, in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("sy", AttributeOrder = 14)]
    public int Height { get; set; }
  }
}
