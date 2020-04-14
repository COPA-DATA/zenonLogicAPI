using zenonApi.Serialization;

namespace zenonApi.Xml.Variable
{
  public class Driver : zenonSerializable<Driver>
  {
    public override string NodeName => "Driver";

    [zenonSerializableAttribute("DriverID")]
    public int ID { get; set; }

    [zenonSerializableNode("Name")]
    public string Name { get; set; }

    [zenonSerializableNode("Module")]
    public string Module { get; set; }

    public Driver(string name)
    {
      ID = 1;
      Name = name;
    }
  }
}
