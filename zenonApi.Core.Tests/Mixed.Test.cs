using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class Mixed
  {
    #region AllInOne
    public class MixedClass : zenonSerializable<MixedClass>
    {
      [zenonSerializableNode(nameof(SimpleSingleSerializationClass), typeof(MixedResolver), NodeOrder = 40)]
      public SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass SimpleSingleSerializationClass { get; set; }

      [zenonSerializableNode(
        nameof(SimpleSingleSerializationEncapsulateFalseClasses),
        typeof(MixedListResolver),
        EncapsulateChildsIfList = false,
        NodeOrder = 20)]
      public List<SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass> SimpleSingleSerializationEncapsulateFalseClasses { get; set; }

      [zenonSerializableNode(
        nameof(SimpleSingleSerializationEncapsulateTrueClasses),
        typeof(MixedListResolver),
        EncapsulateChildsIfList = true,
        NodeOrder = 10)]
      public List<SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass> SimpleSingleSerializationEncapsulateTrueClasses { get; set; }

      [zenonSerializableNode(nameof(EnumSerializationAttrEnum))]
      public EnumSerialization.EnumSerializationEnum EnumSerializationAttrEnum { get; set; }

      [zenonSerializableAttribute(nameof(EnumSerializationEnum))]
      public EnumSerialization.EnumSerializationEnum EnumSerializationEnum { get; set; }
    }

    public class MixedListResolver : IZenonSerializableResolver
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
        if (name != targetProperty.Name)
        {
          return null;
        }

        return targetProperty.PropertyType.GenericTypeArguments[0];
      }
    }

    public class MixedResolver : IZenonSerializableResolver
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
        if (name != targetProperty.Name)
        {
          return null;
        }

        return targetProperty.PropertyType;
      }
    }

    public MixedClass MixedClassImpl => new MixedClass
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

    [Fact(DisplayName = "Combination of various test cases (with string de-/serialization)")]
    public void TestMixedClassToString()
    {
      var impl = MixedClassImpl;
      var result = impl.ExportAsString();
      Assert.Equal(ComparisonValues.Mixed, result);
    }

    [Fact(DisplayName = "Combination of various test cases (with XElement de-/serialization)")]
    public void TestMixedClassToXElement()
    {
      var allInOneClass = MixedClassImpl;
      var result = allInOneClass.ExportAsXElement();

      var withoutXmlHeader = XDocument.Parse(ComparisonValues.Mixed).Root;
      Assert.True(XNode.DeepEquals(withoutXmlHeader, result));

      var deserialized = MixedClass.Import(result);

      Assert.True(allInOneClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion
  }
}
