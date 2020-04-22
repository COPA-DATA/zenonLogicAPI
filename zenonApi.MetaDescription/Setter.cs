using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Setter : zenonSerializable<Setter>
  {
    public override string NodeName => "Setter";

    [zenonSerializableAttribute("Hide")]
    public bool Hide { get; set; }

    [zenonSerializableNode("SourceCode")]
    public string SourceCode { get; set; }

    public Setter()
    {

    }
  }
}
