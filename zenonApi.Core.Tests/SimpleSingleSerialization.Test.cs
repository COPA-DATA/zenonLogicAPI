using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class SimpleSingleSerialization
  {
    public class SimpleSingleSerializationClass : zenonSerializable<SimpleSingleSerializationClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger))]
      public int SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleDouble))]
      public double SimpleDouble { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }
    }

    public static SimpleSingleSerializationClass SimpleSingleSerializationImpl =>
      new SimpleSingleSerializationClass
      {
        SimpleInteger = 5,
        SimpleDouble = 5.3,
        SimpleString = "TestString"
      };


    [Fact(DisplayName = "De-/Serialization of Primitives with different types")]
    public void TestSimpleSingleSerializationToXDocument()
    {
      var simpleSingleSerialization = SimpleSingleSerializationImpl;

      var result = simpleSingleSerialization.ExportAsXElement();

      Assert.NotNull(result);
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.SimpleSingleSerializationToString)));

      var deserialized = SimpleSingleSerializationClass.Import(result);
      Assert.True(simpleSingleSerialization.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
  }
}