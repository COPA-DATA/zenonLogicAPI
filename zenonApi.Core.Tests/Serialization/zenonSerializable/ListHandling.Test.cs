using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests.Serialization.zenonSerializable
{
  public class ListHandling
  {
    #region ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse

    public class ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse : zenonSerializable<ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = false)]
      public List<string> PrimitiveList { get; set; }
    }

    private ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse
      ListHandlingStringPrimitiveListEncapsulateChildsIfListFalseImpl =
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
      // Arrange

      ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse listHandlingStringPrimitiveListEncapsulateChildsIfListFalse = ListHandlingStringPrimitiveListEncapsulateChildsIfListFalseImpl;

      // Apply

      string result = listHandlingStringPrimitiveListEncapsulateChildsIfListFalse.ExportAsString();

      // Assert
      Assert.Equal(zenonSerializableTestXmlComparison.ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse, result);
    }

    [Fact]
    public void ListHandlingSimplePrimitiveListEncapsulateChildsIfListFalseToXElement()
    {
      // Arrange
      ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse listHandlingStringPrimitiveListEncapsulateChildsIfListFalse = ListHandlingStringPrimitiveListEncapsulateChildsIfListFalseImpl;

      // Apply

      XElement result = listHandlingStringPrimitiveListEncapsulateChildsIfListFalse.ExportAsXElement();

      // Assert
      Assert.True(XNode.DeepEquals(result, XElement.Parse(zenonSerializableTestXmlComparison.ListHandlingStringPrimitiveListEncapsulateChildsIfListFalse)));
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
      // Arrange

      ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalseImpl;

      // Apply

      string result = listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse.ExportAsString();

      // Assert
      Assert.Equal(zenonSerializableTestXmlComparison.ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse, result);
    }

    [Fact]
    public void TestListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalseToXElement()
    {
      // Arrange
      ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalseImpl;

      // Apply

      XElement result = listHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse.ExportAsXElement();

      // Assert
      Assert.True(XNode.DeepEquals(result, XElement.Parse(zenonSerializableTestXmlComparison.ListHandlingIntegerPrimitiveListEncapsulateChildsIfListFalse)));
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
      // Arrange

      ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse = ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalseImpl;

      // Apply

      string result = listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse.ExportAsString();

      // Assert
      Assert.Equal(zenonSerializableTestXmlComparison.ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse, result);
    }

    [Fact]
    public void TestListHandlingDoublePrimitiveListEncapsulateChildsIfListFalseToXElement()
    {
      // Arrange
      ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse = ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalseImpl;

      // Apply

      XElement result = listHandlingDoublePrimitiveListEncapsulateChildsIfListFalse.ExportAsXElement();

      // Assert
      Assert.True(XNode.DeepEquals(result, XElement.Parse(zenonSerializableTestXmlComparison.ListHandlingDoublePrimitiveListEncapsulateChildsIfListFalse)));
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
      // Arrange

      ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue listHandlingStringPrimitiveListEncapsulateChildsIfListTrue = ListHandlingStringPrimitiveListEncapsulateChildsIfListTrueImpl;

      // Apply

      string result = listHandlingStringPrimitiveListEncapsulateChildsIfListTrue.ExportAsString();

      // Assert
      Assert.Equal(zenonSerializableTestXmlComparison.ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue, result);
    }

    [Fact]
    public void TestListHandlingSimplePrimitiveListEncapsulateChildsIfListTrueToXElement()
    {
      // Arrange
      ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue listHandlingStringPrimitiveListEncapsulateChildsIfListTrue = ListHandlingStringPrimitiveListEncapsulateChildsIfListTrueImpl;

      // Apply

      XElement result = listHandlingStringPrimitiveListEncapsulateChildsIfListTrue.ExportAsXElement();

      // Assert
      Assert.True(XNode.DeepEquals(result, XElement.Parse(zenonSerializableTestXmlComparison.ListHandlingStringPrimitiveListEncapsulateChildsIfListTrue)));
    }



    #endregion

    #region ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue

    public class ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue : zenonSerializable<ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = true)]
      public List<int> PrimitiveList { get; set; }
    }

    private static ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue
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
      // Arrange

      ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrueImpl;

      // Apply

      string result = listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue.ExportAsString();

      // Assert
      Assert.Equal(zenonSerializableTestXmlComparison.ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue, result);
    }

    [Fact]
    public void TestListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrueToXElement()
    {
      // Arrange
      ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue = ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrueImpl;

      // Apply

      XElement result = listHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue.ExportAsXElement();

      // Assert
      Assert.True(XNode.DeepEquals(result, XElement.Parse(zenonSerializableTestXmlComparison.ListHandlingIntegerPrimitiveListEncapsulateChildsIfListTrue)));
    }

    #endregion

    #region ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue

    public class ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue : zenonSerializable<ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = true)]
      public List<double> PrimitiveList { get; set; }
    }

    private static ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue
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
      // Arrange

      ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue = ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrueImpl;

      // Apply

      string result = listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue.ExportAsString();

      // Assert
      Assert.Equal(zenonSerializableTestXmlComparison.ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue, result);
    }

    [Fact]
    public void TestListHandlingDoublePrimitiveListEncapsulateChildsIfListTrueToXElement()
    {
      // Arrange
      ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue = ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrueImpl;

      // Apply

      XElement result = listHandlingDoublePrimitiveListEncapsulateChildsIfListTrue.ExportAsXElement();

      // Assert
      Assert.True(XNode.DeepEquals(result, XElement.Parse(zenonSerializableTestXmlComparison.ListHandlingDoublePrimitiveListEncapsulateChildsIfListTrue)));
    }

    #endregion

    #region ListHandlingComplexListEncapsulateChildsIfListFalse

    public class ListHandlingComplexListEncapsulateChildsIfListFalse : zenonSerializable<ListHandlingComplexListEncapsulateChildsIfListFalse>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = false)]
      public List<SimpleSingleSerialization.SimpleSingleSerializationClass> PrimitiveList { get; set; }
    }

    private static ListHandlingComplexListEncapsulateChildsIfListFalse
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
      // Arrange

      ListHandlingComplexListEncapsulateChildsIfListFalse listHandlingComplexListEncapsulateChildsIfListFalseImpl = ListHandlingComplexListEncapsulateChildsIfListFalseImpl;

      // Apply

      string result = listHandlingComplexListEncapsulateChildsIfListFalseImpl.ExportAsString();

      // Assert

      Assert.Equal(zenonSerializableTestXmlComparison.ListHandlingComplexListEncapsulateChildsIfListFalse, result);
    }

    [Fact]
    public void TestListHandlingComplexListEncapsulateChildsIfListFalseToXDocument()
    {
      // Arrange

      ListHandlingComplexListEncapsulateChildsIfListFalse listHandlingComplexListEncapsulateChildsIfListFalseImpl = ListHandlingComplexListEncapsulateChildsIfListFalseImpl;

      // Apply

      XElement result = listHandlingComplexListEncapsulateChildsIfListFalseImpl.ExportAsXElement();

      // Assert

      Assert.True(XNode.DeepEquals(result, XElement.Parse(zenonSerializableTestXmlComparison.ListHandlingComplexListEncapsulateChildsIfListFalse)));
    }

    #endregion

    #region ListHandlingComplexListEncapsulateChildsIfListTrue

    public class ListHandlingComplexListEncapsulateChildsIfListTrue : zenonSerializable<ListHandlingComplexListEncapsulateChildsIfListTrue>
    {
      [zenonSerializableNode(nameof(PrimitiveList), EncapsulateChildsIfList = true)]
      public List<SimpleSingleSerialization.SimpleSingleSerializationClass> PrimitiveList { get; set; }
    }

    private static ListHandlingComplexListEncapsulateChildsIfListTrue
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
      // Arrange

      ListHandlingComplexListEncapsulateChildsIfListTrue listHandlingComplexListEncapsulateChildsIfListTrueImpl = ListHandlingComplexListEncapsulateChildsIfListTrueImpl;

      // Apply

      string result = listHandlingComplexListEncapsulateChildsIfListTrueImpl.ExportAsString();
      // Assert

      Assert.Equal(zenonSerializableTestXmlComparison.ListHandlingComplexListEncapsulateChildsIfListTrue, result);
    }

    [Fact]
    public void TestListHandlingComplexListEncapsulateChildsIfListTrueToXDocument()
    {
      // Arrange

      ListHandlingComplexListEncapsulateChildsIfListTrue listHandlingComplexListEncapsulateChildsIfListTrueImpl = ListHandlingComplexListEncapsulateChildsIfListTrueImpl;

      // Apply

      XElement result = listHandlingComplexListEncapsulateChildsIfListTrueImpl.ExportAsXElement();

      // Assert

      Assert.True(XNode.DeepEquals(result, XElement.Parse(zenonSerializableTestXmlComparison.ListHandlingComplexListEncapsulateChildsIfListTrue)));
    }

    #endregion

    #region ListHandlingOnSingleNodes

    public class ListHandlingOnSingleNodesClass: zenonSerializable<ListHandlingOnSingleNodesClass>
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
      // Arrange

      ListHandlingOnSingleNodesClass listHandlingOnSingleNodesClassImpl = ListHandlingOnSingleNodesClassImpl;

      // Apply and Assert

      Assert.ThrowsAny<Exception>(() => listHandlingOnSingleNodesClassImpl.ExportAsString());
    }

    #endregion
  }
}
