using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class NodeOrder
  {
    #region SimpleNodeOrdering
    public class SimpleNodeOrderingClass : zenonSerializable<SimpleNodeOrderingClass>
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
      SimpleNodeOrderingClass nodeOrderingClass = SimpleNodeOrderingImpl;

      string result = nodeOrderingClass.ExportAsString();
      Assert.Equal(ComparisonValues.SimpleNodeOrdering, result);
    }

    [Fact]
    public void TestSimpleNodeOrderingToXElement()
    {
      SimpleNodeOrderingClass nodeOrderingClass = SimpleNodeOrderingImpl;

      XElement result = nodeOrderingClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.SimpleNodeOrdering), result));
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

    public static SimpleNodeOrderingDuplicateOrderNumbersClass SimpleNodeOrderingDuplicateOrderNumbersImpl =>
      new SimpleNodeOrderingDuplicateOrderNumbersClass
      {
        SimpleInteger = 12,
        SimpleDouble = 88.88,
        SimpleString = "Abc"
      };

    [Fact]
    public void TestSimpleNodeOrderingDuplicateOrderNumbersToString()
    {
      SimpleNodeOrderingDuplicateOrderNumbersClass nodeOrderingClass = SimpleNodeOrderingDuplicateOrderNumbersImpl;

      string result = nodeOrderingClass.ExportAsString();
      Assert.Equal(ComparisonValues.SimpleNodeOrderingDuplicateOrderNumbers, result);

    }

    [Fact]
    public void TestSimpleNodeOrderingDuplicateOrderNumbersToXElement()
    {
      SimpleNodeOrderingDuplicateOrderNumbersClass nodeOrderingClass = SimpleNodeOrderingDuplicateOrderNumbersImpl;

      XElement result = nodeOrderingClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.SimpleNodeOrderingDuplicateOrderNumbers), result));
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
      SimpleAttributeOrderingClass simpleAttributeOrderingClass = SimpleAttributeOrderingClassImpl;

      string result = simpleAttributeOrderingClass.ExportAsString();
      Assert.Equal(ComparisonValues.SimpleAttributeOrdering, result);
    }

    [Fact]
    public void TestSimpleAttributeOrderingToXElement()
    {
      SimpleAttributeOrderingClass simpleAttributeOrderingClass = SimpleAttributeOrderingClassImpl;

      XElement result = simpleAttributeOrderingClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.SimpleAttributeOrdering), result));
    }
    #endregion


    #region SelectiveNodeOrdering
    public class SelectiveNodeOrderingClass : zenonSerializable<SelectiveNodeOrderingClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger), NodeOrder = 30)]
      public int SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleDouble), NodeOrder = 10)]
      public double SimpleDouble { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }
    }

    public static SelectiveNodeOrderingClass SelectiveNodeOrderingImpl => new SelectiveNodeOrderingClass
    {
      SimpleInteger = 12,
      SimpleDouble = 88.88,
      SimpleString = "Abc"
    };

    [Fact]
    public void TestSelectiveNodeOrderingToString()
    {
      SelectiveNodeOrderingClass selectiveNodeOrdering = SelectiveNodeOrderingImpl;
      string result = selectiveNodeOrdering.ExportAsString();
      Assert.Equal(ComparisonValues.SelectiveNodeOrdering, result);
    }

    [Fact]
    public void TestSelectiveNodeOrderingToXElement()
    {
      SimpleNodeOrderingClass selectiveNodeOrdering = SimpleNodeOrderingImpl;
      XElement result = selectiveNodeOrdering.ExportAsXElement();

      XElement comparisonWithoutXmlHeader = XDocument.Parse(ComparisonValues.SelectiveNodeOrdering).Root;
      Assert.True(XNode.DeepEquals(comparisonWithoutXmlHeader, result));
    }
    #endregion
  }
}
