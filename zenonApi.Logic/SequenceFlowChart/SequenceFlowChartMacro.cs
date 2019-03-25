using zenonApi.Serialization;

namespace zenonApi.Logic.SequenceFlowChart
{
  /// <summary>
  /// Describes a macro-step symbol in a SFC chart.
  /// </summary>
  public class SequenceFlowChartMacro : zenonSerializable<SequenceFlowChartMacro>
  {
    public override string NodeName => "SFCmacro";

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
    /// The numerical reference of the macro beginning step (mandatory).
    /// </summary>
    [zenonSerializableAttribute("ref", AttributeOrder = 2)]
    public string NumericalReference { get; set; }

    /// <summary>
    /// The name of the macro beginning step (e.g. "GS1", mandatory).
    /// </summary>
    [zenonSerializableAttribute("name", AttributeOrder = 3, OmitIfNull = false)]
    public string Name { get; set; }
  }
}
