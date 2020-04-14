using Scada.Common.DynPropertyParser;
using System.Collections.Generic;
using System.Linq;

namespace zenonApi.MetaDescription.Parser.OdlWrapperClasses
{
  class ComDynPropTable
  {
    private List<ComDynProp> _dynPropList;
    public string description { get; }
    public string inheritance { get; }
    public IReadOnlyList<ComDynProp> DynProplist { get { return _dynPropList; } }

    public ComDynPropTable(IReadOnlyList<IDynPropertyEntry> entries)
    {

      _dynPropList = new List<ComDynProp>();
      foreach (var entry in entries)
      {
        _dynPropList.Add(new ComDynProp((ZenPropEntry)entry));
      }
      _dynPropList = _dynPropList.OrderBy(x => x.Externalname).ToList();

    }

  }
}
