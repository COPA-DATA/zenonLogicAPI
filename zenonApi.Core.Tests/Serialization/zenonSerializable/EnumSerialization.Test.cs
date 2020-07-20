using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests.Serialization.zenonSerializable
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
      // Arrange

      EnumSerializationAsNodeClass enumSerializationAsNodeClass = EnumSerializationAsNodeClassImpl;

      // Apply

      string result = enumSerializationAsNodeClass.ExportAsString();

      // Assert

      Assert.Equal(zenonSerializableTestXmlComparison.EnumSerializationAsNode, result);

    }

    [Fact]
    public void TestEnumSerializationAsNodeToXElement()
    {
      // Arrange

      EnumSerializationAsNodeClass enumSerializationAsNodeClass = EnumSerializationAsNodeClassImpl;

      // Apply

      XElement result = enumSerializationAsNodeClass.ExportAsXElement();

      // Assert

      Assert.True(XNode.DeepEquals(XElement.Parse(zenonSerializableTestXmlComparison.EnumSerializationAsNode), result));

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
      // Arrange

      EnumSerializationAsParameterClass enumSerializationAsParameterClass = EnumSerializationAsParameterClassImpl;

      // Apply

      string result = enumSerializationAsParameterClass.ExportAsString();

      // Assert

      Assert.Equal(zenonSerializableTestXmlComparison.EnumSerializationAsParameter, result);

    }

    [Fact]
    public void TestEnumSerializationAsParameterToXElement()
    {
      // Arrange

      EnumSerializationAsParameterClass enumSerializationAsParameterClass = EnumSerializationAsParameterClassImpl;

      // Apply

      XElement result = enumSerializationAsParameterClass.ExportAsXElement();

      // Assert

      Assert.True(XNode.DeepEquals(XElement.Parse(zenonSerializableTestXmlComparison.EnumSerializationAsParameter), result));

    }

    #endregion

  }
}
