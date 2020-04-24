using System.Collections.Generic;
using zenonApi.MetaDescription.Parser.AdapterAnalysis.Structure;

namespace zenonApi.MetaDescription.Parser.XML
{
  public static class WrappedNamespaces
  {
    internal static Definitions Parse(IDictionary<string,NamespaceStruct> namespaceStructs, OdlWrapperClasses.ZenonCom _zenonCom)
    {
      Definitions definitions = new Definitions("8200");

      foreach(KeyValuePair<string,NamespaceStruct> namespaceStructKeyValuePair in namespaceStructs)
      {
        WrappedNamespace.Parse(definitions, namespaceStructKeyValuePair, _zenonCom);
      }
      return definitions;
    }
  }
}
