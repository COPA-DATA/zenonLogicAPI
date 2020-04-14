using Scada.Common.DynPropertyParser;

namespace zenonApi.MetaDescription.Parser.OdlWrapperClasses
{
  class ComDynProp
  {
    public string Datatype { get; }
    public string DisplayName { get; }
    public string Externalname { get; }
    public string Id { get; }
    public string PointerZtKnoten { get; }


    public ComDynProp(ZenPropEntry entry)
    {
      //Datatype = entry.DataType;
      //DisplayName = entry.DisplayText;
      //Externalname = entry.ExternalName;
      //Id = entry.Name;
      //if(Datatype == "ZT_KNOTEN")
      //{
      //    if (entry.ReturnTypeReference != null)
      //        PointerZtKnoten = entry.ReturnTypeReference.Name;
      //    else
      //        Parser.Feed.Report($"Returntype Reference is null: of DynProperty ({entry.Name}) {entry.DisplayText} in class {entry.Parent.ComName}", FeedControl.LogLvl.Warn);
      //}
    }
  }
}
