using zenonApi.Collections;
using zenonApi.Logic.SerializationConverters;
using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Represents a connection line in a FBD diagram.
  /// </summary>
  public class FunctionBlockDiagramLine : zenonSerializable<FunctionBlockDiagramLine>
  {
    public override string NodeName => "FBDline";

    /// <summary>
    /// If set to true, a "o" boolean negation mark is drawn in zenon Logic.
    /// </summary>
    [zenonSerializableAttribute("negate", AttributeOrder = 0, Converter = typeof(OptionalBooleanConverter))]
    public bool Negate { get; set; }

    /// <summary>
    /// The ID of the source FBD object (according to the FBD flow).
    /// </summary>
    [zenonSerializableAttribute("idfrom", AttributeOrder = 1)]
    public string FromID { get; set; } // TODO: int or uint possible here? Same for other ids in this class

    /// <summary>
    /// The ID of the source FBD object (according to the FBD flow).
    /// </summary>
    [zenonSerializableAttribute("idto", AttributeOrder = 2)]
    public string ToID { get; set; }

    /// <summary>
    /// The zero based index of the output pin in the source object (mandatory).
    /// </summary>
    [zenonSerializableAttribute("pinfrom", AttributeOrder = 3)]
    public uint FromPin { get; set; }

    /// <summary>
    /// The zero based index of the output pin in the source object (mandatory).
    /// </summary>
    [zenonSerializableAttribute("pinto", AttributeOrder = 4)]
    public uint ToPin { get; set; }

    /// <summary>
    /// Significant points for drawing connection lines in FBD diagrams.
    /// </summary>
    [zenonSerializableAttribute("FBDlinepoint", AttributeOrder = 5)]
    public ExtendedObservableCollection<FunctionBlockDiagramLinePoint> DrawingPoints { get; set; }
  }
}
