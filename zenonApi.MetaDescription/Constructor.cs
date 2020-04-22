using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Constructor : zenonSerializable<Constructor>
  {
    public override string NodeName => "Constructor";

    [zenonSerializableAttribute("Modifier")]
    public string Modifier { get; set; }

    [zenonSerializableNode("Argument")]
    public List<Argument> Argument { get; set; }

    [zenonSerializableNode("SourceCode")]
    public string SourceCode { get; set; }
     
    public Constructor()
    {
      Argument = new List<Argument>();
    }
  }
}
