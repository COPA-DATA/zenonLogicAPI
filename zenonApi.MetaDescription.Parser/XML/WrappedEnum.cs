using zenonApi.MetaDescription.Parser.AdapterAnalysis;

namespace zenonApi.MetaDescription.Parser.XML
{
  public static class WrappedEnum
  {
    public static Enum Parse(IScadaEnum scadaEnum)
    {
      Enum enumForXml = new Enum(scadaEnum.ViewName, scadaEnum.HostName);

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
