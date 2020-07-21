using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class EnumSerialization
  {
    public enum EnumSerializationEnum
    {
      [zenonSerializableEnum("AbcTest")]
      Abc,
      [zenonSerializableEnum("DefTest")]
      Def,
      [zenonSerializableEnum("GhiTest")]
      Ghi
    }

    #region EnumSerializationAsNode
    public class EnumSerializationAsNodeClass : zenonSerializable<EnumSerializationAsNodeClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger))]
      public int SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleEnumSerializationEnum))]
      public EnumSerializationEnum SimpleEnumSerializationEnum { get; set; }

    }

    public static EnumSerializationAsNodeClass EnumSerializationAsNodeClassImpl => new EnumSerializationAsNodeClass
    {
      SimpleInteger = 1234,
      SimpleEnumSerializationEnum = EnumSerializationEnum.Abc
    };

    [Fact]
    public void TestEnumSerializationAsNodeToString()
    {
      EnumSerializationAsNodeClass enumSerializationAsNodeClass = EnumSerializationAsNodeClassImpl;
      string result = enumSerializationAsNodeClass.ExportAsString();
      Assert.Equal(ComparisonValues.EnumSerializationAsNode, result);
    }

    [Fact]
    public void TestEnumSerializationAsNodeToXElement()
    {
      EnumSerializationAsNodeClass enumSerializationAsNodeClass = EnumSerializationAsNodeClassImpl;
      XElement result = enumSerializationAsNodeClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.EnumSerializationAsNode), result));
    }
    #endregion


    #region EnumSerializationAsParameter
    public class EnumSerializationAsParameterClass : zenonSerializable<EnumSerializationAsParameterClass>
    {
      [zenonSerializableNode(nameof(SimpleInteger))]
      public int SimpleInteger { get; set; }

      [zenonSerializableAttribute(nameof(SimpleEnumSerializationEnum))]
      public EnumSerializationEnum SimpleEnumSerializationEnum { get; set; }

    }

    public static EnumSerializationAsParameterClass EnumSerializationAsParameterClassImpl => new EnumSerializationAsParameterClass
    {
      SimpleInteger = 1234,
      SimpleEnumSerializationEnum = EnumSerializationEnum.Abc
    };

    [Fact]
    public void TestEnumSerializationAsParameterToString()
    {
      EnumSerializationAsParameterClass enumSerializationAsParameterClass = EnumSerializationAsParameterClassImpl;
      string result = enumSerializationAsParameterClass.ExportAsString();
      Assert.Equal(ComparisonValues.EnumSerializationAsParameter, result);
    }

    [Fact]
    public void TestEnumSerializationAsParameterToXElement()
    {
      EnumSerializationAsParameterClass enumSerializationAsParameterClass = EnumSerializationAsParameterClassImpl;
      XElement result = enumSerializationAsParameterClass.ExportAsXElement();

      XElement withoutXmlHeader = XDocument.Parse(ComparisonValues.EnumSerializationAsParameter).Root;
      Assert.True(XNode.DeepEquals(withoutXmlHeader, result));
    }
    #endregion
  }
}
