using System;

namespace zenonApi.Serialization
{
  /// <summary>
  /// Use this attribute to specify for each value of an enumeration how it shall be written in XML.
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public class zenonSerializableEnumAttribute : Attribute
  {
    /// <summary>
    /// Creates a new instance of a <see cref="zenonSerializableEnumAttribute"/>.
    /// The <paramref name="name"/> is required to control how the value shall be displayed in the XML representation.
    /// </summary>
    /// <param name="name">Specifies how the enumeration value is written in the resulting XML.</param>
    public zenonSerializableEnumAttribute(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new Exception("No node name is given.");
      }

      Name = name;
    }

    /// <summary>
    /// Specifies how the enumeration value is written in the resulting XML.
    /// </summary>
    public string Name { get; protected set; }
  }
}
