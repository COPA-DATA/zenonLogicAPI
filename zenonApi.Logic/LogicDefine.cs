using System;
using System.Diagnostics;
using zenonApi.Logic.Resources;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  [DebuggerDisplay("Name = {Name}")]
  public class LogicDefine : zenonSerializable<LogicDefine>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "defines";
    #endregion

    internal LogicDefine() { }

    public LogicDefine(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new ArgumentException(Strings.LogicDefinesConstructorArgumentException);
      }

      Name = name; // TODO: Check for correct naming? (StringExtensions.IsValidZenonLogicName)
    }

    /// <summary>
    /// Name of the group. Can be a program name or:
    /// (COMMON) : definitions common to all projects
    /// (GLOBAL) : global definitions of the project
    /// </summary>
    [zenonSerializableAttribute("name", AttributeOrder = 0)]
    public string Name { get; protected set; }

    /// <summary>
    /// Formated text (extended ST format using #define pragmas)
    /// </summary>
    [zenonSerializableNodeContent]
    public string DefineContent { get; set; }
  }
}
