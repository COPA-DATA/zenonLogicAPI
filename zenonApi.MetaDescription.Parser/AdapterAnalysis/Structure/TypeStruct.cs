using System;

namespace zenonApi.MetaDescription.Parser.AdapterAnalysis.Structure
{
  public class TypeStruct
  {
    public string TypeName;
    public Type type;

    public TypeStruct(Type _type)
    {
      TypeName = _type.Name;
      type = _type;
    }
  }
}
