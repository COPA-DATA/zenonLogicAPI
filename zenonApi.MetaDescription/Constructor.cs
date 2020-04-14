using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Constructor : zenonSerializable<Constructor>, IModuleItem
  {
    public override string NodeName => "constructor";

    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }

    [zenonSerializableNode("NO_NAME_FOR_LIST")]
    public List<Parameter> Parameter { get; set; }

    [zenonSerializableNode("Override")]
    public string ConstructorOverride { get; set; }

    public Constructor()
    {
      Parameter = new List<Parameter>();
    }
  }
}
