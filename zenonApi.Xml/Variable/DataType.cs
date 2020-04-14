using zenonApi.Serialization;

namespace zenonApi.Xml.Variable
{
  public class  DataType : zenonSerializable<DataType>
  {
    public override string NodeName => "Type";

    [zenonSerializableAttribute("TypeID")]
    public int ID { get; set; }

    private string _isComplex => IsComplex.ToString().ToUpper();

    [zenonSerializableAttribute("IsComplex")]
    public bool IsComplex { get; set; }

    public DType DType { get; set; }

    public DataType(DType dType)
    {
      DType = dType;
    }
  }

  public enum DType
  {
    Bit = 1,
    Int8 = 2,
    String = 64
  }
}
