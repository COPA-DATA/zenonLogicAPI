using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Interface : zenonSerializable<Interface>
  {
    public override string NodeName => "Interface";

    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("OverrideViewName")]
    public string OverrideViewName { get; set; }

    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }

    public Interface()
    {

    }
  }
}
