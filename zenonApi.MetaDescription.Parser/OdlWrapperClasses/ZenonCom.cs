using Scada.Common.DynPropertyParser;
using System.Collections.Generic;
using System.Linq;
using zenonApi.MetaDescription.Parser.AdapterAnalysis;
using G = Scada.Common.OdlParser;

namespace zenonApi.MetaDescription.Parser.OdlWrapperClasses
{
  class ZenonCom
  {
    private SortedDictionary<string, ComEnum> _enumDictionary;
    private SortedDictionary<string, ComClass> _classDictionary;

    public IReadOnlyDictionary<string, ComEnum> EnumDictionary { get { return _enumDictionary; } }
    public IReadOnlyDictionary<string, ComClass> ClassDictionary { get { return _classDictionary; } }

    public ZenonCom(G.ZenonCom ZenonComWithoutDyn, Dictionary<string, IDynPropertyMap> DynPropDic, IOrderedEnumerable<IScadaNamespace> OrderedNamespaces = null)
    {
      _classDictionary = new SortedDictionary<string, ComClass>();
      _enumDictionary = new SortedDictionary<string, ComEnum>();

      foreach (var comClassPair in ZenonComWithoutDyn.ClassDictionary)
      {
        _classDictionary.Add(comClassPair.Key, new ComClass(comClassPair.Key, comClassPair.Value, DynPropDic));
      }
      foreach (var comEnumPair in ZenonComWithoutDyn.EnumDictionary)
      {
        _enumDictionary.Add(comEnumPair.Key, new ComEnum(comEnumPair.Value));
      }
      if (OrderedNamespaces != null)
      {
        foreach (var nspace in OrderedNamespaces)
        {
          foreach (var e in nspace.Enumerations)
          {
            // Some wierd things going on tp´s from Geralds ODL parser got a space infront of the key so i need to inlucde the space in the containskey search
            if (!_enumDictionary.ContainsKey($" {e.HostName}"))
            {
              _enumDictionary.Add(e.HostName, new ComEnum(e));
            }
          }
        }
      }
    }
  }
}
