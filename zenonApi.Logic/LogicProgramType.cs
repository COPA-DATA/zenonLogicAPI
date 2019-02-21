using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Core;

namespace zenonApi.Logic
{
  /// <summary>
  /// The kind of a <see cref="LogicProgram"/>.
  /// </summary>
  public enum LogicProgramType
  {
    /// <summary>
    /// Use this value to indicate, that the <see cref="LogicProgram"/> is a
    /// main program.
    /// </summary>
    [zenonSerializableEnum("program")]
    Program,
    /// <summary>
    /// Use this value to inidcate, that the <see cref="LogicProgram"/> is a
    /// sub-program.
    /// </summary>
    [zenonSerializableEnum("subprogram")]
    SubProgram,
    /// <summary>
    /// Use this value to inidcate, that the <see cref="LogicProgram"/> is a
    /// child of a <see cref="LogicProgramLanguage.SequenceFlowChart"/> program.
    /// </summary>
    [zenonSerializableEnum("child")]
    Child,
    /// <summary>
    /// Use this value to indicate, that the <see cref="LogicProgram"/> is a
    /// user defined function block.
    /// </summary>
    [zenonSerializableEnum("UDFB")]
    UserDefinedFunctionBlock,
  }
}
