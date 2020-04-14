namespace zenonApi.MetaDescription.Parser.AdapterAnalysis
{
  public interface ITranslatable
  {
    string ViewName { get; }
    string HostName { get; }

    /// <summary>
    /// Is true if the Method is Ignored and therefor not available in the Addin-Framework
    /// </summary>
    bool? IsIgnored { get; }
  }
}
