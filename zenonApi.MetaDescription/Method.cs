using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Method :  zenonSerializable<Method>
  {
    public override string NodeName => "Method";

    [zenonSerializableAttribute("ViewName")]
    public string ViewName { get; set; }
    [zenonSerializableAttribute("HostName")]
    public string HostName { get; set; }
    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }
    [zenonSerializableAttribute("OverrideViewName")]
    public string OverrideViewName { get; set; }
    [zenonSerializableAttribute("ReturnValueType")]
    public string ReturnValueType { get; set; }
    [zenonSerializableNode("Argument")]
    public List<Argument> Parameters { get; set; }

    [zenonSerializableNode("SourceCode")]
    public string SourceCode { get; set; }




    public Method()
    {

    }
  }
}
