using System.Collections.Generic;
using zenonApi.MetaDescription.Parser.AdapterAnalysis.Structure;

namespace zenonApi.MetaDescription.Parser.XML
{
  public static class WrappedNamespaces
  {
    public static Definitions Parse(IDictionary<string,NamespaceStruct> namespaceStructs)
    {
      Definitions definitions = new Definitions("8200");

      foreach(KeyValuePair<string,NamespaceStruct> namespaceStructKeyValuePair in namespaceStructs)
      {
        WrappedNamespace.Parse(definitions, namespaceStructKeyValuePair);
      }
      return definitions;
    }
  }
}
