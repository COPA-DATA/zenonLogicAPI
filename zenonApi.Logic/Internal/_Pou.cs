using System.Collections.Generic;
using zenonApi.Serialization;
using zenonApi.Collections;
using System.ComponentModel;
using zenonApi.Logic.FunctionBlockDiagrams;

namespace zenonApi.Logic.Internal
{
  /// <summary>
  /// Represents a program, sub-program or UDFB. This class is internal and not
  /// meant to be used directly.
  /// </summary>
  internal class _Pou : zenonSerializable<_Pou, _LogicProgramsCollection, LogicProject>, ILogicVariableGroupContainer, ILogicProgram
  {
    /// <summary>
    /// Internal default constructor for serialization.
    /// </summary>
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
      if (connectedProgram.Root?.Programs?.ProgramOrganizationUnits != null)
      {
        connectedProgram.Root.Programs.ProgramOrganizationUnits.Add(this);
      }
    }


    #region zenonSerializable implementation
    IZenonSerializable IZenonSerializable<IZenonSerializable, LogicProject>.Parent => this.Parent;
    protected override string NodeName => "pou";
    #endregion

    #region Specific properties
    /// <summary>
    /// The name of the POU, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("name", AttributeOrder = 0)]
    public string Name { get; set; } // TODO: Validation if not null, if not empty, if conform with zenon logic

    /// <summary>
    /// The kind of the POU, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("kind", AttributeOrder = 1)]
    public LogicProgramType Kind { get; set; }

    /// <summary>
    /// The language of the POU, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("lge", AttributeOrder = 2)]
    public LogicProgramLanguage Language { get; set; }

    /// <summary>
    /// Name of the parent program. This attribute is mandatory for
    /// <see cref="LogicProgramType.Child"/> SFC programs.
    /// </summary>
    [zenonSerializableAttribute("parent", AttributeOrder = 3)]
    public string ParentPou { get; set; } // TOOD; Shouldn't we use a reference to another pou?

    /// <summary>
    /// The execution period (number of cycles) at runtime.
    /// This property is optional and its value is only used in main programs
    /// (for other <see cref="LogicProgramType"/>s it will be ignored).
    /// </summary>
    [zenonSerializableAttribute("period", AttributeOrder = 4)]
    public uint Period { get; set; }

    /// <summary>
    /// Phase for the execution period (number of cycles) at runtime.
    /// This property is optional and its value is only used in main programs.
    /// </summary>
    [zenonSerializableAttribute("phase", AttributeOrder = 5)]
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
    public ContainerAwareObservableCollection<LogicVariableGroup> VariableGroups { get; set; }

    /// <summary>
    /// Describes a group definition.
    /// </summary>
    [zenonSerializableNode("defines", NodeOrder = 2)]
    public _LogicDefine Definitions { get; set; }

    /// <summary>
    /// Contains pre-compiled code of an user defined function block imported
    /// without its source code.
    /// </summary>
    [zenonSerializableNode("pc5code", NodeOrder = 3)]
    public string PrecompiledUdfbCode { get; set; }

    /// <summary>
    /// Contains a piece of ST/IL source code.
    /// </summary>
    [zenonSerializableNode("sourceSTIL", NodeOrder = 4)]
    public string SourceCode { get; set; }

    /// <summary>
    /// Describes a function block diagram.
    /// </summary>
    [zenonSerializableNode("sourceFBD", NodeOrder = 5)]
    public FunctionBlockDiagramDefinition FunctionBlockDiagramDefinition { get; set; }

    // TODO sourceLD, order = 6

    // TODO sourceSFC, order = 7

    /// <summary>
    /// Undocumented zenon Logic node. Contains columns displayed in the
    /// Workbench and further information.
    /// </summary>
    [zenonSerializableNode("srcdic", NodeOrder = 8)]
    public string SourceDictionary { get; set; }

    /// <summary>
    /// Undocumented zenon Logic node.
    /// </summary>
    [zenonSerializableNode("cryptcode", NodeOrder = 9)]
    public string CryptCode { get; set; }
    #endregion
  }
}
