using zenonApi.Xml.ZTKnoten;

namespace zenonApi.Xml.Variable
{
  public class CAKSVarED : IDynProperty, IDynPropertyMap
  {
    public int AdjustHardware { get; set; }
    public CVarGrenze Limits { get; set; }

    public IDynProperty GetDynamicProperty(string dynPropertyName)
    {
      IDynProperty dynProperty = (IDynProperty)this.GetType().GetProperty(dynPropertyName)?.GetValue(this);
      return dynProperty;
    }
  }
}