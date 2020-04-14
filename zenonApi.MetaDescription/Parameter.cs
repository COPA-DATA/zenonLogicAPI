using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Parameter : zenonSerializable<Parameter>, IModuleItem
  {
    public override string NodeName => "parameter";
    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }
  }
}
