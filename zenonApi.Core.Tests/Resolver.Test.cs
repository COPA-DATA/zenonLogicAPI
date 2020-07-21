using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class Resolver
  {
    #region ResolverOnNodesThatAreNotLists
    public class ResolverOnNodesThatAreNotListsResolver : IZenonSerializableResolver
    {
      public string GetNodeNameForSerialization(PropertyInfo targetProperty, Type targetType, object value, int index)
      {
        return targetProperty.Name + "_" + index;
      }

      public Type GetTypeForDeserialization(string nodeName, int index)
      {
        var name = nodeName.Substring(0, nodeName.IndexOf("_", StringComparison.InvariantCulture));
        // ReSharper disable once PossibleNullReferenceException : Property exists, will not be null.
        return typeof(ResolverOnNodesThatAreNotListsClass).GetProperty(name).PropertyType;
      }
    }

    public class ResolverOnNodesThatAreNotListsClass : zenonSerializable<ResolverOnNodesThatAreNotListsClass>
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
      ResolverOnNodesThatAreNotListsClass resolverOnNodesThatAreNotListsClass = ResolverOnNodesThatAreNotListsClassImpl;
      string result = resolverOnNodesThatAreNotListsClass.ExportAsString();
      Assert.Equal(result, ComparisonValues.ResolverOnNodesThatAreNotLists);
    }

    [Fact]
    public void TestResolverOnNodesThatAreNotListsResolverDeserialized()
    {
      ResolverOnNodesThatAreNotListsClass resolverOnNodesThatAreNotListsClass = ResolverOnNodesThatAreNotListsClassImpl;
      string result = resolverOnNodesThatAreNotListsClass.ExportAsString();

      Assert.ThrowsAny<Exception>(() => ResolverOnNodesThatAreNotListsClass.Import(XElement.Parse(result)));
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
      ResolverOnNodeThatIsNotListsClass resolverOnNodesThatAreNotListsClass = ResolverOnNodeThatIsNotListsClassImpl;
      string result = resolverOnNodesThatAreNotListsClass.ExportAsString();

      ResolverOnNodeThatIsNotListsClass deserialized =
        ResolverOnNodeThatIsNotListsClass.Import(XElement.Parse(result));
      Assert.Equal("abc", deserialized.SimpleInteger);
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
        string name = nodeName.Substring(0, nodeName.IndexOf("_", StringComparison.InvariantCulture));
        // ReSharper disable once PossibleNullReferenceException : Will not be null, property exists.
        return typeof(ResolverOnNodesThatAreNotListsClass).GetProperty(name).PropertyType;
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
      ResolverThatReturnsPropertyInfoTypeClass resolverThatReturnsPropertyInfoTypeClass = ResolverThatReturnsPropertyInfoTypeClassImpl;

      string result = resolverThatReturnsPropertyInfoTypeClass.ExportAsString();
      Assert.Equal(ComparisonValues.ResolverThatReturnsPropertyInfoType, result);
    }
    #endregion


    #region ResolverThatReturnsPropertyWrongType
    public class ResolverThatReturnsPropertyWrongTypeClass : zenonSerializable<ResolverThatReturnsPropertyWrongTypeClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger), resolver: typeof(ResolverThatReturnsPropertyWrongTypeResolver))]
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
        string name = nodeName.Substring(0, nodeName.IndexOf("_", StringComparison.InvariantCulture));
        // ReSharper disable once PossibleNullReferenceException : Property exists in our cases, will not be null.
        return typeof(ResolverThatReturnsPropertyWrongTypeClass).GetProperty(name).GetType();
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
      ResolverThatReturnsPropertyWrongTypeClass resolverThatReturnsPropertyWrongTypeClass = ResolverThatReturnsPropertyWrongTypeClassImpl;
      string result = resolverThatReturnsPropertyWrongTypeClass.ExportAsString();
      Assert.ThrowsAny<Exception>(() => ResolverThatReturnsPropertyWrongTypeClass.Import(XElement.Parse(result)));
    }
    #endregion
  }
}
