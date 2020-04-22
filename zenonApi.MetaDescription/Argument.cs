using System;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Argument : zenonSerializable<Argument>
  {
    public override string NodeName => "Argument";

    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("OverrideViewName")]
    public string OverrideViewName { get; set; }
    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }
    [zenonSerializableAttribute("ArgumentType")]
    public Type ArgumentType { get; set; }


    public Argument(string viewName, string hostName)
    {
      ViewName = viewName;
      HostName = hostName;
    }
  }
}
