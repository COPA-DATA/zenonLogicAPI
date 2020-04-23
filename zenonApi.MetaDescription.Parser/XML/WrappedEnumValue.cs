using zenonApi.MetaDescription.Parser.AdapterAnalysis;

namespace zenonApi.MetaDescription.Parser.XML
{
  public class WrappedEnumValue
  {
    public static EnumValue Parse(IScadaEnumValue scadaEnumValue)
    {
      EnumValue enumValueForXml = new EnumValue(scadaEnumValue.ViewName, scadaEnumValue.HostName);
      enumValueForXml.Value = scadaEnumValue.Ordinal;
      return enumValueForXml;
    }
  }
}
