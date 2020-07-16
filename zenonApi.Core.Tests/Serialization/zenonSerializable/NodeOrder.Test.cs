using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests.Serialization.zenonSerializable
{
  public class NodeOrder
  {
    #region SimpleNodeOrdering

    public class SimpleNodeOrderingClass: zenonSerializable<SimpleNodeOrderingClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger), NodeOrder = 30)]
      public int SimpleInteger { get; set; }
      [zenonSerializableNode(nameof(SimpleDouble), NodeOrder = 10)]
      public double SimpleDouble { get; set; }
      [zenonSerializableNode(nameof(SimpleString), NodeOrder = 20)]
      public string SimpleString { get; set; }
    }

    public static SimpleNodeOrderingClass SimpleNodeOrderingImpl => new SimpleNodeOrderingClass
    {
      SimpleInteger = 12,
      SimpleDouble = 88.88,
      SimpleString = "Abc"
    };

    [Fact]
    public void TestSimpleNodeOrderingToString()
    {
      // Arrange
      SimpleNodeOrderingClass nodeOrderingClass = SimpleNodeOrderingImpl;

      // Apply

      string result = nodeOrderingClass.ExportAsString();

      // Assert

      Assert.Equal(zenonSerializableTestXmlComparison.SimpleNodeOrdering, result);

    }
    
    [Fact]
    public void TestSimpleNodeOrderingToXElement()
    {
      // Arrange
      SimpleNodeOrderingClass nodeOrderingClass = SimpleNodeOrderingImpl;

      // Apply

      XElement result = nodeOrderingClass.ExportAsXElement();

      // Assert

      Assert.True(XNode.DeepEquals(XElement.Parse(zenonSerializableTestXmlComparison.SimpleNodeOrdering), result));

    }

    #endregion

    #region SimpleNodeOrderingDuplicateOrderNumbers

    public class SimpleNodeOrderingDuplicateOrderNumbersClass : zenonSerializable<SimpleNodeOrderingDuplicateOrderNumbersClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger), NodeOrder = 30)]
      public int SimpleInteger { get; set; }
      [zenonSerializableNode(nameof(SimpleDouble), NodeOrder = 10)]
      public double SimpleDouble { get; set; }
      [zenonSerializableNode(nameof(SimpleString), NodeOrder = 10)]
      public string SimpleString { get; set; }
    }

    public static SimpleNodeOrderingDuplicateOrderNumbersClass SimpleNodeOrderingDuplicateOrderNumbersImpl => new SimpleNodeOrderingDuplicateOrderNumbersClass
    {
      SimpleInteger = 12,
      SimpleDouble = 88.88,
      SimpleString = "Abc"
    };

    [Fact]
    public void TestSimpleNodeOrderingDuplicateOrderNumbersToString()
    {
      // Arrange
      SimpleNodeOrderingDuplicateOrderNumbersClass nodeOrderingClass = SimpleNodeOrderingDuplicateOrderNumbersImpl;

      // Apply

      string result = nodeOrderingClass.ExportAsString();

      // Assert
      
      Assert.Equal(zenonSerializableTestXmlComparison.SimpleNodeOrderingDuplicateOrderNumbers, result);

    }

    [Fact]
    public void TestSimpleNodeOrderingDuplicateOrderNumbersToXElement()
    {
      // Arrange
      SimpleNodeOrderingDuplicateOrderNumbersClass nodeOrderingClass = SimpleNodeOrderingDuplicateOrderNumbersImpl;

      // Apply

      XElement result = nodeOrderingClass.ExportAsXElement();

      // Assert

      Assert.True(XNode.DeepEquals(XElement.Parse(zenonSerializableTestXmlComparison.SimpleNodeOrderingDuplicateOrderNumbers), result));

    }

    #endregion

    #region SimpleAttributeOrdering

    public class SimpleAttributeOrderingClass : zenonSerializable<SimpleAttributeOrderingClass>
    {
      [zenonSerializableAttribute(nameof(SimpleAttrInteger), AttributeOrder = 10)]
      public int SimpleAttrInteger { get; set; }
      [zenonSerializableAttribute(nameof(SimpleAttrDouble), AttributeOrder = 30)]
      public double SimpleAttrDouble { get; set; }
      [zenonSerializableAttribute(nameof(SimpleAttrString), AttributeOrder = 20)]
      public string SimpleAttrString { get; set; }
      [zenonSerializableNode(nameof(SimpleInteger), NodeOrder = 30)]
      public int SimpleInteger { get; set; }
      [zenonSerializableNode(nameof(SimpleDouble), NodeOrder = 10)]
      public double SimpleDouble { get; set; }
      [zenonSerializableNode(nameof(SimpleString), NodeOrder = 20)]
      public string SimpleString { get; set; }
    }

    public SimpleAttributeOrderingClass SimpleAttributeOrderingClassImpl => new SimpleAttributeOrderingClass
    {
      SimpleInteger = 12,
      SimpleDouble = 88.88,
      SimpleString = "Abc",
      SimpleAttrInteger = 12,
      SimpleAttrDouble = 88.88,
      SimpleAttrString = "Abc"
    };

    [Fact]
    public void TestSimpleAttributeOrderingToString()
    {
      // Arrange

      SimpleAttributeOrderingClass simpleAttributeOrderingClass = SimpleAttributeOrderingClassImpl;

      // Apply

      string result = simpleAttributeOrderingClass.ExportAsString();

      // Assert

      Assert.Equal(zenonSerializableTestXmlComparison.SimpleAttributeOrdering, result);

    }

    [Fact]
    public void TestSimpleAttributeOrderingToXElement()
    {
      // Arrange

      SimpleAttributeOrderingClass simpleAttributeOrderingClass = SimpleAttributeOrderingClassImpl;

      // Apply

      XElement result = simpleAttributeOrderingClass.ExportAsXElement();

      // Assert

      Assert.True(XNode.DeepEquals(XElement.Parse(zenonSerializableTestXmlComparison.SimpleAttributeOrdering), result));

    }

    #endregion

  }
}
