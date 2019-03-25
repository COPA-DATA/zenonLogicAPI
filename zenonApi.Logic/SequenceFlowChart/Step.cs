using System.Collections.Generic;
using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic.SequenceFlowChart
{
  /// <summary>
  /// Describes a SFC step.
  /// </summary>
  public class Step : zenonSerializable<Step>
  {
    public override string NodeName => "SFCstep";

    /// <summary>
    /// The kind of a <see cref="Step"/>.
    /// </summary>
    [zenonSerializableAttribute("kind", AttributeOrder = 0)]
    public StepKind Kind { get; set; }

    /// <summary>
    /// X coordinate in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("dx", AttributeOrder = 1)]
    public int X { get; set; }

    /// <summary>
    /// Y coordinate in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("dy", AttributeOrder = 2)]
    public int Y { get; set; }

    /// <summary>
    /// The numerical reference of the step (mandatory).
    /// </summary>
    [zenonSerializableAttribute("ref", AttributeOrder = 3)]
    public string NumericalReference { get; set; }

    /// <summary>
    /// The name of the step (e.g. "GS1", mandatory).
    /// </summary>
    [zenonSerializableAttribute("name", AttributeOrder = 4, OmitIfNull = false)]
    public string Name { get; set; }

    /// <summary>
    /// A list of following transitions (mandatory).
    /// </summary>
    [zenonSerializableAttribute("next", AttributeOrder = 5)]
    public string Next { get; set; }

    /// <summary>
    /// Contains attached text.
    /// </summary>
    [zenonSerializableNode("SFCnote", NodeOrder = 0, OmitIfNull = false)]
    public string Note { get; set; }

    /// <summary>
    /// Describes an action block within a SFC step.
    /// </summary>
    [zenonSerializableNode("SFCaction", NodeOrder = 1, OmitIfNull = false)]
    public ExtendedObservableCollection<Action> Actions { get; protected set; }
      = new ExtendedObservableCollection<Action>();
  }
}
