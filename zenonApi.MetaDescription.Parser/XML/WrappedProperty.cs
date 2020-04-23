using zenonApi.MetaDescription.Parser.AdapterAnalysis;

namespace zenonApi.MetaDescription.Parser.XML
{
  public static class WrappedProperty
  {
    public static Property Parse(IScadaProperty scadaProperty)
    {
      Property propertyForXml = new Property(scadaProperty.ViewName,scadaProperty.HostName);
      propertyForXml.IsMethodInHost = scadaProperty.IsMethodInHost;

      return propertyForXml;
    }
  }
}
