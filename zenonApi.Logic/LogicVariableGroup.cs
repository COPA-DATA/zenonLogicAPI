using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using zenonApi.Collections;
using zenonApi.Logic.Resources;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class LogicVariableGroup : zenonSerializable<LogicVariableGroup>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "vargroup";
    #endregion

    public static LogicVariableGroup Create(string variableGroupName,
      LogicVariableKind variableKind = LogicVariableKind.Local)
    {
      if (string.IsNullOrWhiteSpace(variableGroupName))
      {
        throw new ArgumentNullException(
          string.Format(Strings.ErrorMessageParameterIsNullOrWhitespace, nameof(Create),
            nameof(variableGroupName)));
      }

      LogicVariableGroup newLogicVariableGroup = new LogicVariableGroup
      {
        Name = variableGroupName,
        Kind = variableKind
      };

      return newLogicVariableGroup;
    }

    /// <summary>
    /// Name of the group.
    /// This attribute is mandatory.
    /// </summary>
    [zenonSerializableAttribute("name", AttributeOrder = 0)]
    public string Name { get; set; } // TODO: Check for correct naming? (StringExtensions.IsValidZenonLogicName)

    /// <summary>
    /// Kind of variable group.
    /// This attribute is mandatory.
    /// </summary>
    [zenonSerializableAttribute("kind", AttributeOrder = 1)]
    public LogicVariableKind Kind { get; set; }

    /// <summary>
    /// List of variables of the group.
    /// </summary>
    [zenonSerializableNode("var", NodeOrder = 2)]
    public ExtendedObservableCollection<LogicVariable> Variables { get; protected set; }
      = new ExtendedObservableCollection<LogicVariable>();
  }

  #region extension methods for variable group management

  [Browsable(false)]
  public static class LogicVariableGroupExtensions
  {
    public static LogicVariableGroup GetByName(this IEnumerable<LogicVariableGroup> self, string variableGroupName,
      StringComparison comparison = StringComparison.Ordinal)
    {
      if (string.IsNullOrWhiteSpace(variableGroupName))
      {
        return null;
      }

      return self.FirstOrDefault(logicVariableGroup =>
        logicVariableGroup?.Name.Equals(variableGroupName, comparison) ?? false);
    }

    public static LogicVariableGroup GetByKind(this IEnumerable<LogicVariableGroup> self, LogicVariableKind variableKind)
    {
      return self.FirstOrDefault(logicVariableGroup => logicVariableGroup.Kind.Equals(variableKind));
    }
  }

  #endregion
}
