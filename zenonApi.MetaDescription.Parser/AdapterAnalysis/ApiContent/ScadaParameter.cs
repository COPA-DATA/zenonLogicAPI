namespace zenonApi.MetaDescription.Parser.AdapterAnalysis.ApiContent
{
  class ScadaParameter : IScadaParameter
  {
    public string ViewName { get; }

    public string HostName { get; }

    public bool? IsIgnored { get; }

    public ScadaParameter(object reflectedParameter)
    {
      this.ViewName = reflectedParameter.GetType().GetProperty("ViewName").GetValue(reflectedParameter, null) as string;
      this.HostName = reflectedParameter.GetType().GetProperty("HostName").GetValue(reflectedParameter, null) as string;
    }

  }
}
