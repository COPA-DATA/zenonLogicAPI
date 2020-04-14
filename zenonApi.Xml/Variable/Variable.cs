using zenonApi.Serialization;
using zenonApi.Xml.ZTKnoten;

namespace zenonApi.Xml.Variable
{
  public class Variable : zenonSerializable<Variable>, IDynPropertyMap
  {
    public override string NodeName => "Variable";

    #region zenon exported properties
    [zenonSerializableAttribute("DriverID")]
    public int ID { get; set; }

    [zenonSerializableAttribute("TypeID")]
    public DataType DataType { get; set; }

    [zenonSerializableAttribute("ShortName")]
    public string Name { get; set; }

    [zenonSerializableAttribute("HWObjectType")]
    public int HWObjectType => (int)ChannelType;

    [zenonSerializableAttribute("HWObjectName")]
    public ChannelType ChannelType { get; set; }

    [zenonSerializableNode("AdjustHardware")]
    private int AdjustHardware => CAKSVarED.AdjustHardware;

    [zenonSerializableNode("Active")]
    private bool Active => CAKSVarED.Limits.Active;
    [zenonSerializableNode("Alarm")]
    private bool Alarm => CAKSVarED.Limits.Alarm;

    #endregion

    #region DynProperty Maps
    public CAKSVarED CAKSVarED { get; set; }
    #endregion

    public Driver Driver { get; set; }


    public Variable(string name, ChannelType channelType, DataType dataType, Driver driver)
    {
      Name = name;
      ChannelType = ChannelType;
      DataType = dataType;
      Driver = driver;

      CAKSVarED = new CAKSVarED
      {
        Limits = new CVarGrenze()
      };

    }

    public IDynProperty GetDynamicProperty(string dynPropertyName)
    {
      IDynProperty dynProperty = (IDynProperty)this.GetType().GetProperty(dynPropertyName)?.GetValue(this);
      return dynProperty;
    }
  }
}
