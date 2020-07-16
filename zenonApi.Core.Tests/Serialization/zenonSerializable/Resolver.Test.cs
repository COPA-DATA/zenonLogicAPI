using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests.Serialization.zenonSerializable
{
  public class Resolver
  {

    #region ResolverOnNodesThatAreNotLists

    public class ResolverOnNodesThatAreNotListsResolver : IZenonSerializableResolver
    {

      public static int Counter;

      public string GetNodeNameForSerialization(PropertyInfo targetProperty, Type targetType, object value, int index)
      {
        Counter = 0;
        return targetProperty.Name + "_" + index;
      }

      public Type GetTypeForDeserialization(string nodeName, int index)
      {
        Counter++;
        return typeof(ResolverOnNodesThatAreNotListsClass).GetProperty(nodeName.Substring(0, nodeName.IndexOf("_"))).PropertyType;
      }
    }

    public class ResolverOnNodesThatAreNotListsClass: zenonSerializable<ResolverOnNodesThatAreNotListsClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger), resolver: typeof(ResolverOnNodesThatAreNotListsResolver))]
      public string SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleDouble), resolver: typeof(ResolverOnNodesThatAreNotListsResolver))]
      public string SimpleDouble { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }
    }

    public static ResolverOnNodesThatAreNotListsClass ResolverOnNodesThatAreNotListsClassImpl =
      new ResolverOnNodesThatAreNotListsClass
      {
        SimpleInteger = "abc",
        SimpleString = "def",
        SimpleDouble = "hij"
      };

    [Fact]
    public void TestResolverOnNodesThatAreNotListsResolver()
    {
      // Arrange

      ResolverOnNodesThatAreNotListsClass resolverOnNodesThatAreNotListsClass = ResolverOnNodesThatAreNotListsClassImpl;

      // Apply

      string result = resolverOnNodesThatAreNotListsClass.ExportAsString();

      // Assert
      Assert.Equal(result, zenonSerializableTestXmlComparison.ResolverOnNodesThatAreNotLists);
    }
    
    [Fact]
    public void TestResolverOnNodesThatAreNotListsResolverDeserialized()
    {
      // Arrange

      ResolverOnNodesThatAreNotListsClass resolverOnNodesThatAreNotListsClass = ResolverOnNodesThatAreNotListsClassImpl;

      // Apply

      string result = resolverOnNodesThatAreNotListsClass.ExportAsString();

      // Assert
      ResolverOnNodesThatAreNotListsClass deserialized =
        ResolverOnNodesThatAreNotListsClass.Import(XElement.Parse(result));
    }

    [Fact]
    public void TestResolverOnNodesThatAreNotListsResolverNumberOfResolverCalls()
    {
      // Arrange

      ResolverOnNodesThatAreNotListsClass resolverOnNodesThatAreNotListsClass = ResolverOnNodesThatAreNotListsClassImpl;
      
      // Apply
      string result = resolverOnNodesThatAreNotListsClass.ExportAsString();

      try
      {
        ResolverOnNodesThatAreNotListsClass des = ResolverOnNodesThatAreNotListsClass.Import(XElement.Parse(result));
      }
      catch(Exception) { }


      // Assert

      Assert.Equal(2, ResolverOnNodesThatAreNotListsResolver.Counter);


    }


    #endregion

    #region ResolverOnNodeThatIsNotLists

    public class ResolverOnNodeThatIsNotListsClass : zenonSerializable<ResolverOnNodeThatIsNotListsClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger), resolver: typeof(ResolverOnNodesThatAreNotListsResolver))]
      public string SimpleInteger { get; set; }
    }

    public static ResolverOnNodeThatIsNotListsClass ResolverOnNodeThatIsNotListsClassImpl => new ResolverOnNodeThatIsNotListsClass
    {
      SimpleInteger = "abc"
    };

    [Fact]
    public void TestResolverOnNodeThatIsNotLists()
    {
      // Arrange

      ResolverOnNodeThatIsNotListsClass resolverOnNodesThatAreNotListsClass = ResolverOnNodeThatIsNotListsClassImpl;

      // Apply

      string result = resolverOnNodesThatAreNotListsClass.ExportAsString();

      // Assert
      ResolverOnNodeThatIsNotListsClass deserialized =
        ResolverOnNodeThatIsNotListsClass.Import(XElement.Parse(result));
    }

    #endregion

    #region ResolverThatReturnsPropertyInfoType

    public class ResolverThatReturnsPropertyInfoTypeClass : zenonSerializable<ResolverThatReturnsPropertyInfoTypeClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger), resolver: typeof(ResolverThatReturnsPropertyInfoTypeResolver))]
      public List<string> SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }
    }

    public class ResolverThatReturnsPropertyInfoTypeResolver : IZenonSerializableResolver
    {
      public string GetNodeNameForSerialization(PropertyInfo targetProperty, Type targetType, object value, int index)
      {
        return targetProperty.Name + "_" + index;
      }

      public Type GetTypeForDeserialization(string nodeName, int index)
      {
        return typeof(ResolverOnNodesThatAreNotListsClass).GetProperty(nodeName.Substring(0, nodeName.IndexOf("_"))).PropertyType;
      }
    }

    public static ResolverThatReturnsPropertyInfoTypeClass ResolverThatReturnsPropertyInfoTypeClassImpl => new ResolverThatReturnsPropertyInfoTypeClass
    {
      SimpleInteger = new List<string>
      {
        "abc", "def", "hij"
      }
    };

    [Fact]
    public void TestResolverThatReturnsPropertyInfoType()
    {
      // Arrange

      ResolverThatReturnsPropertyInfoTypeClass resolverThatReturnsPropertyInfoTypeClass = ResolverThatReturnsPropertyInfoTypeClassImpl;

      // Apply

      string result = resolverThatReturnsPropertyInfoTypeClass.ExportAsString();

      // Assert
      
      Assert.Equal(result, zenonSerializableTestXmlComparison.ResolverThatReturnsPropertyInfoType);
    }


    #endregion

    #region ResolverThatReturnsPropertyWrongType

    public class ResolverThatReturnsPropertyWrongTypeClass : zenonSerializable<ResolverThatReturnsPropertyWrongTypeClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger), resolver: typeof(ResolverThatReturnsPropertyInfoTypeResolver))]
      public List<string> SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }
    }

    public class ResolverThatReturnsPropertyWrongTypeResolver : IZenonSerializableResolver
    {
      public string GetNodeNameForSerialization(PropertyInfo targetProperty, Type targetType, object value, int index)
      {
        return targetProperty.Name + "_" + index;
      }

      public Type GetTypeForDeserialization(string nodeName, int index)
      {
        return typeof(ResolverThatReturnsPropertyWrongTypeClass).GetProperty(nodeName.Substring(0, nodeName.IndexOf("_"))).GetType();
      }
    }

    public static ResolverThatReturnsPropertyWrongTypeClass ResolverThatReturnsPropertyWrongTypeClassImpl => new ResolverThatReturnsPropertyWrongTypeClass
    {
      SimpleInteger = new List<string>
      {
        "abc", "def", "hij"
      }
    };

    [Fact]
    public void TestResolverThatReturnsPropertyWrongType()
    {
      // Arrange

      ResolverThatReturnsPropertyWrongTypeClass resolverThatReturnsPropertyWrongTypeClass = ResolverThatReturnsPropertyWrongTypeClassImpl;

      // Apply 

      string result = resolverThatReturnsPropertyWrongTypeClass.ExportAsString();

      // Assert
      
      Assert.ThrowsAny<Exception>(() => ResolverThatReturnsPropertyWrongTypeClass.Import(XElement.Parse(result)));
    }


    #endregion

  }
}
