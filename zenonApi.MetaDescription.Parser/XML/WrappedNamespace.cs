using System.Collections.Generic;
using zenonApi.MetaDescription.Parser.AdapterAnalysis;
using zenonApi.MetaDescription.Parser.AdapterAnalysis.Structure;

namespace zenonApi.MetaDescription.Parser.XML
{
  public static class WrappedNamespace
  {
    public static Namespace Parse(KeyValuePair<string, NamespaceStruct> namespaceKeyValuePair)
    {
      Namespace namespaceForXml = new Namespace(namespaceKeyValuePair.Key);

      foreach (IScadaInterface scadaInterface in namespaceKeyValuePair.Value.ScadaNamespace.Interfaces)
      {
        if (scadaInterface.IsIgnored != null && (bool) scadaInterface.IsIgnored)
        {
          continue;
        }
        namespaceForXml.Classes.Add(WrappedClass.Parse(scadaInterface));
      }

      foreach (IScadaEnum scadaEnum in namespaceKeyValuePair.Value.ScadaNamespace.Enumerations)
      {
        if (scadaEnum.IsIgnored != null && (bool)scadaEnum.IsIgnored)
        {
          continue;
        }
        namespaceForXml.Enums.Add(WrappedEnum.Parse(scadaEnum));
      }

      return namespaceForXml;
    }
  }
}
