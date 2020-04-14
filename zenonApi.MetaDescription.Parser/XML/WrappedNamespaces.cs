using System.Collections.Generic;
using zenonApi.MetaDescription.Parser.AdapterAnalysis.Structure;

namespace zenonApi.MetaDescription.Parser.XML
{
  public static class WrappedNamespaces
  {
    public static Namespaces Parse(IDictionary<string,NamespaceStruct> namespaceStructs)
    {
      Namespaces namespaces = new Namespaces("zenon.XML.API");

      foreach(KeyValuePair<string,NamespaceStruct> namespaceStructKeyValuePair in namespaceStructs)
      {
        namespaces.Add(WrappedNamespace.Parse(namespaceStructKeyValuePair));
      }
      return namespaces;
    }
  }
}
