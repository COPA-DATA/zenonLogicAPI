using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests.Serialization.zenonSerializable
{
  public class AllInOne
  {

    #region AllInOne

    public class AllInOneClass: zenonSerializable<AllInOneClass>
    {
      [zenonSerializableNode(nameof(SimpleSingleSerializationClass), typeof(AllInOneResolver), NodeOrder = 40)]
      public SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass SimpleSingleSerializationClass { get; set; }

      [zenonSerializableNode(nameof(SimpleSingleSerializationEncapsulateFalseClasses), typeof(AllInOneResolver), EncapsulateChildsIfList = false, NodeOrder = 20)]
      public List<SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass> SimpleSingleSerializationEncapsulateFalseClasses { get; set; }

      [zenonSerializableNode(nameof(SimpleSingleSerializationEncapsulateTrueClasses), typeof(AllInOneResolver), EncapsulateChildsIfList = true, NodeOrder = 10)]
      public List<SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass> SimpleSingleSerializationEncapsulateTrueClasses { get; set; }

    }

    public class AllInOneResolver: IZenonSerializableResolver
    {
      public string GetNodeNameForSerialization(PropertyInfo targetProperty, Type targetType, object value, int index)
      {
        return targetProperty.Name + "_" + index;
      }

      public Type GetTypeForDeserialization(string nodeName, int index)
      {
        return typeof(AllInOneClass).GetProperty(nodeName.Substring(0, nodeName.IndexOf("_"))).PropertyType;
      }
    }

    public AllInOneClass AllInOneClassImpl => new AllInOneClass
    {
      SimpleSingleSerializationClass = SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
      SimpleSingleSerializationEncapsulateFalseClasses = new List<SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass>
      {
        SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
        SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
        SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
        SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
        SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl
      },
      SimpleSingleSerializationEncapsulateTrueClasses = new List<SimpleSerializationWithAttributes.SimpleSerializationWithAttributesClass>
      {
        SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
        SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
        SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
        SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl,
        SimpleSerializationWithAttributes.SimpleSerializationWithAttributesImpl
      },

    };

    [Fact]
    public void TestAllInOneClassToString()
    {
      // Arrange
      AllInOneClass allInOneClass = AllInOneClassImpl;

      // Apply

      string result = allInOneClass.ExportAsString();

      // Assert

      Assert.Equal(zenonSerializableTestXmlComparison.AllInOneClass, result);
    }
    
    [Fact]
    public void TestAllInOneClassToXElement()
    {
      // Arrange
      AllInOneClass allInOneClass = AllInOneClassImpl;

      // Apply

      XElement result = allInOneClass.ExportAsXElement();

      // Assert

      Assert.True(XNode.DeepEquals(XElement.Parse(zenonSerializableTestXmlComparison.AllInOneClass), result));
    }
    #endregion

  }
}
