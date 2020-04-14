using System;
using System.Collections.Generic;

namespace zenonApi.MetaDescription.Parser.AdapterAnalysis.Structure
{
  public class NamespaceStruct
  {
    private string _name;
    public SortedDictionary<string, TypeStruct> TypeDict { get; }
    public IScadaNamespace ScadaNamespace;

    public NamespaceStruct(string name, Type[] types)
    {
      _name = name;
      TypeDict = new SortedDictionary<string, TypeStruct>();
      foreach (var type in types)
      {
        if (type != null)
        {
          if (type.Namespace == name)
          {
            TypeDict.Add(type.Name, new TypeStruct(type));
          }
        }
      }
    }
  }
}
