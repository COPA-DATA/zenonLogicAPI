using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class EnumValue : zenonSerializable<EnumValue>, IModuleItem
  {
    public override string NodeName => "enumvalue";

    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }

    [zenonSerializableNodeContent]
    public object Value { get; set; }
  }
}
