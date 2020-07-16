using System;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests.Serialization.zenonSerializable
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
      // Arrange

      NestedSerializationNodes nestedSerializationNodes = NestedSerializationNodesImpl;

      // Apply

      string result = nestedSerializationNodes.ExportAsString();

      // Assert

      Assert.NotNull(result);
      Assert.Equal(result, zenonSerializableTestXmlComparison.TestNestedSerializationNodes);
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
        SimpleSerializationWithAttributes = SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
        SimpleInteger = 5,
        SimpleDouble = 5.3,
        SimpleString = "TestString",
        SimpleAttrInteger = 8,
        SimpleAttrDouble = 8.88,
        SimpleAttrString = "HelloWorld"
      };

    #region ToString

    [Fact]
    public void TestNestedSerializationWithAttributesToString()
    {
      // Arrange

      NestedSerializationWithAttributes nestedSerializationWithAttributes = NestedSerializationWithAttributesImpl;

      // Apply

      string result = nestedSerializationWithAttributes.ExportAsString();

      // Assert

      Assert.Equal(result, zenonSerializableTestXmlComparison.NestedSerializationWithAttributes);

    }

    #endregion

    #region ToXElement

    [Fact]
    public void TestNestedSerializationWithAttributesXDocument()
    {
      // Arrange

      NestedSerializationWithAttributes nestedSerializationWithAttributes = NestedSerializationWithAttributesImpl;

      // Apply

      XElement result = nestedSerializationWithAttributes.ExportAsXElement();

      // Assert

      Assert.NotNull(result);
      Assert.True(XNode.DeepEquals(result, XElement.Parse(zenonSerializableTestXmlComparison.NestedSerializationWithAttributes)));
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

    public static NestedSerializationNestedXmlAsAttribute NestedSerializationNestedXmlAsAttributeImpl = new NestedSerializationNestedXmlAsAttribute
    {
      SimpleSingleSerialization = SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
      SimpleInteger = 5,
      SimpleDouble = 5.3,
      SimpleString = "TestString"
    };


    // should fail as a nested Serialzation should not be in an Attribute
    [Fact]
    public void TestNestedSerializationNestedXmlAsAttribute()
    {
      // Arrange

      NestedSerializationNestedXmlAsAttribute nestedSerializationNestedXmlAsAttribute = NestedSerializationNestedXmlAsAttributeImpl;

      // Apply * Assert

      Assert.ThrowsAny<Exception>(() => nestedSerializationNestedXmlAsAttribute.ExportAsString());

    }

    #endregion
  }
}
