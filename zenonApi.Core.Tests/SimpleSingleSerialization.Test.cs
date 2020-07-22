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


    #region ToString
    [Fact]
    public void TestSimpleSingleSerializationToString()
    {
      var simpleSingleSerialization = SimpleSingleSerializationImpl;

      var result = simpleSingleSerialization.ExportAsString();

      Assert.NotNull(result);
      Assert.Equal(result, ComparisonValues.SimpleSingleSerializationToString);

      var deserialized = SimpleSingleSerializationClass.Import(XElement.Parse(result));
      Assert.True(simpleSingleSerialization.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion


    #region ToXElement
    [Fact]
    public void TestSimpleSingleSerializationToXDocument()
    {
      var simpleSingleSerialization = SimpleSingleSerializationImpl;

      var result = simpleSingleSerialization.ExportAsXElement();

      Assert.NotNull(result);
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.SimpleSingleSerializationToString)));

      var deserialized = SimpleSingleSerializationClass.Import(result);
      Assert.True(simpleSingleSerialization.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion
  }
}