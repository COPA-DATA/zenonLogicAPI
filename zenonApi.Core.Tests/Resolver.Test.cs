using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using Xunit;
using zenonApi.Extensions;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class Resolver
  {
    #region ResolverOnNodesThatAreNotLists
    public class ResolverOnNodesThatAreNotListsResolverWrongUsage : IZenonSerializableResolver
    {
      public string GetNodeNameForSerialization(PropertyInfo targetProperty, Type targetType, object value, int index)
      {
        return targetProperty.Name + "_" + index;
      }

      public Type GetTypeForDeserialization(PropertyInfo targetProperty, string nodeName, XElement node, int index)
      {
        var name = nodeName.Substring(0, nodeName.IndexOf("_", StringComparison.InvariantCulture));
        // ReSharper disable once PossibleNullReferenceException : Property exists, will not be null.
        return typeof(ResolverOnNodesThatAreNotListsClassWrongUsage).GetProperty(name).PropertyType;
      }
    }

    public class ResolverOnNodesThatAreNotListsResolverCorrectUsage : IZenonSerializableResolver
    {
      public string GetNodeNameForSerialization(PropertyInfo targetProperty, Type targetType, object value, int index)
      {
        return targetProperty.Name + "_" + index;
      }

      public Type GetTypeForDeserialization(PropertyInfo targetProperty, string nodeName, XElement node, int index)
      {
        var removeFrom = nodeName.IndexOf("_", StringComparison.InvariantCulture);
        if (removeFrom == -1)
        {
          return null;
        }

        var name = nodeName.Substring(0, removeFrom);
        if (name == targetProperty.Name)
        {
          return targetProperty.PropertyType;
        }

        return null;
      }
    }

    public class ResolverOnNodesThatAreNotListsClassWrongUsage : zenonSerializable<ResolverOnNodesThatAreNotListsClassWrongUsage>
    {
      public override string NodeName => "ResolverOnNodesThatAreNotListsClass";

      [zenonSerializableNode(nameof(SimpleInteger), typeof(ResolverOnNodesThatAreNotListsResolverWrongUsage))]
      public string SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleDouble), typeof(ResolverOnNodesThatAreNotListsResolverWrongUsage))]
      public string SimpleDouble { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }
    }

    public class ResolverOnNodesThatAreNotListsClassCorrectUsage : zenonSerializable<ResolverOnNodesThatAreNotListsClassCorrectUsage>
    {
      public override string NodeName => "ResolverOnNodesThatAreNotListsClass";

      [zenonSerializableNode(nameof(SimpleInteger), typeof(ResolverOnNodesThatAreNotListsResolverCorrectUsage))]
      public string SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleDouble), typeof(ResolverOnNodesThatAreNotListsResolverCorrectUsage))]
      public string SimpleDouble { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }
    }

    public static ResolverOnNodesThatAreNotListsClassWrongUsage ResolverOnNodesThatAreNotListsClassWrongUsageUsageImpl =
      new ResolverOnNodesThatAreNotListsClassWrongUsage
      {
        SimpleInteger = "abc",
        SimpleString = "def",
        SimpleDouble = "hij"
      };

    public static ResolverOnNodesThatAreNotListsClassCorrectUsage ResolverOnNodesThatAreNotListsClassCorrectUsageUsageImpl =
      new ResolverOnNodesThatAreNotListsClassCorrectUsage
      {
        SimpleInteger = "abc",
        SimpleString = "def",
        SimpleDouble = "hij"
      };

    [Fact(DisplayName = "Resolver on multiple primitive values")]
    public void TestResolverOnNodesThatAreNotListsResolver()
    {
      var resolverOnNodesThatAreNotListsClassWrongUsage = ResolverOnNodesThatAreNotListsClassWrongUsageUsageImpl;

      var result = resolverOnNodesThatAreNotListsClassWrongUsage.ExportAsString();
      Assert.Equal(result, ComparisonValues.ResolverOnNodesThatAreNotLists);

      Assert.ThrowsAny<Exception>(() => ResolverOnNodesThatAreNotListsClassWrongUsage.Import(XElement.Parse(result)));

      var deserialized = ResolverOnNodesThatAreNotListsClassCorrectUsage.Import(XElement.Parse(result));

      var resolverOnNodesThatAreNotListsClassCorrectUsage
        = ResolverOnNodesThatAreNotListsClassCorrectUsageUsageImpl;

      Assert.True(resolverOnNodesThatAreNotListsClassCorrectUsage.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact(DisplayName = "Resolver deserialization")]
    public void TestResolverOnNodesThatAreNotListsResolverDeserialized()
    {
      var resolverOnNodesThatAreNotListsClassWrongUsage = ResolverOnNodesThatAreNotListsClassWrongUsageUsageImpl;
      var result = resolverOnNodesThatAreNotListsClassWrongUsage.ExportAsString();

      Assert.ThrowsAny<Exception>(() => ResolverOnNodesThatAreNotListsClassWrongUsage.Import(XElement.Parse(result)));

      var resolverOnNodesThatAreNotListsClassCorrectUsage
        = ResolverOnNodesThatAreNotListsClassCorrectUsageUsageImpl;

      var deserialized = ResolverOnNodesThatAreNotListsClassCorrectUsage.Import(XElement.Parse(result));
      Assert.True(resolverOnNodesThatAreNotListsClassCorrectUsage.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion


    #region ResolverOnNodeThatIsNotLists
    public class ResolverOnNodeThatIsNotListsClass : zenonSerializable<ResolverOnNodeThatIsNotListsClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger), resolver: typeof(ResolverOnNodesThatAreNotListsResolverWrongUsage))]
      public string SimpleInteger { get; set; }
    }

    public static ResolverOnNodeThatIsNotListsClass ResolverOnNodeThatIsNotListsClassImpl => new ResolverOnNodeThatIsNotListsClass
    {
      SimpleInteger = "abc"
    };

    [Fact(DisplayName = "Resolver on primitive value")]
    public void TestResolverOnNodeThatIsNotLists()
    {
      var resolverOnNodesThatAreNotListsClass = ResolverOnNodeThatIsNotListsClassImpl;
      var result = resolverOnNodesThatAreNotListsClass.ExportAsString();

      var deserialized =
        ResolverOnNodeThatIsNotListsClass.Import(XElement.Parse(result));
      Assert.Equal("abc", deserialized.SimpleInteger);

      Assert.True(resolverOnNodesThatAreNotListsClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
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

      public Type GetTypeForDeserialization(PropertyInfo targetProperty, string nodeName, XElement node, int index)
      {
        var name = nodeName.Substring(0, nodeName.IndexOf("_", StringComparison.InvariantCulture));
        // ReSharper disable once PossibleNullReferenceException : Will not be null, property exists.
        return typeof(ResolverOnNodesThatAreNotListsClassWrongUsage).GetProperty(name).PropertyType;
      }
    }

    public static ResolverThatReturnsPropertyInfoTypeClass ResolverThatReturnsPropertyInfoTypeClassImpl => new ResolverThatReturnsPropertyInfoTypeClass
    {
      SimpleInteger = new List<string>
      {
        "abc", "def", "hij"
      }
    };

    [Fact(DisplayName = "Resolver returning invalid type on deserialization")]
    public void TestResolverThatReturnsPropertyInfoType()
    {
      var resolverThatReturnsPropertyInfoTypeClass = ResolverThatReturnsPropertyInfoTypeClassImpl;

      var result = resolverThatReturnsPropertyInfoTypeClass.ExportAsString();
      Assert.Equal(ComparisonValues.ResolverThatReturnsPropertyInfoType, result);

      var deserialized = ResolverThatReturnsPropertyInfoTypeClass.Import(XElement.Parse(result));
      Assert.True(resolverThatReturnsPropertyInfoTypeClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion


    #region ResolverThatReturnsPropertyWrongType
    public class ResolverThatReturnsPropertyWrongTypeClass : zenonSerializable<ResolverThatReturnsPropertyWrongTypeClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger), typeof(ResolverThatReturnsPropertyWrongTypeResolver))]
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

      public Type GetTypeForDeserialization(PropertyInfo targetProperty, string nodeName, XElement node, int index)
      {
        var name = nodeName.Substring(0, nodeName.IndexOf("_", StringComparison.InvariantCulture));
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

    [Fact(DisplayName = "Resolver on list returning invalid type on deserialization")]
    public void TestResolverThatReturnsPropertyWrongType()
    {
      var resolverThatReturnsPropertyWrongTypeClass = ResolverThatReturnsPropertyWrongTypeClassImpl;
      var result = resolverThatReturnsPropertyWrongTypeClass.ExportAsString();
      Assert.ThrowsAny<Exception>(() => ResolverThatReturnsPropertyWrongTypeClass.Import(XElement.Parse(result)));
    }
    #endregion


    #region ResolverWithIndexCheck
    public class ResolverWithIndexCheck : zenonSerializable<ResolverWithIndexCheck>
    {
      [zenonSerializableNode(nameof(SimpleList), typeof(ResolverWithIndexCheckResolver), NodeOrder = 0)]
      public List<string> SimpleList { get; set; }

      [zenonSerializableNode(nameof(SimpleStringA), typeof(ResolverWithIndexCheckResolver), NodeOrder = 1)]
      public string SimpleStringA { get; set; }

      [zenonSerializableNode(nameof(SimpleStringB), typeof(ResolverWithIndexCheckResolver), NodeOrder = 2)]
      public string SimpleStringB { get; set; }
    }

    private class ResolverWithIndexCheckResolver : IZenonSerializableResolver
    {
      public string GetNodeNameForSerialization(PropertyInfo targetProperty, Type targetType, object value, int index)
      {
        var attribute = targetProperty.GetCustomAttribute<zenonSerializableNodeAttribute>();
        if (targetProperty.Name == attribute.NodeName)
        {
          // Each test object stores the expected index
          if (!(value is IEnumerable<string>))
          {
            Assert.Equal(value, index.ToString());
          }
          return attribute.NodeName;
        }

        return null;
      }

      public Type GetTypeForDeserialization(PropertyInfo targetProperty, string nodeName, XElement node, int index)
      {
        if (nodeName != targetProperty.Name)
        {
          // This is a different property
          return null;
        }

        var genericTypeArgs = targetProperty.PropertyType.GenericTypeArguments;
        if (genericTypeArgs.Length > 0)
        {
          // In our test, the list elements contain the index of the node itself, which makes it easy to check them
          Assert.Equal(node.Value, index.ToString());
          return genericTypeArgs[0];
        }

        // Index must be 0 at this point
        Assert.Equal(0, index);
        return targetProperty.PropertyType;
      }
    }

    private static ResolverWithIndexCheck ResolverWithIndexCheckImpl => new ResolverWithIndexCheck()
    {
      SimpleList = new List<string>
      {
        "0",
        "1",
        "2"
      },
      SimpleStringA = "0",
      SimpleStringB = "0"
    };

    [Fact(DisplayName = "Resolver de-/serialization index valid")]
    public void TestResolverWithIndexCheck()
    {
      var resolverWithIndexCheck = ResolverWithIndexCheckImpl;

      // Asserts are done in the resolver
      string result = resolverWithIndexCheck.ExportAsString();
      ResolverWithIndexCheck.Import(XElement.Parse(result));
    }
    #endregion
  }
}
