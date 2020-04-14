namespace zenonApi.MetaDescription.Parser.AdapterAnalysis.ApiContent
{
  public class ScadaEnumValue : IScadaEnumValue
  {
    public int Ordinal { get; }

    public string ViewName { get; }

    public string HostName { get; }

    public bool? IsIgnored { get; }

    public ScadaEnumValue(object reflectedEnumValue)
    {
      this.Ordinal = (int)reflectedEnumValue.GetType().GetProperty("Ordinal").GetValue(reflectedEnumValue, null);
      this.ViewName = reflectedEnumValue.GetType().GetProperty("ViewName").GetValue(reflectedEnumValue, null) as string;
      this.HostName = reflectedEnumValue.GetType().GetProperty("HostName").GetValue(reflectedEnumValue, null) as string;
    }
  }
}
