using System;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Property: zenonSerializable<Property>
  {
    public override string NodeName => "Property";

    #region Property attributes
    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("XmlName")]
    public string XmlName { get; set; }
    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }
    [zenonSerializableAttribute("PropertyType")]
    public string PropertyType { get; set; }
    [zenonSerializableAttribute("DefinitionType")]
    public string DefinitionType { get; set; } 
    [zenonSerializableAttribute("IsDynProperty")]
    public bool IsDynProperty { get; set; }
    [zenonSerializableAttribute("IsMethodInHost")]
    public bool IsMethodInHost { get; set; }
    [zenonSerializableAttribute("BackingField")]
    public bool BackingField { get; set; }
    [zenonSerializableAttribute("BackingFieldName")]
    public string BackingFieldName { get; set; }
    [zenonSerializableAttribute("ConverterType")]
    public string ConverterType { get; set; }
    #endregion

    #region Property nodes
    [zenonSerializableNode("Setter")]
    public Setter PropertySetter { get; set; }
    [zenonSerializableNode("Getter")]
    public Getter PropertyGetter { get; set; }
    #endregion

    public Property(string viewName, string hostName)
    {
      ViewName = viewName;
      HostName = hostName;
      XmlName = ViewName;
    }

    public Property()
    {

    }
  }
}
