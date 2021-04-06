﻿using System.Xml.Linq;
using zenonApi.Serialization;

namespace zenonApi.Logic.SequentialFunctionChart
{
  public class SequentialFunctionChartAction : zenonSerializable<SequentialFunctionChartAction>
  {
    private string _sourceCode = "";
    private FunctionBlockDiagrams.FunctionBlockDiagramDefinition _functionBlockDiagramDefinition;
    private XElement _ladderDiagramDefinition;

    public override string NodeName => "SFCaction";

    /// <summary>
    /// The kind of an <see cref="SequentialFunctionChartAction"/>.
    /// </summary>
    [zenonSerializableAttribute("kind", AttributeOrder = 0)]
    public SequentialFunctionChartActionKind Kind { get; set; }

    /// <summary>
    /// Contains a piece of ST/IL source code.
    /// If this value is set to a value other than null, <see cref="FunctionBlockDiagramDefinition"/>
    /// and <see cref="LadderDiagramDefinition"/> are automatically set to null.
    /// </summary>
    [zenonSerializableNode("sourceSTIL", NodeOrder = 0)]
    public string SourceCode
    {
      get => _sourceCode;
      set
      {
        if (value != null)
        {
          _functionBlockDiagramDefinition = null;
          _ladderDiagramDefinition = null;
        }

        _sourceCode = value;
      }
    }

    /// <summary>
    /// Describes a function block diagram.
    /// If this value is set to a value other than null, <see cref="SourceCode"/>
    /// and <see cref="LadderDiagramDefinition"/> are automatically set to null.
    /// </summary>
    [zenonSerializableNode("sourceFBD", NodeOrder = 1)]
    public FunctionBlockDiagrams.FunctionBlockDiagramDefinition FunctionBlockDiagramDefinition
    {
      get => _functionBlockDiagramDefinition;
      set
      {
        if (value != null)
        {
          _sourceCode = null;
          _ladderDiagramDefinition = null;
        }

        _functionBlockDiagramDefinition = value;
      }
    }

    /// <summary>
    /// Describes a LD diagram.
    /// If this value is set to a value other than null, <see cref="SourceCode"/>
    /// and <see cref="FunctionBlockDiagramDefinition"/> are automatically set to null.
    /// </summary>
    [zenonSerializableNode("sourceLD", NodeOrder = 2)]
    public XElement LadderDiagramDefinition
    {
      get => _ladderDiagramDefinition;
      set
      {
        if (value != null)
        {
          _sourceCode = null;
          _functionBlockDiagramDefinition = null;
        }

        _ladderDiagramDefinition = value;
      }
    } // TODO: If Ld diagrams are implemented, correct the type here
  }
}
