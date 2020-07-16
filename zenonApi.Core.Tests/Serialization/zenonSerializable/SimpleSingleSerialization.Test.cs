using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests.Serialization.zenonSerializable
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
      // Arrange

      SimpleSingleSerializationClass simpleSingleSerialization = SimpleSingleSerializationImpl;

      // Apply

      string result = simpleSingleSerialization.ExportAsString();

      // Assert

      Assert.NotNull(result);
      Assert.Equal(result, zenonSerializableTestXmlComparison.SimpleSingleSerializationToString);
    }

    #endregion

    #region ToXElement

    [Fact]
    public void TestSimpleSingleSerializationToXDocument()
    {
      // Arrange

      SimpleSingleSerializationClass simpleSingleSerialization = SimpleSingleSerializationImpl;

      // Apply

      XElement result = simpleSingleSerialization.ExportAsXElement();

      // Assert

      Assert.NotNull(result);
      Assert.True(XNode.DeepEquals(result, XElement.Parse(zenonSerializableTestXmlComparison.SimpleSingleSerializationToString)));
    }

    #endregion
  }
}