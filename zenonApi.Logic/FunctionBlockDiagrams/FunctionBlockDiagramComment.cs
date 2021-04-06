using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Describes a "comment" box in a function block diagram.
  /// </summary>
  public class FunctionBlockDiagramComment : FunctionBlockDiagramObject<FunctionBlockDiagramComment>
  {
    public override string NodeName => "FBDcomment";

    /// <summary>
    /// The formatted text content of the comment box.
    /// </summary>
    [zenonSerializableNodeContent]
    public string Content { get; set; }
  }
}
