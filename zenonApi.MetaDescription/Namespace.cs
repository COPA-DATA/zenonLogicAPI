using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  /// <summary>
  /// A Module symbols a namespace within the Addin Framework
  /// </summary>
  public class Namespace : zenonSerializable<Namespace>
  {
    public override string NodeName => "namespace";

    [zenonSerializableAttribute("Path")]
    public string Path { get; set; }

    [zenonSerializableNode("NO_NAME_FOR_LIST")]
    public List<Class> Classes { get; set; }

    [zenonSerializableNode("NO_NAME_FOR_LIST")]
    public List<Enum> Enums { get; set; }

    public Namespace(string namespaceName)
    {
      Path = namespaceName;
      Classes = new List<Class>();
      Enums = new List<Enum>();
    }
  }
}
