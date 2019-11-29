using System.Xml.Linq;
using zenonApi.Serialization;

namespace zenonApi.Logic.SequentialFunctionChart
{
  /// <summary>
  /// Describes the condition attached to a SFC transition.
  /// </summary>
  public class SequentialFunctionChartCondition : zenonSerializable<SequentialFunctionChartCondition>
  {
    public override string NodeName => "SFCcondition";

    private string _sourceCode = "";
    private XElement _ladderDiagramDefinition;

    /// <summary>
    /// Contains a piece of ST/IL source code.
    /// If this property is set to a value other than null,
    /// <see cref="LadderDiagramDefinition"/> is automatically set to null.
    /// </summary>
    [zenonSerializableNode("sourceSTIL")]
    public string SourceCode
    {
      get => _sourceCode;
      set
      {
        if (value != null)
        {
          _ladderDiagramDefinition = null;
        }

        _sourceCode = value;
      }
    }

    /// <summary>
    /// Describes a LD diagram.
    /// If this property is set to a value other than null,
    /// <see cref="SourceCode"/> is automatically set to null.
    /// </summary>
    [zenonSerializableRawFormat("sourceLD")]
    public XElement LadderDiagramDefinition
    {
      get => _ladderDiagramDefinition;
      set
      {
        if (value != null)
        {
          _sourceCode = null;
        }

        _ladderDiagramDefinition = value;
      }
    } // TODO: if LD is implemented, change this to the actual type and tag
  }
}
