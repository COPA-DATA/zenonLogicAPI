using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Describes a contact in a FBD diagram.
  /// </summary>
  public class FunctionBlockDiagramContact : FunctionBlockDiagramObject<FunctionBlockDiagramContact>
  {
    protected override string NodeName => "FBDcontact";

    /// <summary>
    /// The kind of a contact in a FBD diagram.
    /// </summary>
    [zenonSerializableAttribute("kind", AttributeOrder = 0)]
    public FunctionBlockContactKind Kind { get; set; }

    /// <summary>
    /// Symbol of the attached variable (mandatory).
    /// </summary>
    [zenonSerializableAttribute("symbol", AttributeOrder = 20)]
    public string Symbol { get; set; }
  }
}
