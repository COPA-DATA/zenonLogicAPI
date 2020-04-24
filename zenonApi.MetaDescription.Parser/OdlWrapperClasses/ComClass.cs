using Scada.Common.DynPropertyParser;
using System.Collections.Generic;
using System.Linq;
using G = Scada.Common.OdlParser;

namespace zenonApi.MetaDescription.Parser.OdlWrapperClasses
{
  class ComClass
  {
    private SortedDictionary<string, ComMember> _dicComEvents;
    private SortedDictionary<string, ComMember> _dicComMethods;
    private SortedDictionary<string, ComMember> _dicComProperties;
    private SortedDictionary<string, ComDynPropTable> _dicComDynProperties;


    public IReadOnlyDictionary<string, ComMember> DicComEvents { get { return _dicComEvents; } }
    public IReadOnlyDictionary<string, ComMember> DicComMethods { get { return _dicComMethods; } }
    public IReadOnlyDictionary<string, ComMember> DicComProperties { get { return _dicComProperties; } }
    public IReadOnlyDictionary<string, ComDynPropTable> DicComDynProperties { get { return _dicComDynProperties; } }

    public IDynPropertyMap DynPropertyMap { get; private set; }

    public Availability Availability { get; }


    public ComClass(string comClassName, G.ComClass _comClass, Dictionary<string, IDynPropertyMap> DynPropDic)
    {
      Availability = new Availability(_comClass.Availablity);
      _dicComEvents = new SortedDictionary<string, ComMember>();
      _dicComMethods = new SortedDictionary<string, ComMember>();
      _dicComProperties = new SortedDictionary<string, ComMember>();
      _dicComDynProperties = new SortedDictionary<string, ComDynPropTable>();


      if (_comClass.DicComEvents.Any())
      {
        foreach (var e in _comClass.DicComEvents)
        {
          _dicComEvents.Add(e.Key, new ComMember(e.Value));
        }
      }
      if (_comClass.DicComMethods.Any())
      {
        foreach (var m in _comClass.DicComMethods)
        {
          _dicComMethods.Add(m.Key, new ComMember(m.Value));
        }
      }
      if (_comClass.DicComProperties.Any())
      {
        foreach (var p in _comClass.DicComProperties)
        {
          _dicComProperties.Add(p.Key, new ComMember(p.Value));
        }
      }
      foreach (var c in DynPropDic)
      {
        if (c.Key == comClassName)
        {

          foreach (var dynPropertyMapKvp in c.Value.ChildMaps)
          {

            if (!(dynPropertyMapKvp.Value is DrvPropMap))
            {
              _dicComDynProperties.Add(dynPropertyMapKvp.Key, new ComDynPropTable(dynPropertyMapKvp.Value.Entries));
            }
            else
            {
              foreach (DrvPropGroupEntry drvPropMap in dynPropertyMapKvp.Value.Entries)
              {
                if (_dicComDynProperties.ContainsKey(drvPropMap.ExternalName))
                {
                  _dicComDynProperties[drvPropMap.ExternalName].Add(new ComDynPropTable(drvPropMap.ChildEntries));
                }
                else
                {
                  _dicComDynProperties.Add(drvPropMap.ExternalName, new ComDynPropTable(drvPropMap.ChildEntries));
                }
              }
            }
          }
          DynPropertyMap = c.Value;
        }
      }
    }
  }
}
