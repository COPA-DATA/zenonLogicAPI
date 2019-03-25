﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using zenonApi.Serialization;

namespace zenonApi.Logic.SequenceFlowChart
{
  public class Action : zenonSerializable<Action>
  {
    private string sourceCode;
    private FunctionBlockDiagrams.FunctionBlockDiagramDefinition functionBlockDiagramDefinition;
    private XElement ldDiagramDefinition;

    public override string NodeName => "SFCaction";

    /// <summary>
    /// The kind of an <see cref="Action"/>.
    /// </summary>
    [zenonSerializableAttribute("kind", AttributeOrder = 0)]
    public ActionKind Kind { get; set; }

    /// <summary>
    /// Contains a piece of ST/IL source code.
    /// If this value is set to a value other than null, <see cref="FunctionBlockDiagramDefinition"/>
    /// and <see cref="LdDiagramDefinition"/> are automatically set to null.
    /// </summary>
    [zenonSerializableNode("sourceSTIL", NodeOrder = 0)]
    public string SourceCode {
      get => sourceCode;
      set
      {
        if (value != null)
        {
          functionBlockDiagramDefinition = null;
          ldDiagramDefinition = null;
        }

        sourceCode = value;
      }
    }

    /// <summary>
    /// Describes a function block diagram.
    /// If this value is set to a value other than null, <see cref="SourceCode"/>
    /// and <see cref="LdDiagramDefinition"/> are automatically set to null.
    /// </summary>
    [zenonSerializableNode("sourceFBD", NodeOrder = 1)]
    public FunctionBlockDiagrams.FunctionBlockDiagramDefinition FunctionBlockDiagramDefinition
    {
      get => functionBlockDiagramDefinition;
      set
      {
        if (value != null)
        {
          sourceCode = null;
          ldDiagramDefinition = null;
        }

        functionBlockDiagramDefinition = value;
      }
    }

    /// <summary>
    /// Describes a LD diagram.
    /// If this value is set to a value other than null, <see cref="SourceCode"/>
    /// and <see cref="FunctionBlockDiagramDefinition"/> are automatically set to null.
    /// </summary>
    [zenonSerializableNode("sourceLD", NodeOrder = 2)]
    public XElement LdDiagramDefinition {
      get => ldDiagramDefinition;
      set
      {
        if (value != null)
        {
          sourceCode = null;
          functionBlockDiagramDefinition = null;
        }

        ldDiagramDefinition = value;
      }
    } // TODO: If Ld diagrams are implemented, correct the type here
  }
}
