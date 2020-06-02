using System;

namespace zenonApi.Serialization
{
  /// <summary>
  /// Set this attribute on properties contained in an <see cref="IZenonSerializable"/> to set the current node's
  /// value to the property value.
  /// Its usage is permitted only within an <see cref="IZenonSerializable"/> object.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  // ReSharper disable once InconsistentNaming : "zenon" is always written lowercase.
  public class zenonSerializableNodeContentAttribute : zenonSerializableBaseAttribute
  {
    /// <summary>
    /// The type of an <see cref="IZenonSerializationConverter"/> to use for serialization and deserialization.
    /// </summary>
    public Type Converter { get; set; }

    #region Internal base class overrides
    internal override zenonSerializableAttributeType AttributeType => zenonSerializableAttributeType.NodeContent;

    internal override byte InternalOrder => 0;

    internal override string InternalName => null;

    internal override bool InternalOmitIfNull => true;

    internal override bool InternalEncapsulateChildsIfList => false;

    internal override Type InternalConverter => this.Converter;

    internal override Type InternalTypeResolver => null; // TODO: Check if this would make sense for node-contents
    #endregion
  }
}
