using System;

namespace zenonApi.Serialization
{
  /// <summary>
  /// Use this attribute to specify for each value of an enumeration how it shall be written in XML.
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  // ReSharper disable once InconsistentNaming : "zenon" is always written lowercase.
  public class zenonSerializableEnumAttribute : zenonSerializableBaseAttribute
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

    #region Internal base class overrides
    internal override zenonSerializableAttributeType AttributeType => zenonSerializableAttributeType.Enum;

    internal override byte InternalOrder => 0;

    internal override string InternalName => this.Name;

    internal override bool InternalOmitIfNull => false;

    internal override bool InternalEncapsulateChildsIfList => false;

    internal override Type InternalConverter => null; // Not supported for enum values

    internal override Type InternalTypeResolver => null; // Not supported for enum values
    #endregion
  }
}
