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
    /// function block diagram (FBD).
    /// </summary>
    [zenonSerializableEnum("FBD")]
    FunctionBlockDiagram,

    /// <summary>
    /// Use this value to indicate, that the language of the <see cref="_Pou"/> is
    /// structured text (ST).
    /// </summary>
    [zenonSerializableEnum("ST")]
    StructuredText,

    /// <summary>
    /// Use this value to indicate, that the language of the <see cref="_Pou"/> is
    /// ladder diagram (LD).
    /// </summary>
    [zenonSerializableEnum("LD")]
    LadderDiagram,

    /// <summary>
    /// Use this value to indicate, that the language of the <see cref="_Pou"/> is
    /// an instruction list (IL).
    /// Note that IL is deprecated since EN 61131 2014-06.
    /// </summary>
    [zenonSerializableEnum("IL")]
    InstructionList,

    /// <summary>
    /// Use this value to indicate, that the language of the <see cref="_Pou"/> is a
    /// sequential function chart (SFC).
    /// </summary>
    [zenonSerializableEnum("SFC")]
    SequentialFunctionChart
  }
}
