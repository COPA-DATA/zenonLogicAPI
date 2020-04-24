using Scada.Common.DynPropertyParser;
using System.Collections.Generic;
using System.Linq;

namespace zenonApi.MetaDescription.Parser.OdlWrapperClasses
{
  public class ComDynPropTable
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
        if (entry is ZenPropEntry)
        {
          _dynPropList.Add(new ComDynProp((ZenPropEntry)entry));
        }
        else
        {
          _dynPropList.Add(new ComDynProp((DrvPropEntry)entry));
        }
      }
      _dynPropList = _dynPropList.OrderBy(x => x.Externalname).ToList();

    }

    public void Add(ComDynPropTable dynPropTable)
    {
      _dynPropList.AddRange(dynPropTable._dynPropList);
    }

  }
}
