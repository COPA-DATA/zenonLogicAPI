using System.Collections.Generic;

namespace zenonApi.MetaDescription.Parser.AdapterAnalysis.ApiContent
{
  public class ScadaMethod : IScadaMethod
  {
    private bool? _isIgnored;
    public List<IScadaParameter> Parameters { get; }
    public bool? IsIgnored => (_isIgnored == true) ? true : false;

    public string ViewName { get; }

    public string HostName { get; }

    public ScadaMethod(object reflectedMethod)
    {
      this.Parameters = new List<IScadaParameter>();
      this.HostName = reflectedMethod.GetType().GetProperty("HostName").GetValue(reflectedMethod, null) as string;

      if (reflectedMethod.GetType().GetProperty("IsIgnored") != null)
        this._isIgnored = reflectedMethod.GetType().GetProperty("IsIgnored").GetValue(reflectedMethod, null) as bool?;

      if (reflectedMethod.GetType().GetProperty("ViewName") != null && IsIgnored == false)
        this.ViewName = reflectedMethod.GetType().GetProperty("ViewName").GetValue(reflectedMethod, null) as string;

      var parameters = reflectedMethod.GetType().GetProperty("Parameters").GetValue(reflectedMethod, null) as IEnumerable<object>;

      foreach (var p in parameters)
      {
        Parameters.Add(new ScadaParameter(p));
      }
    }
  }
}
