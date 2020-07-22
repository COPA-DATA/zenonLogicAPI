using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class SimpleSerializationWithAttributes
  {
    public class SimpleSerializationWithAttributesClass : zenonSerializable<SimpleSerializationWithAttributesClass>
    {
      [zenonSerializableAttribute(nameof(SimpleAttrInteger))]
      public int SimpleAttrInteger { get; set; }

      [zenonSerializableAttribute(nameof(SimpleAttrDouble))]
      public double SimpleAttrDouble { get; set; }

      [zenonSerializableAttribute(nameof(SimpleAttrString))]
      public string SimpleAttrString { get; set; }

      [zenonSerializableNode(nameof(SimpleInteger))]
      public int SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleDouble))]
      public double SimpleDouble { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }
    }

    public static SimpleSerializationWithAttributesClass SimpleSerializationWithAttributesImpl =>
      new SimpleSerializationWithAttributesClass
      {
        SimpleInteger = 5,
        SimpleDouble = 5.3,
        SimpleString = "TestString",
        SimpleAttrInteger = 8,
        SimpleAttrDouble = 8.88,
        SimpleAttrString = "HelloWorld"
      };

    #region ToString
    [Fact]

    public void TestSimpleSerializationWithAttributesToString()
    {
      var simpleSerializationWithAttributes = SimpleSerializationWithAttributesImpl;

      var result = simpleSerializationWithAttributes.ExportAsString();

      Assert.NotNull(result);
      Assert.Equal(result, ComparisonValues.SimpleSerializationWithAttributesToString);

      var deserialized = SimpleSerializationWithAttributesClass.Import(XElement.Parse(result));
      Assert.True(simpleSerializationWithAttributes.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion


    #region ToXElement
    [Fact]
    public void TestSimpleSerializationWithAttributesToXDocument()
    {
      var simpleSerializationWithAttributes = SimpleSerializationWithAttributesImpl;
      var result = simpleSerializationWithAttributes.ExportAsXElement();

      Assert.NotNull(result);
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.SimpleSerializationWithAttributesToString)));

      var deserialized = SimpleSerializationWithAttributesClass.Import(result);
      Assert.True(simpleSerializationWithAttributes.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion
  }
}
