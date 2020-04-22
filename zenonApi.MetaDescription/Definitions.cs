using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Definitions : zenonSerializable<Definitions>
  {
    public override string NodeName => "Definitions";

    [zenonSerializableNode("Apartments", EncapsulateChildsIfList = true)]
    public List<Apartment> Apartments { get; set; }

    [zenonSerializableNode("Class")]
    public List<Class> Classes { get; set; }

    [zenonSerializableNode("Enum")]
    public List<Enum> Enums { get; set; }

    [zenonSerializableNode("Interface")]
    public List<Interface> Interfaces { get; set; }

    [zenonSerializableAttribute("TargetVersion")]
    public string TargetVersion { get; set; }

    public Definitions(string targetVersion)
    {
      TargetVersion = targetVersion;
      Apartments = new List<Apartment>();
      Classes = new List<Class>();
      Enums = new List<Enum>();
      Interfaces = new List<Interface>();

    }

    public Definitions()
    {
    }
  }
}
