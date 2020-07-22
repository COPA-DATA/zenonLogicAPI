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
      var nodeOrderingClass = SimpleNodeOrderingImpl;

      var result = nodeOrderingClass.ExportAsString();
      Assert.Equal(ComparisonValues.SimpleNodeOrdering, result);

      var deserialized = SimpleNodeOrderingClass.Import(XElement.Parse(result));
      Assert.True(nodeOrderingClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestSimpleNodeOrderingToXElement()
    {
      var nodeOrderingClass = SimpleNodeOrderingImpl;

      var result = nodeOrderingClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.SimpleNodeOrdering), result));

      var deserialized = SimpleNodeOrderingClass.Import(result);
      Assert.True(nodeOrderingClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
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
      var nodeOrderingClass = SimpleNodeOrderingDuplicateOrderNumbersImpl;

      var result = nodeOrderingClass.ExportAsString();
      Assert.Equal(ComparisonValues.SimpleNodeOrderingDuplicateOrderNumbers, result);

      var deserialized = SimpleNodeOrderingDuplicateOrderNumbersClass.Import(XElement.Parse(result));
      Assert.True(nodeOrderingClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestSimpleNodeOrderingDuplicateOrderNumbersToXElement()
    {
      var nodeOrderingClass = SimpleNodeOrderingDuplicateOrderNumbersImpl;

      var result = nodeOrderingClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.SimpleNodeOrderingDuplicateOrderNumbers), result));

      var deserialized = SimpleNodeOrderingDuplicateOrderNumbersClass.Import(result);
      Assert.True(nodeOrderingClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
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
      var simpleAttributeOrderingClass = SimpleAttributeOrderingClassImpl;

      var result = simpleAttributeOrderingClass.ExportAsString();
      Assert.Equal(ComparisonValues.SimpleAttributeOrdering, result);

      var deserialized = SimpleAttributeOrderingClass.Import(XElement.Parse(result));
      Assert.True(simpleAttributeOrderingClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestSimpleAttributeOrderingToXElement()
    {
      var simpleAttributeOrderingClass = SimpleAttributeOrderingClassImpl;

      var result = simpleAttributeOrderingClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.SimpleAttributeOrdering), result));

      var deserialized = SimpleAttributeOrderingClass.Import(result);
      Assert.True(simpleAttributeOrderingClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
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
      var selectiveNodeOrdering = SelectiveNodeOrderingImpl;
      var result = selectiveNodeOrdering.ExportAsString();
      Assert.Equal(ComparisonValues.SelectiveNodeOrdering, result);

      var deserialized = SelectiveNodeOrderingClass.Import(XElement.Parse(result));
      Assert.True(selectiveNodeOrdering.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestSelectiveNodeOrderingToXElement()
    {
      var selectiveNodeOrdering = SelectiveNodeOrderingImpl;
      var result = selectiveNodeOrdering.ExportAsXElement();

      var comparisonWithoutXmlHeader = XDocument.Parse(ComparisonValues.SelectiveNodeOrdering).Root;
      Assert.True(XNode.DeepEquals(comparisonWithoutXmlHeader, result));

      var deserialized = SelectiveNodeOrderingClass.Import(result);
      Assert.True(selectiveNodeOrdering.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion
  }
}
