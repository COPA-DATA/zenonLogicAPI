using System.Xml.Linq;
using zenonApi.Serialization;

namespace zenonApi.Logic.SequenceFlowChart
{
  /// <summary>
  /// Describes the condition attached to a SFC transition.
  /// </summary>
  public class Condition : zenonSerializable<Condition>
  {
    public override string NodeName => "SFCcondition";

    private string sourceCode;
    private XElement ldDiagramDefinition;

    /// <summary>
    /// Contains a piece of ST/IL source code.
    /// If this property is set to a value other than null,
    /// <see cref="LdDiagramDefinition"/> is automatically set to null.
    /// </summary>
    [zenonSerializableNode("sourceSTIL")]
    public string SourceCode
    {
      get => sourceCode;
      set
      {
        if (value != null)
        {
          ldDiagramDefinition = null;
        }

        sourceCode = value;
      }
    }

    /// <summary>
    /// Describes a LD diagram.
    /// If this property is set to a value other than null,
    /// <see cref="SourceCode"/> is automatically set to null.
    /// </summary>
    [zenonSerializableRawFormat("sourceLD")]
    public XElement LdDiagramDefinition
    {
      get => ldDiagramDefinition;
      set
      {
        if (value != null)
        {
          sourceCode = null;
        }

        ldDiagramDefinition = value;
      }
    } // TODO: if LD is implemented, change this to the actual type and tag
  }
}
