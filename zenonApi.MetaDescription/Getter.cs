using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Getter : zenonSerializable<Getter>
  {
    public override string NodeName => "Getter";

    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }

    [zenonSerializableNode("SourceCode")]
    public string SourceCode { get; set; }

    public Getter()
    {

    }
    
  }
}
