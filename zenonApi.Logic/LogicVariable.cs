using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using zenonApi.Collections;
using zenonApi.Logic.Resources;
using zenonApi.Logic.SerializationConverters;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class LogicVariable : zenonSerializable<LogicVariable>
  {
    #region zenonSerializable Implementation

    public override string NodeName => "var";

    #endregion

    private const string DataTypeStringVariable = "STRING";

    /// <summary>
    /// Creates a new instance of a zenon Logic variable with the stated variable name and data type.
    /// </summary>
    /// <param name="variableName">The name of the variable to create.</param>
    /// <param name="dataType">The type of the variable to create.</param>
    /// <returns></returns>
    public static LogicVariable Create(string variableName, string dataType)
    {
      if (string.IsNullOrWhiteSpace(variableName))
      {
        throw new ArgumentNullException(
          string.Format(Strings.ErrorMessageParameterIsNullOrWhitespace, nameof(variableName)), nameof(dataType));
      }

      if (string.IsNullOrWhiteSpace(dataType))
      {
        throw new ArgumentNullException(
          string.Format(Strings.ErrorMessageParameterIsNullOrWhitespace, nameof(dataType)), nameof(dataType));
      }

      return new LogicVariable
      {
        Name = variableName,
        Type = dataType,
      };
    }

    /// <summary>
    /// Creates new instance of zenon Logic variable with string data type and a maximum string length of 255 characters.
    /// </summary>
    /// <param name="variableName"></param>
    /// <returns></returns>
    public static LogicVariable CreateStringVariable(string variableName)
    {
      LogicVariable newVariable = Create(variableName, DataTypeStringVariable);
      newVariable.MaxStringLength = Strings.StringVariableDefaultMaxLength;
      return newVariable;
    }

    /// <summary>
    /// Symbol of the variable.
    /// This attribute is mandatory.
    /// </summary>
    [zenonSerializableAttribute("name", AttributeOrder = 0)]
    public string Name { get; set; } // TODO: Check for correct naming? (StringExtensions.IsValidZenonLogicName)

    /// <summary>
    /// Name of the data type of the variable.
    /// This attribute is mandatory.
    /// </summary>
    [zenonSerializableAttribute("type", AttributeOrder = 1)]
    public string Type { get; set; }

    /// <summary>
    /// Maximum length if the data type is STRING.
    /// This attribute is mandatory for STRING variables, and should not appear for other data types.
    /// </summary>
    [zenonSerializableAttribute("len", AttributeOrder = 2)]
    public string MaxStringLength { get; set; } // TODO: Should only be used when datatype is string //TODO: should be nullable int (Nullable not yet supported by Core)

    /// <summary>
    /// Dimension(s) if the variable is an array.
    /// There are at most 3 dimensions, separated by commas.
    /// This attribute is optional.
    /// </summary>
    [zenonSerializableAttribute("dim", AttributeOrder = 3, Converter = typeof(DimensionConverter))]
    public LogicDimension ArrayDimension { get; set; } = null;

    /// <summary>
    /// Attributes of the variable, separated by comas.
    /// This attribute is optional.
    /// </summary>
    [zenonSerializableAttribute("attr", AttributeOrder = 4, Converter = typeof(LogicVariableAttributeConverter))]
    public LogicVariableAttributes Attributes { get; protected set; } = new LogicVariableAttributes();

    /// <summary>
    /// Initial value of the variable.
    /// Must be a valid constant expression that fits the data type.
    /// This attribute is optional.
    /// </summary>
    [zenonSerializableAttribute("init", AttributeOrder = 5)]
    public string InitialValue { get; set; }

    /// <summary>
    /// Indicates additional information for the variable it belongs to.
    /// </summary>
    [zenonSerializableNode("varinfo", NodeOrder = 0)]
    public ExtendedObservableCollection<LogicVariableInfo> VariableInfos { get; set; }
      = new ExtendedObservableCollection<LogicVariableInfo>();
  }

  #region extension methods for variable management

  [Browsable(false)]
  public static class LogicVariableExtensions
  {
    public static LogicVariable GetByName(this IEnumerable<LogicVariable> self, string variableName,
      StringComparison comparison = StringComparison.Ordinal)
    {
      if (string.IsNullOrEmpty(variableName))
      {
        return null;
      }

      return self.FirstOrDefault(logicVariable => logicVariable?.Name.Equals(variableName, comparison) ?? false);
    }

    public static IEnumerable<LogicVariable> GetByType(this IEnumerable<LogicVariable> self, string logicDataType,
      StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
      if (string.IsNullOrWhiteSpace(logicDataType))
      {
        return null;
      }

      return self.Where(logicVariable => logicVariable?.Type.Equals(logicDataType, comparison) ?? false);
    }

    public static bool Remove(this IList<LogicVariable> self, string variableName)
    {
      var variable = self.GetByName(variableName);
      if (variable == null)
      {
        return false;
      }

      return self.Remove(variable);
    }

    public static bool Contains(this IEnumerable<LogicVariable> self, string variableName,
      StringComparison comparison = StringComparison.Ordinal)
    {
      return self.GetByName(variableName, comparison) != null;
    }

    #endregion

  }
}