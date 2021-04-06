using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using zenonApi.Collections;
using zenonApi.Logic.FunctionBlockDiagrams;
using zenonApi.Logic.Internal;
using zenonApi.Logic.Resources;
using zenonApi.Logic.SequentialFunctionChart;
using zenonApi.Serialization;

// TODO: Check if all public setters are really wanted, e.g. for non-primitive types, because the parent and root relationship can be corrupted

namespace zenonApi.Logic
{
  /// <summary>
  /// Represents a program, sub-program or user defined function block.
  /// </summary>
  [DebuggerDisplay("{" + nameof(Name) + "}")]
  public class LogicProgram : zenonSerializable<LogicProgram, ILogicFileContainer, LogicProject>, ILogicProgram
  {
    private const uint DefaultPeriodValueForStProgram = 1;
    private const uint DefaultPhaseValueForStProgram = 0;

    #region Interface implementation
    public override string NodeName => "Program";
    #endregion

    #region Ctor and factory methods

    /// <summary>Default constructor for serialization.</summary>
    private LogicProgram() { }
    
    
    public LogicProgram(string programName)
    {
      if (string.IsNullOrWhiteSpace(programName))
      {
        throw new ArgumentNullException(
          string.Format(Strings.ErrorMessageParameterIsNullOrWhitespace, nameof(LogicProgram), nameof(programName)));
      }

      Name = programName;
      Phase = DefaultPhaseValueForStProgram;
      Period = DefaultPeriodValueForStProgram;
      Language = LogicProgramLanguage.StructuredText;
      Kind = LogicProgramType.Program;
    }

    public static LogicProgram CreateStructuredTextProgram(string programName, LogicProgramType programType = LogicProgramType.Program)
    {
      if (string.IsNullOrWhiteSpace(programName))
      {
        throw new ArgumentNullException(
          string.Format(Strings.ErrorMessageParameterIsNullOrWhitespace, nameof(CreateStructuredTextProgram), nameof(programName)));
      } 

      LogicProgram logicProgram = new LogicProgram
      {
        Name = programName,
        Kind = programType,
        Period = DefaultPeriodValueForStProgram,
        Phase = DefaultPhaseValueForStProgram,
        Language = LogicProgramLanguage.StructuredText
      };

      logicProgram.VariableGroups.Add(LogicVariableGroup.Create(logicProgram.Name));

      return logicProgram;
    }
    #endregion

    #region Specific properties with their fields
    /// <summary>
    /// We require this property for serialization, since the name is also linked to the connected POU.
    /// This means, if we would set the name during serialization, no 
    /// <see cref="zenonSerializable{TSelf, TParent, TRoot}.Root"/> is known at this point although it would be set
    /// during the further serialization steps. The method <see cref="GetOrCreateConnectedPou(bool)"/> would generate
    /// a new POU on accessing the <see cref="Name"/> instead and we would loose the reference.
    /// </summary>
    [zenonSerializableAttribute("Name", AttributeOrder = 0)]
    // ReSharper disable once InconsistentNaming : Serialization requires a property,
    // to emphasize to not to use it, the naming was chosen by intend.
    private string _name { get; set; }

    /// <summary>
    /// The name of the program, which is mandatory.
    /// </summary>
    public string Name
    {
      get => _name;
      set
      {
        _name = value;
        // Validation is done via the setter of _Pou.Name.
        SetPouProperty(value);
      }
    }
    #endregion

    #region Properties, which are serialized/deserialized in external class _Pou

    /// <summary>
    /// The kind of the program, which is mandatory.
    /// </summary>
    public LogicProgramType Kind
    {
      get => (LogicProgramType)GetPouProperty();
      set => SetPouProperty(value);
    }

    /// <summary>
    /// The language of the program, which is mandatory.
    /// </summary>
    public LogicProgramLanguage Language
    {
      get => (LogicProgramLanguage)GetPouProperty();
      set => SetPouProperty(value);
    }

    /// <summary>
    /// Name of the parent program. This is mandatory for
    /// <see cref="LogicProgramType.Child"/> SFC programs.
    /// </summary>
    public string ParentProgram
    {
      get => (string)GetPouProperty(nameof(_Pou.ParentPou));
      set => SetPouProperty(value, nameof(_Pou.ParentPou));
    }

    /// <summary>
    /// The execution period (number of cycles) at runtime.
    /// This property is optional and its value is only used in main programs
    /// (for other <see cref="LogicProgramType"/>s, it will be ignored).
    /// </summary>
    public uint Period
    {
      get => (uint) GetPouProperty();
      set => SetPouProperty(value);
    }

    /// <summary>
    /// Phase for the execution period (number of cycles) at runtime.
    /// This property is optional and its value is only used in main programs.
    /// </summary>
    public uint Phase
    {
      get => (uint)GetPouProperty();
      set => SetPouProperty(value);
    }

    /// <summary>
    /// The task name for the runtime execution.
    /// This property is optional and is only used in main programs.
    /// </summary>
    public string TaskName
    {
      get => (string)GetPouProperty();
      set => SetPouProperty(value);
    }

    /// <summary>
    /// Provides an optional description text.
    /// </summary>
    public string Description
    {
      get => (string)GetPouProperty();
      set => SetPouProperty(value);
    }

    /// <summary>
    /// Provides a multi-line description attached to a program.
    /// </summary>
    public string MultiLineDescription
    {
      get => (string)GetPouProperty();
      set => SetPouProperty(value);
    }

