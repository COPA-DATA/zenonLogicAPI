using System.Collections.Generic;

namespace zenonApi.MetaDescription.Parser.AdapterAnalysis.ApiContent
{
  public class ScadaNamespace : IScadaNamespace
  {

    private List<IScadaInterface> _interfaces;
    private List<IScadaEnum> _enums;

    public string Name { get; }

    public IEnumerable<IScadaInterface> Interfaces => _interfaces;

    public IEnumerable<IScadaEnum> Enumerations => _enums;

    public ScadaNamespace(object reflectedNamespace)
    {
      this.Name = reflectedNamespace.GetType().GetProperty("Name").GetValue(reflectedNamespace, null) as string;
      this._interfaces = new List<IScadaInterface>();
      this._enums = new List<IScadaEnum>();

      var enums = reflectedNamespace.GetType().GetProperty("Enumerations").GetValue(reflectedNamespace, null) as IEnumerable<object>;
      var interfaces = reflectedNamespace.GetType().GetProperty("Interfaces").GetValue(reflectedNamespace, null) as IEnumerable<object>;

      foreach (var e in enums)
      {
        _enums.Add(new ScadaEnum(e));
      }
      foreach (var i in interfaces)
      {
        _interfaces.Add(new ScadaInterface(i));
      }
    }
  }
}
