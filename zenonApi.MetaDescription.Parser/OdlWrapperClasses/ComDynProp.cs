using Scada.Common.DynPropertyParser;

namespace zenonApi.MetaDescription.Parser.OdlWrapperClasses
{
  public class ComDynProp
  {
    public string Datatype { get; }
    public string DisplayName { get; }
    public string Externalname { get; }
    public string Id { get; }
    public string ReturnType { get; }
    public string PointerZtKnoten { get; }


    public ComDynProp(ZenPropEntry entry)
    {
      Datatype = entry.DataType;
      DisplayName = entry.DisplayText[Language.English];
      Externalname = entry.ExternalName;
      Id = entry.PropertyId;
      ReturnType = entry.ReturnTypeString;
      if (Datatype == "ZT_KNOTEN")
      {
        if (entry.ReturnTypeReference != null)
        {
          PointerZtKnoten = entry.ReturnTypeString;
        }
        else
        {
          // No ReturntypeReference
        }
      }
    }

    public ComDynProp(DrvPropEntry entry)
    {
      Datatype = entry.ReturnTypeString;
      DisplayName = entry.ExternalName ;
      Externalname = entry.ExternalName;
      Id = entry.PropertyId;
      ReturnType = entry.ReturnTypeString;
    }
  }
}
