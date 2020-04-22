using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Class : zenonSerializable<Class>
  {
    #region Class Attributes
    public override string NodeName => "Class";

    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("XmlName")]
    public string XmlName { get; set; }
    [zenonSerializableAttribute("OverrideViewName")]
    public string OverrideViewName { get; set; }
    [zenonSerializableAttribute("Apartment")]
    public int Apartment { get; set; }
    [zenonSerializableAttribute("Order")]
    public int Order { get; set; }
    [zenonSerializableAttribute("MinimumVersion")]
    public int MinimumVersion { get; set; }
    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }
    [zenonSerializableAttribute("IsDynProperty")]
    public bool IsDynProperty { get; set; }
    [zenonSerializableAttribute("HasDynProperties")]
    public bool HasDynProperties { get; set; }
    [zenonSerializableAttribute("Modifier")]
    public string Modifier { get; set; }
    [zenonSerializableAttribute("Extends")]
    public string Extends { get; set; }
    [zenonSerializableAttribute("Implements")]
    public string Implements { get; set; }
    #endregion

    #region Class nodes
    [zenonSerializableNode("Constructor")]
    public Constructor Constructor { get; set; }
    [zenonSerializableNode("Property")]
    public List<Property> Properties { get; set; }
    [zenonSerializableNode("Method")]
    public List<Method> Methods { get; set; }
    [zenonSerializableNode("Field")]
    public List<Field> Fields { get; set; }
    #endregion

    public Class(string viewName, string hostName)
    {
      ViewName = viewName;
      HostName = hostName;
      XmlName = ViewName;
      Constructor = new Constructor();
      Properties = new List<Property>();
      Methods = new List<Method>();
      Fields = new List<Field>();
    }

    public Class()
    {

    }
  }
}
