using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  /// <summary>
  /// A Module symbols a namespace within the Addin Framework
  /// </summary>
  public class Apartment : zenonSerializable<Apartment>
  {
    public override string NodeName => "Apartment";

    [zenonSerializableAttribute("ShortName")]
    public string ShortName { get; set; }

    [zenonSerializableAttribute("OverrideViewName")]
    public string OverrideViewName { get; set; }

    [zenonSerializableAttribute("ApartmentId")]
    public int ApartmentId { get; set; }

    public Apartment(string shortName, int apartmentId)
    {
      ShortName = shortName;
      OverrideViewName = string.Empty;
      ApartmentId = apartmentId;
    }

    public Apartment()
    {

    }
  }
}
