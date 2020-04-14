using zenonApi.MetaDescription.Parser.AdapterAnalysis;

namespace zenonApi.MetaDescription.Parser.XML
{
  public static class WrappedEnum
  {
    public static Enum Parse(IScadaEnum scadaEnum)
    {
      Enum enumForXml = new Enum();

      enumForXml.HostName = scadaEnum.HostName;
      enumForXml.ViewName = scadaEnum.ViewName;

      foreach (IScadaEnumValue scadaEnumValue in scadaEnum.Values)
      {
        if (scadaEnumValue.IsIgnored != null && (bool) scadaEnumValue.IsIgnored)
        {
          continue;
        }

        enumForXml.EnumValues.Add(WrappedEnumValue.Parse(scadaEnumValue));
      }

      return enumForXml;
    }
  }
}
