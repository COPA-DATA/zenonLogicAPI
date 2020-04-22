using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Enum : zenonSerializable<Enum>
  {
    public override string NodeName => "Enum";

    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("XmlBane")]
    public string XmlName { get; set; }
    [zenonSerializableAttribute("MinimumVersion")]
    public string MinimumVersion { get; set; }
    [zenonSerializableAttribute("OverrideViewName")]
    public string OverrideViewName { get; set; }
    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }

    [zenonSerializableNode("NO_NAME_FOR_LIST")]
    public List<EnumValue> EnumValues { get; set; }

    public Enum(string  viewName, string hostName)
    {
      ViewName = viewName;
      HostName = hostName;
      XmlName = HostName;
      EnumValues = new List<EnumValue>();
    }
  }
}