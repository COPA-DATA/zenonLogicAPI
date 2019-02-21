using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.Logic.Internal
{
  /// <summary>
  /// Represents a program, sub-program or UDFB. This class is internal and not
  /// meant to be used directly.
  /// </summary>
  internal class _Pou : zenonSerializable<_Pou, _LogicPrograms, LogicProject>, ILogicVariableGroupContainer
  {
    /// <summary>
    /// Private default constructor for serialization.
    /// </summary>
    protected _Pou() { }


    #region zenonSerializable implementation
    IZenonSerializable IZenonSerializable<IZenonSerializable, LogicProject>.Parent => this.Parent;
    protected override string NodeName => "pou";
    #endregion

    #region Specific properties
    /// <summary>
    /// The name of the POU, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("name")]
    public string Name { get; set; } // TODO: Validation if not null, if not empty, if conform with zenon logic

    /// <summary>
    /// The kind of the POU, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("kind")]
    public LogicProgramType Kind { get; set; }

    /// <summary>
    /// The language of the POU, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("lge")]
    public LogicProgramLanguage Language { get; set; }

    /// <summary>
    /// Name of the parent program. This attribute is mandatory for
    /// <see cref="LogicProgramType.Child"/> SFC programs.
    /// </summary>
    [zenonSerializableAttribute("parent")]
    public string ParentPou { get; set; } // TOOD; gather this value from the ApplicationTree

    /// <summary>
    /// The execution period (number of cycles) at runtime.
    /// This property is optional and its value is only used in main programs.
    /// (for other <see cref="LogicProgramType"/> it will be ignored).
    /// </summary>
    [zenonSerializableAttribute("period")]
    public uint Period { get; set; }

    /// <summary>
    /// Phase for the execution period (number of cycles) at runtime.
    /// This property is optional and its value is only used in main programs.
    /// </summary>
    [zenonSerializableAttribute("phase")]
    public uint Phase { get; set; }

    /// <summary>
    /// The task name for the runtime execution.
    /// This property is optional and is only used in main programs.
    /// </summary>
    [zenonSerializableAttribute("task")]
    public string TaskName { get; set; }

    /// <summary>
    /// Provides an optional description text.
    /// </summary>
    [zenonSerializableAttribute("desc")]
    public string Description { get; set; }

    /// <summary>
    /// Provides a multiline description attached to a program.
    /// </summary>
    [zenonSerializableNode("pounote")]
    public string MultiLineDescription { get; set; }

    [zenonSerializableNode("vargroup")]
    public List<LogicVariableGroup> VariableGroups { get; set; }

    /// <summary>
    /// Describes a group definition.
    /// </summary>
    [zenonSerializableNode("defines")]
    public _LogicDefine Definitions { get; set; }

    /// <summary>
    /// Contains pre-compiled code of an user defined function block imported
    /// without its source code.
    /// </summary>
    [zenonSerializableNode("pc5code")]
    public string PrecompiledUdfbCode { get; set; }

    /// <summary>
    /// Contains a piece of ST/IL source code.
    /// </summary>
    [zenonSerializableNode("sourceSTIL")]
    public string SourceCode { get; set; }

    // TODO sourceFBD

    // TODO sourceLD

    // TODO sourceSFC

    /// <summary>
    /// Undocumented zenon Logic node. Contains columns displayed in the
    /// Workbench and further information.
    /// </summary>
    [zenonSerializableNode("srcdic")]
    public string SourceDictionary { get; set; }

    /// <summary>
    /// Undocumented zenon Logic node.
    /// </summary>
    [zenonSerializableNode("cryptcode")]
    public string CryptCode { get; set; }
    #endregion
  }
}
