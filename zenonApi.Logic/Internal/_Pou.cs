using System;
using System.Xml.Linq;
using zenonApi.Collections;
using zenonApi.Extensions;
using zenonApi.Logic.FunctionBlockDiagrams;
using zenonApi.Logic.SequentialFunctionChart;
using zenonApi.Serialization;

namespace zenonApi.Logic.Internal
{
  /// <summary>
  /// Represents a program, sub-program or UDFB. This class is internal and not
  /// meant to be used directly.
  /// </summary>
  // ReSharper disable once InconsistentNaming : Named the class with an underscore by intend to express
  // that it is not reachable via the public API.
  internal class _Pou : zenonSerializable<_Pou, _LogicProgramsCollection, LogicProject>, ILogicProgram
  {
    /// <summary>Internal default constructor for serialization.</summary>
    // ReSharper disable once EmptyConstructor : Required default constructor for serialization.
    internal _Pou() { }


    /// <summary>
    /// If a pou was disconnected from its parent (e.g. if newly created), it is essential to add it back to the tree,
    /// which is done by this method using the connected LogicProgram.
    /// </summary>
    /// <param name="connectedProgram"></param>
    internal void AttachToProjectTreeIfRequired(LogicProgram connectedProgram)
    {
      // Remove the item from its old source collection
      this.Remove();

      // Attach it to the new one
      var pouList = connectedProgram.Root?.Programs?.ProgramOrganizationUnits;
      if (pouList != null && !pouList.Contains(this))
      {
        pouList.Add(this);
      }
    }


    #region zenonSerializable implementation
    public override string NodeName => "pou";
    #endregion

    private string _sourceCode = "";
    private FunctionBlockDiagramDefinition _functionBlockDiagramDefinition;
    private SequentialFunctionChartDefinition _sequentialFunctionChartDefinition;
    private XElement _ladderDiagramDefinition;
    private XElement _freeFormSequentialFunctionChartDefinition;
    private string _name = "unknown";

    #region Specific properties
    /// <summary>
    /// The name of the POU, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("name", AttributeOrder = 0)]
    public string Name
    {
      get => _name;
      set
      {
        if (!value.IsValidZenonLogicName())
        {
          throw new Exception($"Invalid zenon logic program name: {value}");
        }

        _name = value;
      }
    }

    /// <summary>
    /// The kind of the POU, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("kind", AttributeOrder = 1)]
    public LogicProgramType Kind { get; set; }

    /// <summary>
    /// The language of the POU, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("lge", AttributeOrder = 5)]
    public LogicProgramLanguage Language { get; set; }

    /// <summary>
    /// Name of the parent program. This attribute is mandatory for
    /// <see cref="LogicProgramType.Child"/> SFC programs.
    /// </summary>
    [zenonSerializableAttribute("parent", AttributeOrder = 2)]
    public string ParentPou { get; set; } // TODO: Shouldn't we use a reference to another pou? Validation of naming otherwise?

    /// <summary>
    /// The execution period (number of cycles) at runtime.
    /// This property is optional and its value is only used in main programs
    /// (for other <see cref="LogicProgramType"/>s it will be ignored).
    /// </summary>
    [zenonSerializableAttribute("period", AttributeOrder = 3)]
    public uint Period { get; set; }

    /// <summary>
    /// Phase for the execution period (number of cycles) at runtime.
    /// This property is optional and its value is only used in main programs.
    /// </summary>
    [zenonSerializableAttribute("phase", AttributeOrder = 4)]
    public uint Phase { get; set; }

    /// <summary>
    /// The task name for the runtime execution.
    /// This property is optional and is only used in main programs.
    /// </summary>
    [zenonSerializableAttribute("task", AttributeOrder = 6)]
    public string TaskName { get; set; }

    /// <summary>
    /// Provides an optional description text.
    /// </summary>
    [zenonSerializableAttribute("desc", AttributeOrder = 7)]
    public string Description { get; set; }

    /// <summary>
    /// Provides a multiline description attached to a program.
    /// </summary>
    [zenonSerializableNode("pounote", NodeOrder = 0)]
    public string MultiLineDescription { get; set; }

    /// <summary>
    /// Groups variables of the same variable group.
    /// </summary>
    [zenonSerializableNode("vargroup", NodeOrder = 1)]
    public ExtendedObservableCollection<LogicVariableGroup> VariableGroups { get; protected set; }
      = new ExtendedObservableCollection<LogicVariableGroup>();

    /// <summary>
    /// Describes a group definition.
    /// </summary>
    [zenonSerializableNode("defines", NodeOrder = 2)]
    public _LogicDefine Definitions { get; protected set; } = new _LogicDefine();

    /// <summary>
    /// Undocumented zenon Logic node. Contains columns displayed in the
    /// Workbench and further information.
    /// </summary>
    [zenonSerializableNode("srcdic", NodeOrder = 3)]
    public string SourceDictionary { get; set; }

