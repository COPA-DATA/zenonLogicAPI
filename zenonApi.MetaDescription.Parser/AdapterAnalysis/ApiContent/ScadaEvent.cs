using System.Collections.Generic;

namespace zenonApi.MetaDescription.Parser.AdapterAnalysis.ApiContent
{
  public class ScadaEvent : IScadaEvent
  {
    private List<IScadaParameter> _parameters;
    private bool? _isIgnored;
    public IEnumerable<IScadaParameter> Parameters => _parameters;

    public string ViewName { get; }

    public string HostName { get; }

    public bool? IsIgnored => (_isIgnored == true) ? true : false;

    public ScadaEvent(object reflectedEvent)
    {
      this._parameters = new List<IScadaParameter>();

      this.HostName = reflectedEvent.GetType().GetProperty("HostName").GetValue(reflectedEvent, null) as string;
      if (reflectedEvent.GetType().GetProperty("IsIgnored") != null)
        this._isIgnored = reflectedEvent.GetType().GetProperty("IsIgnored").GetValue(reflectedEvent, null) as bool?;
      if (reflectedEvent.GetType().GetProperty("ViewName") != null && IsIgnored == false)
        this.ViewName = reflectedEvent.GetType().GetProperty("ViewName").GetValue(reflectedEvent, null) as string;
      var parameters = reflectedEvent.GetType().GetProperty("Parameters").GetValue(reflectedEvent, null) as IEnumerable<object>;

      foreach (var p in parameters)
      {
        _parameters.Add(new ScadaParameter(p));
      }
    }
  }
}
