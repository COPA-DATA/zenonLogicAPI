using System;

namespace zenonApi.Serialization
{
  /// <summary>
  /// Set this attribute on properties contained in an <see cref="IZenonSerializable"/> to set the current node's
  /// value to the property value.
  /// Its usage is permitted only within an <see cref="IZenonSerializable"/> object.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class zenonSerializableNodeContentAttribute : zenonSerializableBaseAttribute
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="zenonSerializableNodeContentAttribute"/>.
    /// </summary>
    public zenonSerializableNodeContentAttribute() { }

    /// <summary>
    /// The type of an <see cref="IZenonSerializationConverter"/> to use for serialization and deserialization.
    /// </summary>
    public Type Converter { get; set; }

    #region Internal base class overrides
    internal override zenonSerializableAttributeType AttributeType => zenonSerializableAttributeType.NodeContent;

    internal override byte InternalOrder => 0;

    internal override string InternalName => null;

    internal override bool InternalOmitIfNull => true;

    internal override Type InternalConverter => this.Converter;
    #endregion
  }
}
