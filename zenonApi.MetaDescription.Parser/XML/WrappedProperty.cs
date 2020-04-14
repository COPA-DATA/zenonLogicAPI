using zenonApi.MetaDescription.Parser.AdapterAnalysis;

namespace zenonApi.MetaDescription.Parser.XML
{
  public static class WrappedProperty
  {
    public static Property Parse(IScadaProperty scadaProperty)
    {
      Property propertyForXml = new Property();

      propertyForXml.HostName = scadaProperty.HostName;
      propertyForXml.ViewName = scadaProperty.ViewName;
      propertyForXml.IsMethodInHost = scadaProperty.IsMethodInHost;

      return propertyForXml;
    }
  }
}
