using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Represents a label in a function block diagram.
  /// </summary>
  public class FunctionBlockDiagramLabel : FunctionBlockDiagramObject<FunctionBlockDiagramLabel>
  {
    public override string NodeName => "FBDlabel";

    /// <summary>
    /// The name of the label (mandatory)
    /// </summary>
    [zenonSerializableAttribute("label", AttributeOrder = 20)]
    public string Text { get; set; }
  }
}
