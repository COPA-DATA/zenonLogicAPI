using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// The kind of a function block box.
  /// </summary>
  public enum FunctionBlockDiagramBoxKind
  {
    /// <summary>
    /// A function block (includes an instance name on the top).
    /// </summary>
    [zenonSerializableEnum("FB")]
    FunctionBlock,
    /// <summary>
    /// An operator or function (no instance).
    /// </summary>
    [zenonSerializableEnum("OP")]
    OperatorOrFunction,
  }
}
