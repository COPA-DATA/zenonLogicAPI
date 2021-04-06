using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Represents a jump or "&lt;RETURN&gt;" item in a FBD diagram.
  /// </summary>
  public class FunctionBlockDiagramJump : FunctionBlockDiagramObject<FunctionBlockDiagramJump>
  {
    public override string NodeName => "FBDjump";

    /// <summary>
    /// The name of the destination label or "&lt;RETURN&gt;" (mandatory)
    /// </summary>
    [zenonSerializableAttribute("label", AttributeOrder = 20)]
    public string Text { get; set; }
  }
}
