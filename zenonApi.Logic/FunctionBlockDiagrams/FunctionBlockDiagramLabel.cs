using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Represents a label in a function block diagram.
  /// </summary>
  public class FunctionBlockDiagramLabel : FunctionBlockDiagramObject<FunctionBlockDiagramLabel>
  {
    protected override string NodeName => "FBDlabel";

    /// <summary>
    /// The name of the label (mandatory)
    /// </summary>
    [zenonSerializableAttribute("label", AttributeOrder = 20)]
    public string Text { get; set; }
  }
}
