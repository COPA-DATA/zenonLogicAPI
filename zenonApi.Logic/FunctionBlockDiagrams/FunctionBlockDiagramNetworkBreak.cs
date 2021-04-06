using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Represents a network break separation line in a FBD diagram.
  /// </summary>
  public class FunctionBlockDiagramNetworkBreak : zenonSerializable<FunctionBlockDiagramNetworkBreak>
  {
    public override string NodeName => "FBDbreak";

    /// <summary>
    /// The ID of the FBD object, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("id", AttributeOrder = 0)]
    // ReSharper disable once InconsistentNaming : Using .NET Framework Design Guidelines for the naming here.
    public string ID { get; set; } // TODO: Can this be an int? / uint?

    /// <summary>
    /// Y coordinate of the box, in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("dy", AttributeOrder = 2)]
    public int Y { get; set; }

    /// <summary>
    /// The text attached to the network break, which is (mandatory).
    /// </summary>
    [zenonSerializableAttribute("text", AttributeOrder = 4)]
    public string Text { get; set; }
  }
}
