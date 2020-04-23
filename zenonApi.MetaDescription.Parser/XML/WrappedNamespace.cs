using System.Collections.Generic;
using zenonApi.MetaDescription.Parser.AdapterAnalysis;
using zenonApi.MetaDescription.Parser.AdapterAnalysis.Structure;

namespace zenonApi.MetaDescription.Parser.XML
{
  public static class WrappedNamespace
  {
    public static void Parse(Definitions definitions, KeyValuePair<string, NamespaceStruct> namespaceKeyValuePair)
    {
      foreach (IScadaInterface scadaInterface in namespaceKeyValuePair.Value.ScadaNamespace.Interfaces)
      {
        if (scadaInterface.IsIgnored != null && (bool) scadaInterface.IsIgnored)
        {
          continue;
        }
        definitions.Classes.Add(WrappedClass.Parse(scadaInterface));
      }

      foreach (IScadaEnum scadaEnum in namespaceKeyValuePair.Value.ScadaNamespace.Enumerations)
      {
        if (scadaEnum.IsIgnored != null && (bool)scadaEnum.IsIgnored)
        {
          continue;
        }
        definitions.Enums.Add(WrappedEnum.Parse(scadaEnum));
      }
    }
  }
}
