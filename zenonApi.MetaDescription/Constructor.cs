using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Constructor : zenonSerializable<Constructor>
  {
    public override string NodeName => "Constructor";

    [zenonSerializableAttribute("Modifier")]
    public string Modifier { get; set; }

    [zenonSerializableNode("NO_NAME_FOR_LIST")]
    public List<Argument> Arguments { get; set; }
    [zenonSerializableNode("SourceCode")]
    public string SourceCode { get; set; }
     
    public Constructor()
    {
      Arguments = new List<Argument>();
    }
  }
}
