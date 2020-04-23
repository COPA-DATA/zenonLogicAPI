using zenonApi.MetaDescription.Parser.AdapterAnalysis;

namespace zenonApi.MetaDescription.Parser.XML
{
  public static class WrappedClass
  {
    public static Class Parse(IScadaInterface scadaInterface)
    {
      Class classForXml = new Class(scadaInterface.ViewName,scadaInterface.HostName);

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
  }
}
