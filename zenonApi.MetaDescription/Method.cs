using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Method :  zenonSerializable<Method>, IModuleItem
  {
    public override string NodeName => "Method";
    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }

    [zenonSerializableNode("NO_NAME_FOR_LIST")]
    public List<Parameter> Parameters { get; set; } 
  }
}
