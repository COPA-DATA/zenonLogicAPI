using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Class : zenonSerializable<Class>, IModuleItem
  {
    public override string NodeName => "class";
    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }
    [zenonSerializableAttribute("Extends")]
    public string Extends { get; set; }
    [zenonSerializableAttribute("IsDynProperty")]
    public bool IsDynProperty { get; set; }
    [zenonSerializableAttribute("HasDynProperties")]
    public bool HasDynProperties { get; set; }

    [zenonSerializableNode("constructor")]
    public Constructor Constructor { get; set; }

    [zenonSerializableNode("NO_NAME_FOR_LIST")]
    public List<Property> Properties { get; set; }

    public Class()
    {
      Constructor = new Constructor();
      Properties = new List<Property>();
    }
  }
}
