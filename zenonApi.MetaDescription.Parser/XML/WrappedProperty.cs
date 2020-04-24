using zenonApi.MetaDescription.Parser.AdapterAnalysis;
using zenonApi.MetaDescription.Parser.OdlWrapperClasses;

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

    public static Property Parse(ComDynProp dynProperty)
    {
      Property propertyForXml = new Property(dynProperty.DisplayName, dynProperty.Id);
      propertyForXml.IsDynProperty = true;
      propertyForXml.DefinitionType = dynProperty.Datatype;
      propertyForXml.XmlName = dynProperty.Externalname;
      return propertyForXml;
    }
  }
}
