using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Property: zenonSerializable<Property>, IModuleItem
  {
    public override string NodeName => "property";
    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }
    [zenonSerializableAttribute("Type")]
    public string Type { get; set; }
    [zenonSerializableAttribute("IsDynProperty")]
    public bool IsDynProperty { get; set; }
    [zenonSerializableAttribute("IsMethodInHost")]
    public bool IsMethodInHost { get; set; }
  }
}
