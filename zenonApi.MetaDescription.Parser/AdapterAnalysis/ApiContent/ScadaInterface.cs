using System.Collections.Generic;

namespace zenonApi.MetaDescription.Parser.AdapterAnalysis.ApiContent
{
  public class ScadaInterface : IScadaInterface
  {
    private List<IScadaMethod> _methods;
    private List<IScadaEvent> _events;
    private List<IScadaProperty> _properties;

    public IEnumerable<IScadaMethod> Methods => _methods;
    public IEnumerable<IScadaEvent> Events => _events;
    public IEnumerable<IScadaProperty> Properties => _properties;

    public string ViewName { get; }
    public string HostName { get; }
    public bool? IsIgnored { get; }

    public ScadaInterface(object reflectedScadaInterface)
    {
      _methods = new List<IScadaMethod>();
      _events = new List<IScadaEvent>();
      _properties = new List<IScadaProperty>();

      this.ViewName = reflectedScadaInterface.GetType().GetProperty("ViewName").GetValue(reflectedScadaInterface, null) as string;
      this.HostName = reflectedScadaInterface.GetType().GetProperty("HostName").GetValue(reflectedScadaInterface, null) as string;

      var methods = reflectedScadaInterface.GetType().GetProperty("Methods").GetValue(reflectedScadaInterface, null) as IEnumerable<object>;
      var events = reflectedScadaInterface.GetType().GetProperty("Events").GetValue(reflectedScadaInterface, null) as IEnumerable<object>;
      var properties = reflectedScadaInterface.GetType().GetProperty("Properties").GetValue(reflectedScadaInterface, null) as IEnumerable<object>;

      foreach (var m in methods)
      {
        _methods.Add(new ScadaMethod(m));
      }

      foreach (var e in events)
      {
        _events.Add(new ScadaEvent(e));
      }

      foreach (var p in properties)
      {
        _properties.Add(new ScadaProperty(p));
      }
    }
  }
}
