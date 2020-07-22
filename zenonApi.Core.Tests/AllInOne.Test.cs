using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class AllInOne
  {
    #region AllInOne
    public class AllInOneClass : zenonSerializable<AllInOneClass>
    {
      [zenonSerializableNode(nameof(SimpleSingleSerializationClass), typeof(AllInOneResolver), NodeOrder = 40)]
      public SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass SimpleSingleSerializationClass { get; set; }

      [zenonSerializableNode(
        nameof(SimpleSingleSerializationEncapsulateFalseClasses),
        typeof(AllInOneListResolver),
        EncapsulateChildsIfList = false,
        NodeOrder = 20)]
      public List<SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass> SimpleSingleSerializationEncapsulateFalseClasses { get; set; }

      [zenonSerializableNode(
        nameof(SimpleSingleSerializationEncapsulateTrueClasses),
        typeof(AllInOneListResolver),
        EncapsulateChildsIfList = true,
        NodeOrder = 10)]
      public List<SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass> SimpleSingleSerializationEncapsulateTrueClasses { get; set; }

      [zenonSerializableNode(nameof(EnumSerializationAttrEnum))]
      public EnumSerialization.EnumSerializationEnum EnumSerializationAttrEnum { get; set; }

      [zenonSerializableAttribute(nameof(EnumSerializationEnum))]
      public EnumSerialization.EnumSerializationEnum EnumSerializationEnum { get; set; }
    }

    public class AllInOneListResolver : IZenonSerializableResolver
    {
      public string GetNodeNameForSerialization(PropertyInfo targetProperty, Type targetType, object value, int index)
      {
        return targetProperty.Name + "_" + index;
      }

      public Type GetTypeForDeserialization(PropertyInfo targetProperty, string nodeName, int index)
      {
        var removeFrom = nodeName.IndexOf("_", StringComparison.InvariantCulture);
        if (removeFrom == -1)
        {
          return null;
        }

        var name = nodeName.Substring(0, removeFrom);
        if (name != targetProperty.Name)
        {
          return null;
        }

        return targetProperty.PropertyType.GenericTypeArguments[0];
      }
    }

    public class AllInOneResolver : IZenonSerializableResolver
    {
      public string GetNodeNameForSerialization(PropertyInfo targetProperty, Type targetType, object value, int index)
      {
        return targetProperty.Name + "_" + index;
      }

      public Type GetTypeForDeserialization(PropertyInfo targetProperty, string nodeName, int index)
      {
        var removeFrom = nodeName.IndexOf("_", StringComparison.InvariantCulture);
        if (removeFrom == -1)
        {
          return null;
        }

        var name = nodeName.Substring(0, removeFrom);
        if (name != targetProperty.Name)
        {
          return null;
        }

        return targetProperty.PropertyType;
      }
    }

    public AllInOneClass AllInOneClassImpl => new AllInOneClass
    {
      SimpleSingleSerializationClass = SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
      SimpleSingleSerializationEncapsulateFalseClasses
        = new List<SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass>
          {
            SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
            SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
            SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
            SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
            SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl
          },
      SimpleSingleSerializationEncapsulateTrueClasses
        = new List<SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass>
          {
            SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
            SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
            SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
            SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
            SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl
          },
      EnumSerializationEnum = EnumSerialization.EnumSerializationEnum.Abc
    };

    [Fact]
    public void TestAllInOneClassToString()
    {
      var allInOneClass = AllInOneClassImpl;
      var result = allInOneClass.ExportAsString();
      Assert.Equal(ComparisonValues.AllInOneClass, result);
    }

    [Fact]
    public void TestAllInOneClassToXElement()
    {
      var allInOneClass = AllInOneClassImpl;
      var result = allInOneClass.ExportAsXElement();

      var withoutXmlHeader = XDocument.Parse(ComparisonValues.AllInOneClass).Root;
      Assert.True(XNode.DeepEquals(withoutXmlHeader, result));

      var deserialized = AllInOneClass.Import(result);

      Assert.True(allInOneClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion
  }
}
