using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic.SequenceFlowChart
{
  /// <summary>
  /// Describes a SFC program.
  /// </summary>
  public class SequenceFlowChartDefinition : zenonSerializable<SequenceFlowChartDefinition>
  {
    public override string NodeName => "sourceSFC";

    /// <summary>
    /// The width of the whole chart in grid units.
    /// This property is mandatory.
    /// </summary>
    [zenonSerializableAttribute("sx", AttributeOrder = 0)]
    public uint ChartWidth { get; set; }

    /// <summary>
    /// The height of the whole chart in grid units.
    /// This property is mandatory.
    /// </summary>
    [zenonSerializableAttribute("sy", AttributeOrder = 1)]
    public uint ChartHeight { get; set; }

    /// <summary>
    /// Describes a SFC transition.
    /// </summary>
    [zenonSerializableNode("SFCtrans", NodeOrder = 0)]
    public ExtendedObservableCollection<Transition> Transitions { get; set; }

    /// <summary>
    /// Describes SFC steps.
    /// </summary>
    [zenonSerializableNode("SFCstep", NodeOrder = 1)]
    public ExtendedObservableCollection<Step> Steps { get; set; }

    /// <summary>
    /// Describes the jumps to SFC steps.
    /// </summary>
    [zenonSerializableNode("SFCjump", NodeOrder = 2)]
    public ExtendedObservableCollection<Jump> Jumps { get; set; }

    /// <summary>
    /// Describes the macro-step symbols in a SFC chart.
    /// </summary>
    [zenonSerializableNode("SFCmacro", NodeOrder = 3)]
    public ExtendedObservableCollection<Macro> Macros { get; set; }

    /// <summary>
    /// Describes line segments drawn in the SFC chart area.
    /// </summary>
    [zenonSerializableNode("SFClines", NodeOrder = 4)]
    public ExtendedObservableCollection<Line> Lines { get; set; }
  }
}
