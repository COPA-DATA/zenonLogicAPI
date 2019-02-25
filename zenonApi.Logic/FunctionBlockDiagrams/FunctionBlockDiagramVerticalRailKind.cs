using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// The kind of a function block diagram rail.
  /// </summary>
  public enum FunctionBlockDiagramVerticalRailKind
  {
    /// <summary>
    /// Power rail on the left.
    /// </summary>
    [zenonSerializableEnum("LEFT")]
    Left,
    /// <summary>
    /// Power rail on the right.
    /// </summary>
    [zenonSerializableEnum("RIGHT")]
    Right,
    /// <summary>
    /// Vertical rail in the diagram.
    /// </summary>
    [zenonSerializableEnum("OR")]
    VerticalRail,
    /// <summary>
    /// Represents a "set" coil.
    /// </summary>
    [zenonSerializableEnum("S")]
    Set
  }
}
