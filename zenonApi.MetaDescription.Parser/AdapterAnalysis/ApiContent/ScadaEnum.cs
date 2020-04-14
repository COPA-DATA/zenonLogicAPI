using System.Collections.Generic;

namespace zenonApi.MetaDescription.Parser.AdapterAnalysis.ApiContent
{
  public class ScadaEnum : IScadaEnum
  {

    private List<IScadaEnumValue> _values;
    private bool? _isIgnored;
    public List<IScadaEnumValue> Values => _values;

    public string ViewName { get; }

    public string HostName { get; }

    public bool? IsIgnored => (_isIgnored == true) ? true : false;

    public ScadaEnum(object reflectedEnum)
    {
      this.ViewName = reflectedEnum.GetType().GetProperty("ViewName").GetValue(reflectedEnum, null) as string;
      this.HostName = reflectedEnum.GetType().GetProperty("HostName").GetValue(reflectedEnum, null) as string;
      if (reflectedEnum.GetType().GetProperty("IsIgnored") != null)
        this._isIgnored = reflectedEnum.GetType().GetProperty("IsIgnored").GetValue(reflectedEnum, null) as bool?;

      _values = new List<IScadaEnumValue>();

      var values = reflectedEnum.GetType().GetProperty("Values").GetValue(reflectedEnum, null) as IEnumerable<object>;

      foreach (var v in values)
      {
        _values.Add(new ScadaEnumValue(v));
      }
    }
  }
}
