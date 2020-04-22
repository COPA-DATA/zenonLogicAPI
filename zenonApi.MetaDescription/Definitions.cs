using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Definitions : zenonSerializable<Definitions>
  {
    public override string NodeName => "Definitions";

    [zenonSerializableNode("Apartments", EncapsulateChildsIfList = true)]
    public List<Apartment> Apartments { get; set; }

    [zenonSerializableNode("NO_NAME_FOR_LIST")]
    public List<Class> Classes { get; set; }

    [zenonSerializableNode("NO_NAME_FOR_LIST")]
    public List<Enum> Enums { get; set; }

    [zenonSerializableNode("NO_NAME_FOR_LIST")]
    public List<Method> Interfaces { get; set; }

    [zenonSerializableAttribute("Path")]
    public string TargetVersion { get; set; }

    public Definitions(string targetVersion)
    {
      TargetVersion = targetVersion;
      Apartments = new List<Apartment>();
      Classes = new List<Class>();
      Enums = new List<Enum>();
      Interfaces = new List<Method>();

    }
  }
}