    /// <summary>
    /// Contains pre-compiled code of an user defined function block imported
    /// without its source code.
    /// </summary>
    [zenonSerializableNode("pc5code", NodeOrder = 4)]
    public string PrecompiledUdfbCode { get; set; } // TODO: Should this be set to null if SourceCode is set and vice versa?

    /// <summary>
    /// Contains a piece of ST/IL source code.
    /// If this value is set to a value other than null, <see cref="FunctionBlockDiagramDefinition"/>,
    /// <see cref="SequentialFunctionChartDefinition"/>, <see cref="FreeFormSequentialFunctionChartDefinition"/>
    /// and <see cref="LadderDiagramDefinition"/> are automatically set to null.
    /// </summary>
    [zenonSerializableNode("sourceSTIL", NodeOrder = 5)]
    public string SourceCode
    {
      get => _sourceCode;
      set
      {
        if (value != null)
        {
          _functionBlockDiagramDefinition = null;
          _sequentialFunctionChartDefinition = null;
          _ladderDiagramDefinition = null;
          _freeFormSequentialFunctionChartDefinition = null;
        }

        _sourceCode = value;
      }
    }

    /// <summary>
    /// Describes a function block diagram.
    /// If this value is set to a value other than null, <see cref="SourceCode"/>,
    /// <see cref="SequentialFunctionChartDefinition"/>, <see cref="FreeFormSequentialFunctionChartDefinition"/>
    /// and <see cref="LadderDiagramDefinition"/> are automatically set to null.
    /// </summary>
    [zenonSerializableNode("sourceFBD", NodeOrder = 6)]
    public FunctionBlockDiagramDefinition FunctionBlockDiagramDefinition
    {
      get => _functionBlockDiagramDefinition;
      set
      {
        if (value != null)
        {
          _sourceCode = null;
          _sequentialFunctionChartDefinition = null;
          _ladderDiagramDefinition = null;
          _freeFormSequentialFunctionChartDefinition = null;
        }

        _functionBlockDiagramDefinition = value;
      }
    }

    /// <summary>
    /// Describes a LD diagram.
    /// If this value is set to a value other than null, <see cref="SourceCode"/>,
    /// <see cref="SequentialFunctionChartDefinition"/>, <see cref="FreeFormSequentialFunctionChartDefinition"/>
    /// and <see cref="FunctionBlockDiagramDefinition"/> are automatically set to null.
    /// </summary>
    [zenonSerializableRawFormat("sourceLD", NodeOrder = 7)]
    public XElement LadderDiagramDefinition
    {
      get => _ladderDiagramDefinition;
      set
      {
        if (value != null)
        {
          _sourceCode = null;
          _functionBlockDiagramDefinition = null;
          _sequentialFunctionChartDefinition = null;
          _freeFormSequentialFunctionChartDefinition = null;
        }

        _ladderDiagramDefinition = value;
      }
    }

    /// <summary>
    /// Describes a SFC program.
    /// If this value is set to a value other than null, <see cref="SourceCode"/>,
    /// <see cref="LadderDiagramDefinition"/>, <see cref="FunctionBlockDiagramDefinition"/>
    /// and <see cref="FreeFormSequentialFunctionChartDefinition"/> are automatically set to null.
    /// </summary>
    [zenonSerializableNode("sourceSFC", NodeOrder = 8)]
    public SequentialFunctionChartDefinition SequentialFunctionChartDefinition
    {
      get => _sequentialFunctionChartDefinition;
      set
      {
        if (value != null)
        {
          _sourceCode = null;
          _functionBlockDiagramDefinition = null;
          _ladderDiagramDefinition = null;
          _freeFormSequentialFunctionChartDefinition = null;
        }

        _sequentialFunctionChartDefinition = value;
      }
    }

    /// <summary>
    /// Describes a FFSFC program.
    /// If this value is set to a value other than null, <see cref="SourceCode"/>,
    /// <see cref="FunctionBlockDiagramDefinition"/>, <see cref="LadderDiagramDefinition"/>
    /// and <see cref="SequentialFunctionChartDefinition"/> are automatically set to null.
    /// </summary>
    [zenonSerializableRawFormat("sourceFFSFC", NodeOrder = 9)]
    public XElement FreeFormSequentialFunctionChartDefinition
    {
      get => _freeFormSequentialFunctionChartDefinition;
      set
      {
        if (value != null)
        {
          _sourceCode = null;
          _functionBlockDiagramDefinition = null;
          _ladderDiagramDefinition = null;
          _sequentialFunctionChartDefinition = null;
        }

        _freeFormSequentialFunctionChartDefinition = value;
      }
    }

    /// <summary>
    /// Undocumented zenon Logic node.
    /// </summary>
    [zenonSerializableNode("cryptcode", NodeOrder = 10)]
    public string CryptCode { get; set; }
    #endregion
  }
}
