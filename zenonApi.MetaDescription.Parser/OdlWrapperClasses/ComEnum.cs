using System;
using System.Collections.Generic;
using System.Linq;
using zenonApi.MetaDescription.Parser.AdapterAnalysis;
using G = Scada.Common.OdlParser;

namespace zenonApi.MetaDescription.Parser.OdlWrapperClasses
{
  class ComEnum
  {
    private Dictionary<string, Tuple<int, string>> _memberDictionary;
    public IReadOnlyDictionary<string, Tuple<int, string>> MemberDictionary { get { return _memberDictionary; } }
    public Availability Availability;

    public ComEnum(G.ComEnum comEnum)
    {
      Availability = new Availability(comEnum.Availablity);
      _memberDictionary = new Dictionary<string, Tuple<int, string>>();
      foreach (var pair in comEnum.MemberDictionary)
      {
        _memberDictionary.Add(pair.Key, pair.Value);
      }
      _memberDictionary.Values.OrderBy(pair => pair.Item1);
    }

    public ComEnum(IScadaEnum scadaEnum)
    {
      _memberDictionary = new Dictionary<string, Tuple<int, string>>();
      foreach (var pair in scadaEnum.Values)
      {
        _memberDictionary.Add(pair.HostName, new Tuple<int, string>(pair.Ordinal, pair.ViewName));
      }
      _memberDictionary.Values.OrderBy(pair => pair.Item1);
    }
  }
}
