using System;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  public class Field : zenonSerializable<Field>
  {
    public override string NodeName => "Field";

    [zenonSerializableAttribute("FieldType")]
    public Type FieldType { get; set; }
    [zenonSerializableAttribute("FieldName")]
    public string FieldName { get; set; }
    [zenonSerializableAttribute("Modifier")]
    public string Modifier { get; set; }
  }
}
