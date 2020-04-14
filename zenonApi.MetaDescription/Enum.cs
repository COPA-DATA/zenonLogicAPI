using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Enum : zenonSerializable<Enum>, IModuleItem
  {
    public override string NodeName => "enum";

    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }

    [zenonSerializableNode("NO_NAME_FOR_LIST")]
    public List<EnumValue> EnumValues { get; set; }

    public Enum()
    {
      EnumValues = new List<EnumValue>();
    }
  }
}