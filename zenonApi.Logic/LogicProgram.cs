using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using zenonApi.Serialization;
using zenonApi.Logic.Internal;
using zenonApi.Collections;
using zenonApi.Logic.FunctionBlockDiagrams;

// TODO: Check if all public setters are really wanted, e.g. for non-primitive types, because the parent and root relationship can be corrupted

namespace zenonApi.Logic
{
  /// <summary>
  /// Represents a program, sub-program or user defined function block.
  /// </summary>
  public class LogicProgram : zenonSerializable<LogicProgram, ILogicFileContainer, LogicProject>, ILogicProgram
  {
    #region Interface implementation
    public override string NodeName => "Program";
    #endregion

    #region Ctor
    private LogicProgram() { }

    internal LogicProgram(LogicFolder parent)
    {
      this.Parent = parent;
      this.Root = parent.Root;
    }
    #endregion

    #region Specific properties with their fields
    private string name;

    /// <summary>
    /// The name of the program, which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("Name", AttributeOrder = 0)]
    public string Name
    {
      get => name;
      set
      {
        // TODO: Validation if not null, if not empty, if conform with zenon logic
        name = value;
        setPouProperty(value);
      }
    }
    #endregion


    #region Properties, which are serialized/deserialized in external class _Pou
    /// <summary>
    /// The kind of the program, which is mandatory.
    /// </summary>
    public LogicProgramType Kind
    {
      get => (LogicProgramType)getPouProperty();
      set => setPouProperty(value);
    }

    /// <summary>
    /// The language of the program, which is mandatory.
    /// </summary>
    public LogicProgramLanguage Language
    {
      get => (LogicProgramLanguage)getPouProperty();
      set => setPouProperty(value);
    }

    /// <summary>
    /// Name of the parent program. This is mandatory for
    /// <see cref="LogicProgramType.Child"/> SFC programs.
    /// </summary>
    public string ParentProgram
    {
      get => (string)getPouProperty(nameof(_Pou.ParentPou));
      set => setPouProperty(value, nameof(_Pou.ParentPou));
    }

    /// <summary>
    /// The execution period (number of cycles) at runtime.
    /// This property is optional and its value is only used in main programs
    /// (for other <see cref="LogicProgramType"/>s, it will be ignored).
    /// </summary>
    public uint Period
    {
      get => (uint)getPouProperty();
      set => setPouProperty(value);
    }

    /// <summary>
    /// Phase for the execution period (number of cycles) at runtime.
    /// This property is optional and its value is only used in main programs.
    /// </summary>
    public uint Phase
    {
      get => (uint)getPouProperty();
      set => setPouProperty(value);
    }

    /// <summary>
    /// The task name for the runtime execution.
    /// This property is optional and is only used in main programs.
    /// </summary>
    public string TaskName
    {
      get => (string)getPouProperty();
      set => setPouProperty(value);
    }

    /// <summary>
    /// Provides an optional description text.
    /// </summary>
    public string Description
    {
      get => (string)getPouProperty();
      set => setPouProperty(value);
    }

    /// <summary>
    /// Provides a multiline description attached to a program.
    /// </summary>
    public string MultiLineDescription
    {
      get => (string)getPouProperty();
      set => setPouProperty(value);
    }

    /// <summary>
    /// Groups variables of the <see cref="LogicVariableGroup"/>.
    /// </summary>
    public ExtendedObservableCollection<LogicVariableGroup> VariableGroups
    {
      get => (ExtendedObservableCollection<LogicVariableGroup>)getPouProperty();
      set => setPouProperty(value);
    }

    /// <summary>
    /// Describes a group definition.
    /// </summary>
    internal _LogicDefine Definitions // TODO: Do we need to make this public?
    {
      get => (_LogicDefine)getPouProperty();
      set => setPouProperty(value);
    }

    /// <summary>
    /// Contains pre-compiled code of an user defined function block imported
    /// without its source code.
    /// </summary>
    public string PrecompiledUdfbCode
    {
      get => (string)getPouProperty();
      set => setPouProperty(value);
    }

    /// <summary>
    /// Contains a piece of ST/IL source code.
    /// </summary>
    public string SourceCode
    {
      get => (string)getPouProperty();
      set => setPouProperty(value);
    }

    // TODO: FunctionBlockDiagramDefinition, FB-LD, FB-SFC are exclusive and cannot appear together --> shall we set the other to null on setting them?

    /// <summary>
    /// Describes a function block diagram.
    /// </summary>
    public FunctionBlockDiagramDefinition FunctionBlockDiagramDefinition
    {
      get => (FunctionBlockDiagramDefinition)getPouProperty();
      set => setPouProperty(value);
    }

    // TODO sourceLD, order = 6

    // TODO sourceSFC, order = 7

    /// <summary>
    /// Undocumented zenon Logic node. Contains columns displayed in the
    /// Workbench and further information.
    /// </summary>
    public string SourceDictionary
    {
      get => (string)getPouProperty();
      set => setPouProperty(value);
    }

    /// <summary>
    /// Undocumented zenon Logic node.
    /// </summary>
    public string CryptCode
    {
      get => (string)getPouProperty();
      set => setPouProperty(value);
    }
    #endregion


    #region Private/Internal methods and properties to handle connected _Pou instances
    private _Pou connectedPou;

    internal _Pou GetOrCreateConnectedPou()
    {
      if (connectedPou != null)
      {
        // A POU was already set, but we have to ensure that it is also connected to the corresponding
        // LogicProject tree, which might have changed in the meantime.
        connectedPou.AttachToProjectTreeIfRequired(this);
        return connectedPou;
      }

      // POU is null, this may be the reason if this is the first access of the
      // current instance; try to find a matching poo
      if (this.Root != null)
      {
        connectedPou = this.Root.Programs?.ProgramOrganizationUnits?.Where(x => x.Name == this.Name).FirstOrDefault();
      }

      // If no POU was found so far, we need to create one. This may be the reason if the current Program instance
      // is not attached to any LogicProject tree yet and is created newly.
      if (connectedPou == null)
      {
        connectedPou = new _Pou()
        {
          Name = this.Name
        };

        connectedPou.AttachToProjectTreeIfRequired(this);
      }

      return connectedPou;
    }


    private object getPouProperty([CallerMemberName]string propertyName = null)
    {
      var pou = GetOrCreateConnectedPou();
      if (pou == null)
      {
        throw new Exception($"Cannot find the {nameof(_Pou)} named \"{this.Name}\".");
      }

      var property = typeof(_Pou).GetRuntimeProperties().Where(x => x.Name == propertyName).FirstOrDefault();
      if (property == null)
      {
        throw new Exception($"{nameof(_Pou)} does not have any properties named \"{propertyName}\"");
      }

      return property.GetValue(pou);
    }

    private void setPouProperty(object value, [CallerMemberName]string propertyName = null)
    {
      var pou = GetOrCreateConnectedPou();
      if (pou == null)
      {
        throw new Exception($"Cannot find the {nameof(_Pou)} named \"{this.Name}\".");
      }

      var property = typeof(_Pou).GetRuntimeProperties().Where(x => x.Name == propertyName).FirstOrDefault();
      if (property == null)
      {
        throw new Exception($"{nameof(_Pou)} does not have any properties named \"{propertyName}\"");
      }

      property.SetValue(pou, value);
    }
    #endregion
  }
}
