using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic.SequentialFunctionChart
{
  /// <summary>
  /// Describes a SFC program.
  /// </summary>
  public class SequentialFunctionChartDefinition : zenonSerializable<SequentialFunctionChartDefinition>
  {
    public override string NodeName => "sourceSFC";

    /// <summary>
    /// The width of the whole chart in grid units.
    /// This property is mandatory.
    /// </summary>
    [zenonSerializableAttribute("sx", AttributeOrder = 0)]
    public uint ChartWidth { get; set; } = 100; // TODO; Find a good default value

    /// <summary>
    /// The height of the whole chart in grid units.
    /// This property is mandatory.
    /// </summary>
    [zenonSerializableAttribute("sy", AttributeOrder = 1)]
    public uint ChartHeight { get; set; } = 100; // TODO: Good default value?

    /// <summary>
    /// Describes a SFC transition.
    /// </summary>
    [zenonSerializableNode("SFCtrans", NodeOrder = 0)]
    public ExtendedObservableCollection<SequentialFunctionChartTransition> Transitions { get; protected set; }
      = new ExtendedObservableCollection<SequentialFunctionChartTransition>();

    /// <summary>
    /// Describes SFC steps.
    /// </summary>
    [zenonSerializableNode("SFCstep", NodeOrder = 1)]
    public ExtendedObservableCollection<SequentialFunctionChartStep> Steps { get; protected set; }
      = new ExtendedObservableCollection<SequentialFunctionChartStep>();

    /// <summary>
    /// Describes the jumps to SFC steps.
    /// </summary>
    [zenonSerializableNode("SFCjump", NodeOrder = 2)]
    public ExtendedObservableCollection<SequentialFunctionChartJump> Jumps { get; protected set; }
      = new ExtendedObservableCollection<SequentialFunctionChartJump>();

    /// <summary>
    /// Describes the macro-step symbols in a SFC chart.
    /// </summary>
    [zenonSerializableNode("SFCmacro", NodeOrder = 3)]
    public ExtendedObservableCollection<SequentialFunctionChartMacro> Macros { get; protected set; }
      = new ExtendedObservableCollection<SequentialFunctionChartMacro>();

    /// <summary>
    /// Describes line segments drawn in the SFC chart area.
    /// </summary>
    [zenonSerializableNode("SFClines", NodeOrder = 4)]
    public ExtendedObservableCollection<SequentialFunctionChartLine> Lines { get; protected set; }
      = new ExtendedObservableCollection<SequentialFunctionChartLine>();
  }
}
