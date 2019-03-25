using zenonApi.Serialization;

namespace zenonApi.Logic.SequentialFunctionChart
{
  /// <summary>
  /// Describes an SFC transition.
  /// </summary>
  public class SequentialFunctionChartTransition : zenonSerializable<SequentialFunctionChartTransition>
  {
    public override string NodeName => "SFCtrans";

    /// <summary>
    /// X coordinate of the transition, in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("dx", AttributeOrder = 0)]
    public int X { get; set; }

    /// <summary>
    /// Y coordinate of the transition, in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("dy", AttributeOrder = 1)]
    public int Y { get; set; }

    /// <summary>
    /// The numerical reference of the transition (mandatory).
    /// </summary>
    [zenonSerializableAttribute("ref", AttributeOrder = 2)]
    public string NumericalReference { get; set; } // TODO: Is this an Integer? Also for others in the same namespace

    /// <summary>
    /// The name of the transition (e.g. "GT1", mandatory).
    /// </summary>
    [zenonSerializableAttribute("name", AttributeOrder = 3, OmitIfNull = false)]
    public string Name { get; set; }

    /// <summary>
    /// A list of following steps (mandatory).
    /// </summary>
    [zenonSerializableAttribute("next", AttributeOrder = 4)]
    public string Next { get; set; }
    // TODO: How does this value look like? Can we automate it? Also for others in the same namespace

    /// <summary>
    /// Contains the text attached to a transition.
    /// </summary>
    [zenonSerializableNode("SFCnote", NodeOrder = 0, OmitIfNull = false)]
    public string Note { get; set; }

    /// <summary>
    /// Describes the condition attached to a SFC transition.
    /// </summary>
    [zenonSerializableNode("SFCcondition", NodeOrder = 1)]
    public SequentialFunctionChartCondition Condition { get; protected set; } = new SequentialFunctionChartCondition();
  }
}
