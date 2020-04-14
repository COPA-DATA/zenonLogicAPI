using System.Collections;
using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Namespaces : zenonSerializable<Namespaces>, IEnumerable<Namespace>
  {
    public override string NodeName => "namespaces";

    [zenonSerializableNode("NO_NODE_NAME_FOR_LIST")]
    private List<Namespace> _namespaces { get; set; }

    [zenonSerializableAttribute("Path")]
    public string Path { get; set; }

    public Namespaces(string newNamespace)
    {
      Path = newNamespace;
      _namespaces = new List<Namespace>();
    }

    public IEnumerator<Namespace> GetEnumerator()
    {
      foreach (Namespace _namespace in _namespaces)
      {
        yield return _namespace;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Add(Namespace _namespace)
    {
      _namespaces.Add(_namespace);
    }
  }
}
