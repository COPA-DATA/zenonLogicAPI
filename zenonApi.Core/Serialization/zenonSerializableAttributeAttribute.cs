using System;

namespace zenonApi.Serialization
{
  [AttributeUsage(AttributeTargets.Property)]
  public class zenonSerializableAttributeAttribute : Attribute
  {
    public zenonSerializableAttributeAttribute(string attributeName)
    {
      this.AttributeName = attributeName;
    }

    public string AttributeName { get; private set; }
    public byte AttributeOrder { get; set; }
    public bool OmitIfNull { get; set; }

    private Type converter = null;
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
  }
}
