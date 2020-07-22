using System;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class NestedSerialization
  {
    #region NestedSerializationNodes
    public class NestedSerializationNodes : zenonSerializable<NestedSerializationNodes>
    {
      [zenonSerializableNode(nameof(SimpleInteger))]
      public int SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleDouble))]
      public double SimpleDouble { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }

      [zenonSerializableNode(nameof(SimpleSingleSerialization))]
      public SimpleSingleSerialization.SimpleSingleSerializationClass SimpleSingleSerialization { get; set; }
    }

    public NestedSerializationNodes NestedSerializationNodesImpl = new NestedSerializationNodes
    {
      SimpleSingleSerialization = SimpleSingleSerialization.SimpleSingleSerializationImpl,
      SimpleInteger = 5,
      SimpleDouble = 5.3,
      SimpleString = "TestString"
    };

    [Fact]
    public void TestNestedSerializationNodes()
    {
      var nestedSerializationNodes = NestedSerializationNodesImpl;

      var result = nestedSerializationNodes.ExportAsString();
      Assert.NotNull(result);
      Assert.Equal(result, ComparisonValues.TestNestedSerializationNodes);

      var deserialized = NestedSerializationNodes.Import(XElement.Parse(result));
      Assert.True(nestedSerializationNodes.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion


    #region NestedSerializationWithAttributes
    public class NestedSerializationWithAttributes : zenonSerializable<NestedSerializationWithAttributes>
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

      [zenonSerializableNode(nameof(SimpleSerializationWithAttributes))]
      public SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass SimpleSerializationWithAttributes { get; set; }
    }

    public static NestedSerializationWithAttributes NestedSerializationWithAttributesImpl =
      new NestedSerializationWithAttributes()
      {
        SimpleAttrInteger = 8,
        SimpleAttrDouble = 8.88,
        SimpleAttrString = "HelloWorld",
        SimpleInteger = 5,
        SimpleSerializationWithAttributes = SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
        SimpleDouble = 5.3,
        SimpleString = "TestString"
      };


    #region ToString
    [Fact]
    public void TestNestedSerializationWithAttributesToString()
    {
      var nestedSerializationWithAttributes = NestedSerializationWithAttributesImpl;

      var result = nestedSerializationWithAttributes.ExportAsString();
      Assert.Equal(result, ComparisonValues.NestedSerializationWithAttributes);

      var deserialized = NestedSerializationWithAttributes.Import(XElement.Parse(result));
      Assert.True(nestedSerializationWithAttributes.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion

    #region ToXElement
    [Fact]
    public void TestNestedSerializationWithAttributesXDocument()
    {
      var nestedSerializationWithAttributes = NestedSerializationWithAttributesImpl;

      var result = nestedSerializationWithAttributes.ExportAsXElement();
      Assert.NotNull(result);
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.NestedSerializationWithAttributes)));

      var deserialized = NestedSerializationWithAttributes.Import(result);
      Assert.True(nestedSerializationWithAttributes.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion
    #endregion


    #region NestedSerializationNestedXmlAsAttribute
    public class NestedSerializationNestedXmlAsAttribute : zenonSerializable<NestedSerializationNestedXmlAsAttribute>
    {
      [zenonSerializableNode(nameof(SimpleInteger))]
      public int SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleDouble))]
      public double SimpleDouble { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }

      [zenonSerializableAttribute(nameof(SimpleSingleSerialization))]
      public SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass SimpleSingleSerialization { get; set; }
    }

    public static NestedSerializationNestedXmlAsAttribute NestedSerializationNestedXmlAsAttributeImpl
      = new NestedSerializationNestedXmlAsAttribute
      {
        SimpleSingleSerialization = SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
        SimpleInteger = 5,
        SimpleDouble = 5.3,
        SimpleString = "TestString"
      };

    [Fact]
    public void TestNestedSerializationNestedXmlAsAttribute()
    {
      // Should fail as a nested Serialzation should not be in an Attribute
      var nestedSerializationNestedXmlAsAttribute = NestedSerializationNestedXmlAsAttributeImpl;
      Assert.ThrowsAny<Exception>(() => nestedSerializationNestedXmlAsAttribute.ExportAsString());
    }
    #endregion
  }
}
