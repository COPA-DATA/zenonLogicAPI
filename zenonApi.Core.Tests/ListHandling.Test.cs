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
      ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse listHandlingStringPrimitiveListEncapsulateChildsIfListFalse
        = _listHandlingStringPrimitiveListEncapsulateChildsIfListFalseImpl;

      string result = listHandlingStringPrimitiveListEncapsulateChildsIfListFalse.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse, result);
    }

    [Fact]
    public void ListHandlingSimplePrimitiveListEncapsulateChildsIfListFalseToXElement()
    {
      ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse listHandlingStringPrimitiveListEncapsulateChildsIfListFalse
        = _listHandlingStringPrimitiveListEncapsulateChildsIfListFalseImpl;

      XElement result = listHandlingStringPrimitiveListEncapsulateChildsIfListFalse.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse)));
    }
    #endregion


    #region ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse
    public class ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse : zenonSerializable<ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = false)]
      public List<int> PrimitiveList { get; set; }
    }

    private static ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse
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
      ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse
        = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalseImpl;

      string result = listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse, result);
    }

    [Fact]
    public void TestListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalseToXElement()
    {
      ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse
        = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalseImpl;

      XElement result = listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse)));
    }
    #endregion


    #region ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse
    public class ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse : zenonSerializable<ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = false)]
      public List<double> PrimitiveList { get; set; }
    }

    private static ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse
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
      ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse
        = ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalseImpl;

      string result = listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse, result);
    }

    [Fact]
    public void TestListHandlingDoublePrimitiveListEncapsulateChildsIfListFalseToXElement()
    {
      ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse
        = ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalseImpl;

      XElement result = listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse)));
    }
    #endregion


    #region ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue
    public class ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue : zenonSerializable<ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = true)]
      public List<string> PrimitiveList { get; set; }
    }

    private ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue
      ListHandlingStringPrimitiveListEncapsulateChildsIfListTrueImpl =
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
      ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue listHandlingStringPrimitiveListEncapsulateChildsIfListTrue = ListHandlingStringPrimitiveListEncapsulateChildsIfListTrueImpl;
      string result = listHandlingStringPrimitiveListEncapsulateChildsIfListTrue.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue, result);
    }

    [Fact]
    public void TestListHandlingSimplePrimitiveListEncapsulateChildsIfListTrueToXElement()
    {
      ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue listHandlingStringPrimitiveListEncapsulateChildsIfListTrue = ListHandlingStringPrimitiveListEncapsulateChildsIfListTrueImpl;
      XElement result = listHandlingStringPrimitiveListEncapsulateChildsIfListTrue.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue)));
    }
    #endregion


    #region ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue
    public class ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue : zenonSerializable<ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue>
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
      ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue
        = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrueImpl;

      string result = listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue, result);
    }

    [Fact]
    public void TestListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrueToXElement()
    {
      ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue
        = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrueImpl;

      XElement result = listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue)));
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
      ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue
        = ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrueImpl;
      string result = listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue, result);
    }

    [Fact]
    public void TestListHandlingDoublePrimitiveListEncapsulateChildsIfListTrueToXElement()
    {
      ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue
        = ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrueImpl;

      XElement result = listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue)));
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
      ListHandlingComplexListEncapsulateChildsIfListFalse listHandlingComplexListEncapsulateChildsIfListFalseImpl
        = ListHandlingComplexListEncapsulateChildsIfListFalseImpl;

      string result = listHandlingComplexListEncapsulateChildsIfListFalseImpl.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingComplexListEncapsulateChildsIfListFalse, result);
    }

    [Fact]
    public void TestListHandlingComplexListEncapsulateChildsIfListFalseToXDocument()
    {
      ListHandlingComplexListEncapsulateChildsIfListFalse listHandlingComplexListEncapsulateChildsIfListFalseImpl
        = ListHandlingComplexListEncapsulateChildsIfListFalseImpl;

      XElement result = listHandlingComplexListEncapsulateChildsIfListFalseImpl.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingComplexListEncapsulateChildsIfListFalse)));
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
      ListHandlingComplexListEncapsulateChildsIfListTrue listHandlingComplexListEncapsulateChildsIfListTrueImpl = ListHandlingComplexListEncapsulateChildsIfListTrueImpl;

      string result = listHandlingComplexListEncapsulateChildsIfListTrueImpl.ExportAsString();
      Assert.Equal(ComparisonValues.ListHandlingComplexListEncapsulateChildsIfListTrue, result);
    }

    [Fact]
    public void TestListHandlingComplexListEncapsulateChildsIfListTrueToXDocument()
    {
      ListHandlingComplexListEncapsulateChildsIfListTrue listHandlingComplexListEncapsulateChildsIfListTrueImpl = ListHandlingComplexListEncapsulateChildsIfListTrueImpl;

      XElement result = listHandlingComplexListEncapsulateChildsIfListTrueImpl.ExportAsXElement();
      Assert.True(XNode.DeepEquals(result, XElement.Parse(ComparisonValues.ListHandlingComplexListEncapsulateChildsIfListTrue)));
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
      string result = ListHandlingOnSingleNodesClassImpl.ExportAsString();
      Assert.Equal(ComparisonValues.EncapsulateChildsOnNonListEntity, result);
    }
    #endregion
  }
}
