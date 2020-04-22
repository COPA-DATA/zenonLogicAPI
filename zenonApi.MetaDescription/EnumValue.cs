using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class EnumValue : zenonSerializable<EnumValue>
  {
    public override string NodeName => "EnumValue";

    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("XmlName")]
    public string XmlName { get; set; }
    [zenonSerializableAttribute("OverrideViewName")]
    public string OverrideViewName { get; set; }
    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }

    [zenonSerializableNodeContent]
    public int Value { get; set; }


    public EnumValue(string viewName, string hostName)
    {
      ViewName = viewName;
      HostName = hostName;
      XmlName = HostName;
    }

    public EnumValue()
    {

    }
  }
}
