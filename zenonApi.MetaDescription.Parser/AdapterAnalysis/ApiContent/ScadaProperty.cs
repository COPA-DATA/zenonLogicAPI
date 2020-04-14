namespace zenonApi.MetaDescription.Parser.AdapterAnalysis.ApiContent
{
  public class ScadaProperty : IScadaProperty
  {
    private bool? _isIgnored;
    public bool IsMethodInHost { get; }

    public string ViewName { get; }

    public string HostName { get; }

    public bool? IsIgnored => (_isIgnored == true) ? true : false;

    public ScadaProperty(object reflectedProperty)
    {
      this.IsMethodInHost = (bool)reflectedProperty.GetType().GetProperty("IsMethodInHost").GetValue(reflectedProperty, null);
      if (reflectedProperty.GetType().GetProperty("IsIgnored") != null)
        this._isIgnored = reflectedProperty.GetType().GetProperty("IsIgnored").GetValue(reflectedProperty, null) as bool?;
      if (reflectedProperty.GetType().GetProperty("ViewName") != null && IsIgnored == false)
        this.ViewName = reflectedProperty.GetType().GetProperty("ViewName").GetValue(reflectedProperty, null) as string;
      this.HostName = reflectedProperty.GetType().GetProperty("HostName").GetValue(reflectedProperty, null) as string;
    }
  }
}