    /// <summary>
    /// Groups variables of the <see cref="LogicVariableGroup"/>.
    /// </summary>
    public ExtendedObservableCollection<LogicVariableGroup> VariableGroups
    {
      get => (ExtendedObservableCollection<LogicVariableGroup>)GetPouProperty();
      protected set => SetPouProperty(value);
    }

    /// <summary>
    /// Describes a group definition.
    /// </summary>
    internal _LogicDefine Definitions // TODO: Do we need to make this public?
    {
      get => (_LogicDefine)GetPouProperty();
      set => SetPouProperty(value);
    }

    /// <summary>
    /// Contains pre-compiled code of an user defined function block imported
    /// without its source code.
    /// </summary>
    public string PrecompiledUdfbCode
    {
      get => (string)GetPouProperty();
      set => SetPouProperty(value);
    }

    /// <summary>
    /// Contains a piece of ST/IL source code.
    /// If this value is set to a value other than null, <see cref="FunctionBlockDiagramDefinition"/>,
    /// <see cref="SequentialFunctionChartDefinition"/> and <see cref="LadderDiagramDefinition"/> are automatically set to null.
    /// </summary>
    public string SourceCode
    {
      get => (string)GetPouProperty();
      set => SetPouProperty(value);
    }

    /// <summary>
    /// Describes a function block diagram.
    /// If this value is set to a value other than null, <see cref="SourceCode"/>,
    /// <see cref="SequentialFunctionChartDefinition"/> and <see cref="LadderDiagramDefinition"/> are automatically set to null.
    /// </summary>
    public FunctionBlockDiagramDefinition FunctionBlockDiagramDefinition
    {
      get => (FunctionBlockDiagramDefinition)GetPouProperty();
      set => SetPouProperty(value);
    }

    /// <summary>
    /// Describes a LD diagram.
    /// If this value is set to a value other than null, <see cref="SourceCode"/>,
    /// <see cref="SequentialFunctionChartDefinition"/>
    /// and <see cref="FunctionBlockDiagramDefinition"/> are automatically set to null.
    /// </summary>
    public XElement LadderDiagramDefinition
    {
      // TODO: If LD is implemented, change this from XElement to the according type
      get => (XElement)GetPouProperty(nameof(_Pou.LadderDiagramDefinition));
      set => SetPouProperty(value, nameof(_Pou.LadderDiagramDefinition));
    }

    /// <summary>
    /// Describes a SFC program.
    /// If this value is set to a value other than null, <see cref="SourceCode"/>,
    /// <see cref="LadderDiagramDefinition"/> and <see cref="FunctionBlockDiagramDefinition"/>
    /// are automatically set to null.
    /// </summary>
    public SequentialFunctionChartDefinition SequentialFunctionChartDefinition
    {
      get => (SequentialFunctionChartDefinition)GetPouProperty();
      set => SetPouProperty(value);
    }

    /// <summary>
    /// Undocumented zenon Logic node. Contains columns displayed in the
    /// Workbench and further information.
    /// </summary>
    public string SourceDictionary
    {
      get => (string)GetPouProperty();
      set => SetPouProperty(value);
    }

    /// <summary>
    /// Undocumented zenon Logic node.
    /// </summary>
    public string CryptCode
    {
      get => (string)GetPouProperty();
      set => SetPouProperty(value);
    }

    #endregion

    #region Private/Internal methods and properties to handle connected _Pou instances

    private _Pou _connectedPou;

    internal _Pou GetOrCreateConnectedPou(bool getOnly = false)
    {
      if (_connectedPou != null)
      {
        // A POU was already set, but we have to ensure that it is also connected to the corresponding
        // LogicProject tree, which might have changed in the meantime.
        _connectedPou.AttachToProjectTreeIfRequired(this);
        return _connectedPou;
      }

      // POU is null, this may be the reason if this is the first access of the
      // current instance; try to find a matching poo
      if (this.Root != null)
      {
        _connectedPou = this.Root.Programs?.ProgramOrganizationUnits?.Where(x => x.Name == this.Name).FirstOrDefault();
      }

      // If no POU was found so far, we need to create one. This may be the reason if the current Program instance
      // is not attached to any LogicProject tree yet and is created newly.
      if (_connectedPou == null && !getOnly)
      {
        _connectedPou = new _Pou()
        {
          Name = this.Name
        };

        _connectedPou.AttachToProjectTreeIfRequired(this);
      }

      return _connectedPou;
    }


    private object GetPouProperty([CallerMemberName]string propertyName = null)
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

    private void SetPouProperty(object value, [CallerMemberName]string propertyName = null)
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

    #region Overrides

    public override LogicProject Root
    {
      get
      {
        if (base.Root != Parent?.Root)
        {
          base.Root = Parent?.Root;
          GetOrCreateConnectedPou();
        }

        return base.Root;
      }
      protected set => base.Root = value;
    }

    #endregion
  }

  #region extension methods for program management

  [Browsable(false)]
  public static class LogicProgramExtensions
  {
    public static LogicProgram GetByName(this IEnumerable<LogicProgram> self, string programName,
      StringComparison comparison = StringComparison.Ordinal)
    {
      if (string.IsNullOrEmpty(programName))
      {
        return null;
      }

      return self.FirstOrDefault(logicProgram => logicProgram?.Name.Equals(programName, comparison) ?? false);
    }

    public static bool Contains(this IEnumerable<LogicProgram> self, string programName,
      StringComparison comparison = StringComparison.Ordinal)
    {
      return self.GetByName(programName, comparison) != null;
    }
  }

  #endregion
}
