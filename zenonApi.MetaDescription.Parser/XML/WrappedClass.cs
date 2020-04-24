using System.Collections.Generic;
using Scada.Common.DynPropertyParser;
using zenonApi.MetaDescription.Parser.AdapterAnalysis;
using zenonApi.MetaDescription.Parser.OdlWrapperClasses;

namespace zenonApi.MetaDescription.Parser.XML
{
  public static class WrappedClass
  {
    public static Class Parse(IScadaInterface scadaInterface, string scadaNamespace)
    {
      Class classForXml = new Class(scadaNamespace + "." + scadaInterface.ViewName,scadaInterface.HostName);

      foreach (IScadaProperty scadaProperty in scadaInterface.Properties)
      {
        if (scadaProperty.IsIgnored != null && (bool) scadaProperty.IsIgnored)
        {
          continue;
        }

        classForXml.Properties.Add(WrappedProperty.Parse(scadaProperty));
      }

      return classForXml;
    }

    public static Class Parse(KeyValuePair<string, ComDynPropTable> dynPropTable, string scadaNamespace)
    {
      Class classForXml = new Class(scadaNamespace + "." + dynPropTable.Key,
        scadaNamespace + "." + dynPropTable.Key);

      classForXml.IsDynProperty = true;
      classForXml.HasDynProperties = true;

      foreach (ComDynProp dynPropertyEntry  in dynPropTable.Value.DynProplist)
      {
        classForXml.Properties.Add(WrappedProperty.Parse(dynPropertyEntry));
      }
      return classForXml;
    }
  }
}
