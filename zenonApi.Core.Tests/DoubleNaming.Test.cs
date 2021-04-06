using System;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class DoubleNaming
  {
    #region DoubleZenonSerializableNode 
    public class DoubleZenonSerializableNode : zenonSerializable<DoubleZenonSerializableNode>
    {
      [zenonSerializableNode(nameof(SimpleInteger))]
      public int SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleInteger))]
      public double SimpleDouble { get; set; }
    }

    public static DoubleZenonSerializableNode DoubleZenonSerializableNodeImpl => new DoubleZenonSerializableNode
    {
      SimpleInteger = 5,
      SimpleDouble = 12.25
    };

    [Fact(DisplayName = "Throw on duplicate node names")]
    public void TestDoubleZenonSerializableNode()
    {
      var doubleZenonSerializableNode = DoubleZenonSerializableNodeImpl; 
      Assert.ThrowsAny<Exception>(() => doubleZenonSerializableNode.ExportAsString());
    }
    #endregion


    #region DoubleZenonSerializableAttribute
    public class DoubleZenonSerializableAttribute : zenonSerializable<DoubleZenonSerializableAttribute>
    {
      [zenonSerializableAttribute(nameof(SimpleInteger))]
      public int SimpleInteger { get; set; }

      [zenonSerializableAttribute(nameof(SimpleInteger))]
      public double SimpleDouble { get; set; }
      
      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }
    }

    public static DoubleZenonSerializableAttribute DoubleZenonSerializableAttributeImpl =
      new DoubleZenonSerializableAttribute
      {
        SimpleInteger = 1,
        SimpleDouble = 12.3,
        SimpleString = "Abc"
      };

    [Fact(DisplayName = "Throw on duplicate attribute names")]
    public void TestDoubleZenonSerializableAttribute()
    {
      var doubleZenonSerializableAttribute = DoubleZenonSerializableAttributeImpl;
      Assert.ThrowsAny<Exception>(() => doubleZenonSerializableAttribute.ExportAsString());
    }
    #endregion


    #region DoubleZenonSerializableNode 
    public class ValidDoubleNamingClass : zenonSerializable<ValidDoubleNamingClass>
    {
      [zenonSerializableNode("A")]
      public int A1 { get; set; }

      [zenonSerializableNode("B")]
      public double B1 { get; set; }

      [zenonSerializableAttribute("A")]
      public int A2 { get; set; }

      [zenonSerializableAttribute("B")]
      public double B2 { get; set; }
    }

    public static ValidDoubleNamingClass ValidDoubleNamingImpl => new ValidDoubleNamingClass()
    {
      A1 = 5,
      B1 = 12.25,
      A2 = 6,
      B2 = 13.25
    };

    [Fact(DisplayName = "Allows same attribute and node name")]
    public void TestValidDoubleNamingImpl()
    {
      var impl = ValidDoubleNamingImpl;
      var result = impl.ExportAsString();
      var deserialized = ValidDoubleNamingClass.Import(XElement.Parse(result));

      Assert.True(impl.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion
  }
}
