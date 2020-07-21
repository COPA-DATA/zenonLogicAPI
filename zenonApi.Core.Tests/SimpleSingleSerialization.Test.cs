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
      SimpleSingleSerializationClass simpleSingleSerialization = SimpleSingleSerializationImpl;

      string result = simpleSingleSerialization.ExportAsString();

      Assert.NotNull(result);
      Assert.Equal(result, ComparisonValues.SimpleSingleSerializationToString);
    }
    #endregion


    #region ToXElement
    [Fact]
    public void TestSimpleSingleSerializationToXDocument()
    {
      SimpleSingleSerializationClass simpleSingleSerialization = SimpleSingleSerializationImpl;

      XElement result = simpleSingleSerialization.ExportAsXElement();

      Assert.NotNull(result);
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.SimpleSingleSerializationToString)));
    }
    #endregion
  }
}