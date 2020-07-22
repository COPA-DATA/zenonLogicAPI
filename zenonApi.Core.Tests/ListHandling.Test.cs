using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class ListHandling
  {
    #region ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse
    public class ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse
      : zenonSerializable<ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = false)]
      public List<string> PrimitiveList { get; set; }
    }

    private readonly ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse
      _listHandlingStringPrimitiveListEncapsulateChildsIfListFalseImpl =
        new ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse
        {
          PrimitiveList = new List<string>
          {
            "a","a","a","a","a","a"
          }
        };

    [Fact]
    public void ListHandlingStringPrimitiveListEncapsulateChildsIfListFalseToString()
    {
      var listHandlingStringPrimitiveListEncapsulateChildsIfListFalse
        = _listHandlingStringPrimitiveListEncapsulateChildsIfListFalseImpl;

      var result = listHandlingStringPrimitiveListEncapsulateChildsIfListFalse.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse, result);

      var deserialized = ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse.Import(XElement.Parse(result));
      Assert.True(listHandlingStringPrimitiveListEncapsulateChildsIfListFalse.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void ListHandlingSimplePrimitiveListEncapsulateChildsIfListFalseToXElement()
    {
      var listHandlingStringPrimitiveListEncapsulateChildsIfListFalse
        = _listHandlingStringPrimitiveListEncapsulateChildsIfListFalseImpl;

      var result = listHandlingStringPrimitiveListEncapsulateChildsIfListFalse.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse)));

      var deserialized = ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse.Import(result);
      Assert.True(listHandlingStringPrimitiveListEncapsulateChildsIfListFalse.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion


    #region ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse
    public class ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse : zenonSerializable<ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = false)]
      public List<int> PrimitiveList { get; set; }
    }

    private static readonly ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse
      ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalseImpl =
        new ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse
        {
          PrimitiveList = new List<int>
          {
            1,2,3,4,5,6
          }
        };

    [Fact]
    public void TestListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalseToString()
    {
      var listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse
        = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalseImpl;

      var result = listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse, result);

      var deserialized = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse.Import(XElement.Parse(result));
      Assert.True(listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalseToXElement()
    {
      var listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse
        = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalseImpl;

      var result = listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse)));

      var deserialized = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse.Import(result);
      Assert.True(listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion


    #region ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse
    public class ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse : zenonSerializable<ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = false)]
      public List<double> PrimitiveList { get; set; }
    }

    private static readonly ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse
      ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalseImpl =
        new ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse
        {
          PrimitiveList = new List<double>
          {
            1.1,2.2,3.3,4.4,5.5,6.6
          }
        };

    [Fact]
    public void TestListHandlingDoublePrimitiveListEncapsulateChildsIfListFalseToString()
    {
      var listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse
        = ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalseImpl;

      var result = listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse, result);

      var deserialized = ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse.Import(XElement.Parse(result));
      Assert.True(listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestListHandlingDoublePrimitiveListEncapsulateChildsIfListFalseToXElement()
    {
      var listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse
        = ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalseImpl;

      var result = listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse)));

      var deserialized = ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse.Import(result);
      Assert.True(listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion


    #region ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue
    public class ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue : zenonSerializable<ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = true)]
      public List<string> PrimitiveList { get; set; }
    }

    private readonly ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue
      _listHandlingStringPrimitiveListEncapsulateChildsIfListTrueImpl =
        new ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue
        {
          PrimitiveList = new List<string>
          {
            "a","a","a","a","a","a"
          }
        };

    [Fact]
    public void TestListHandlingStringPrimitiveListEncapsulateChildsIfListTrueToString()
    {
      var listHandlingStringPrimitiveListEncapsulateChildsIfListTrue
        = _listHandlingStringPrimitiveListEncapsulateChildsIfListTrueImpl;
      var result = listHandlingStringPrimitiveListEncapsulateChildsIfListTrue.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue, result);

      var deserialized = ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue.Import(XElement.Parse(result));
      Assert.True(listHandlingStringPrimitiveListEncapsulateChildsIfListTrue.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestListHandlingSimplePrimitiveListEncapsulateChildsIfListTrueToXElement()
    {
      var listHandlingStringPrimitiveListEncapsulateChildsIfListTrue 
        = _listHandlingStringPrimitiveListEncapsulateChildsIfListTrueImpl;
      var result = listHandlingStringPrimitiveListEncapsulateChildsIfListTrue.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue)));

      var deserialized = ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue.Import(result);
      Assert.True(listHandlingStringPrimitiveListEncapsulateChildsIfListTrue.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion


    #region ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue
    public class ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue
      : zenonSerializable<ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = true)]
      public List<int> PrimitiveList { get; set; }
    }

    private static readonly ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue
      ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrueImpl =
        new ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue
        {
          PrimitiveList = new List<int>
          {
            1,2,3,4,5,6
          }
        };

    [Fact]
    public void TestListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrueToString()
    {
      var listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue
        = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrueImpl;

      var result = listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue, result);

      var deserialized = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue.Import(XElement.Parse(result));
      Assert.True(listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrueToXElement()
    {
      var listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue
        = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrueImpl;

      var result = listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue)));

      var deserialized = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue.Import(result);
      Assert.True(listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion


    #region ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue
    public class ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue : zenonSerializable<ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = true)]
      public List<double> PrimitiveList { get; set; }
    }

    private static readonly ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue
      ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrueImpl =
        new ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue
        {
          PrimitiveList = new List<double>
          {
            1.1,2.2,3.3,4.4,5.5,6.6
          }
        };

    [Fact]
    public void TestListHandlingDoublePrimitiveListEncapsulateChildsIfListTrueToString()
    {
      var listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue
        = ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrueImpl;
      var result = listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue, result);

      var deserialized = ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue.Import(XElement.Parse(result));
      Assert.True(listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestListHandlingDoublePrimitiveListEncapsulateChildsIfListTrueToXElement()
    {
      var listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue
        = ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrueImpl;

      var result = listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue)));

      var deserialized = ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue.Import(result);
      Assert.True(listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion


    #region ListHandlingComplexListEncapsulateChildsIfListFalse
    public class ListHandlingComplexListEncapsulateChildsIfListFalse : zenonSerializable<ListHandlingComplexListEncapsulateChildsIfListFalse>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = false)]
      public List<SimpleSingleSerialization.SimpleSingleSerializationClass> PrimitiveList { get; set; }
    }

    private static readonly ListHandlingComplexListEncapsulateChildsIfListFalse
      ListHandlingComplexListEncapsulateChildsIfListFalseImpl =
        new ListHandlingComplexListEncapsulateChildsIfListFalse
        {
          PrimitiveList = new List<SimpleSingleSerialization.SimpleSingleSerializationClass>
          {
            new SimpleSingleSerialization.SimpleSingleSerializationClass
            {
              SimpleInteger = 5,
              SimpleDouble = 5.3,
              SimpleString = "TestString"
            },
            new SimpleSingleSerialization.SimpleSingleSerializationClass
            {
              SimpleInteger = 5,
              SimpleDouble = 5.3,
              SimpleString = "TestString"
            },
            new SimpleSingleSerialization.SimpleSingleSerializationClass
            {
              SimpleInteger = 5,
              SimpleDouble = 5.3,
              SimpleString = "TestString"
            },
            new SimpleSingleSerialization.SimpleSingleSerializationClass
            {
              SimpleInteger = 5,
              SimpleDouble = 5.3,
              SimpleString = "TestString"
            },
            new SimpleSingleSerialization.SimpleSingleSerializationClass
            {
              SimpleInteger = 5,
              SimpleDouble = 5.3,
              SimpleString = "TestString"
            }
          }
        };

    [Fact]
    public void TestListHandlingComplexListEncapsulateChildsIfListFalseToString()
    {
      var listHandlingComplexListEncapsulateChildsIfListFalseImpl
        = ListHandlingComplexListEncapsulateChildsIfListFalseImpl;

      var result = listHandlingComplexListEncapsulateChildsIfListFalseImpl.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingComplexListEncapsulateChildsIfListFalse, result);

      var deserialized = ListHandlingComplexListEncapsulateChildsIfListFalse.Import(XElement.Parse(result));
      Assert.True(listHandlingComplexListEncapsulateChildsIfListFalseImpl.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestListHandlingComplexListEncapsulateChildsIfListFalseToXDocument()
    {
      var listHandlingComplexListEncapsulateChildsIfListFalseImpl
        = ListHandlingComplexListEncapsulateChildsIfListFalseImpl;

      var result = listHandlingComplexListEncapsulateChildsIfListFalseImpl.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingComplexListEncapsulateChildsIfListFalse)));

      var deserialized = ListHandlingComplexListEncapsulateChildsIfListFalse.Import(result);
      Assert.True(listHandlingComplexListEncapsulateChildsIfListFalseImpl.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion


    #region ListHandlingComplexListEncapsulateChildsIfListTrue
    public class ListHandlingComplexListEncapsulateChildsIfListTrue : zenonSerializable<ListHandlingComplexListEncapsulateChildsIfListTrue>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = true)]
      public List<SimpleSingleSerialization.SimpleSingleSerializationClass> PrimitiveList { get; set; }
    }

    private static readonly ListHandlingComplexListEncapsulateChildsIfListTrue
      ListHandlingComplexListEncapsulateChildsIfListTrueImpl =
        new ListHandlingComplexListEncapsulateChildsIfListTrue
        {
          PrimitiveList = new List<SimpleSingleSerialization.SimpleSingleSerializationClass>
          {
            new SimpleSingleSerialization.SimpleSingleSerializationClass
            {
              SimpleInteger = 5,
              SimpleDouble = 5.3,
              SimpleString = "TestString"
            },
            new SimpleSingleSerialization.SimpleSingleSerializationClass
            {
              SimpleInteger = 5,
              SimpleDouble = 5.3,
              SimpleString = "TestString"
            },
            new SimpleSingleSerialization.SimpleSingleSerializationClass
            {
              SimpleInteger = 5,
              SimpleDouble = 5.3,
              SimpleString = "TestString"
            },
            new SimpleSingleSerialization.SimpleSingleSerializationClass
            {
              SimpleInteger = 5,
              SimpleDouble = 5.3,
              SimpleString = "TestString"
            },
            new SimpleSingleSerialization.SimpleSingleSerializationClass
            {
              SimpleInteger = 5,
              SimpleDouble = 5.3,
              SimpleString = "TestString"
            }
          }
        };

    [Fact]
    public void TestListHandlingComplexListEncapsulateChildsIfListTrueToString()
    {
      var listHandlingComplexListEncapsulateChildsIfListTrueImpl = ListHandlingComplexListEncapsulateChildsIfListTrueImpl;

      var result = listHandlingComplexListEncapsulateChildsIfListTrueImpl.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingComplexListEncapsulateChildsIfListTrue, result);

      var deserialized = ListHandlingComplexListEncapsulateChildsIfListTrue.Import(XElement.Parse(result));
      Assert.True(listHandlingComplexListEncapsulateChildsIfListTrueImpl.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestListHandlingComplexListEncapsulateChildsIfListTrueToXDocument()
    {
      var listHandlingComplexListEncapsulateChildsIfListTrueImpl = ListHandlingComplexListEncapsulateChildsIfListTrueImpl;

      var result = listHandlingComplexListEncapsulateChildsIfListTrueImpl.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingComplexListEncapsulateChildsIfListTrue)));

      var deserialized = ListHandlingComplexListEncapsulateChildsIfListTrue.Import(result);
      Assert.True(listHandlingComplexListEncapsulateChildsIfListTrueImpl.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion


    #region ListHandlingOnSingleNodes
    public class ListHandlingOnSingleNodesClass : zenonSerializable<ListHandlingOnSingleNodesClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger), EncapsulateChildsIfList = true)]
      public int SimpleInteger { get; set; }
    }

    public static ListHandlingOnSingleNodesClass ListHandlingOnSingleNodesClassImpl =>
      new ListHandlingOnSingleNodesClass
      {
        SimpleInteger = 123
      };

    [Fact]
    public void ListHandlingOnSingleNodesClassTest()
    {
      // If we have a single node, then it shall be fine to use the EncapsulateChildsIfList parameter.
      // It shall be simply ignored then.
      var result = ListHandlingOnSingleNodesClassImpl.ExportAsString();
      Assert.Equal(ComparisonValues.EncapsulateChildsOnNonListEntity, result);
    }
    #endregion
  }
}
