using System;

namespace zenonApi.Serialization
{
  /// <summary>
  /// Set this attribute to properties of an <see cref="IZenonSerializable"/> to create an XML attribute.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class zenonSerializableAttributeAttribute : zenonSerializableBaseAttribute
  {
    /// <summary>
    /// Initializes a new instance of a <see cref="zenonSerializableAttributeAttribute"/>.
    /// The <paramref name="attributeName"/> is required to control how the attribute is named in the XML.
    /// </summary>
    /// <param name="attributeName">The name of the resulting XML attribute.</param>
    public zenonSerializableAttributeAttribute(string attributeName)
    {
      this.AttributeName = attributeName;
    }

    /// <summary>
    /// The name of the propertie's representation as an attribute in XML.
    /// </summary>
    public string AttributeName { get; private set; }

    /// <summary>
    /// Controls the serialization order of properties within an <see cref="IZenonSerializable"/>.
    /// </summary>
    public byte AttributeOrder { get; set; }

    /// <summary>
    /// Specifies if an attribute shall be omitted if it is null (default = true).
    /// </summary>
    public bool OmitIfNull { get; set; } = true;

    /// <summary>
    /// The type of an <see cref="IZenonSerializationConverter"/> to use for serialization and deserialization.
    /// </summary>
    private Type converter = null;

    /// <summary>
    /// The type of an <see cref="IZenonSerializationConverter"/> to use for serialization and deserialization.
    /// </summary>
    public Type Converter
    {
      get => converter;
      set
      {
        if (value != null && typeof(IZenonSerializationConverter).IsAssignableFrom(value))
        {
          converter = value;
        }
        else
        {
          throw new Exception($"Only a type implementing {typeof(IZenonSerializationConverter)} can be passed as a converter.");
        }
      }
    }

    #region Internal base class overrides
    internal override zenonSerializableAttributeType AttributeType => zenonSerializableAttributeType.Attribute;
    
    internal override byte InternalOrder => this.AttributeOrder;
    
    internal override string InternalName => this.AttributeName;

    internal override bool InternalOmitIfNull => this.OmitIfNull;

    internal override bool InternalEncapsulateChildsIfList => false;

    internal override Type InternalConverter => this.Converter;
    #endregion
  }
}
