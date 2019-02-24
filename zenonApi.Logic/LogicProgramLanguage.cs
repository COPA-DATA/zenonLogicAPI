using zenonApi.Logic.Internal;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  /// <summary>
  /// The kind of a <see cref="_Pou"/>.
  /// </summary>
  public enum LogicProgramLanguage
  {
    /// <summary>
    /// Use this value to indicate, that the language of the <see cref="_Pou"/> is
    /// Function Block Diagram.
    /// </summary>
    [zenonSerializableEnum("FBD")]
    FunctionBlockDiagram,
    /// <summary>
    /// Use this value to indicate, that the language of the <see cref="_Pou"/> is
    /// Structured Text.
    /// </summary>
    [zenonSerializableEnum("ST")]
    StructuredText,
    /// <summary>
    /// Use this value to indicate, that the language of the <see cref="_Pou"/> is a
    /// Sequence Flow Chart.
    /// </summary>
    [zenonSerializableEnum("SFC")]
    SequenceFlowChart //TODO; is this the correct name?
  }
}
