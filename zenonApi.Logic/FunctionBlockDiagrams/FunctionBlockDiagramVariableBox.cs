using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Describes a "variable" box in a FBD diagram.
  /// </summary>
  public class FunctionBlockDiagramVariableBox : FunctionBlockDiagramObject<FunctionBlockDiagramVariableBox>
  {
    protected override string NodeName => "FBDvarbox";

    /// <summary>
    /// The symbol of the attached variable in the project, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("symbol", AttributeOrder = 5)]
    public string Symbol { get; set; }
  }
}
