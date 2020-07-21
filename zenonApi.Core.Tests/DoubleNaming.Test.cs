using System;
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

    [Fact]
    public void TestDoubleZenonSerializableNode()
    {
      DoubleZenonSerializableNode doubleZenonSerializableNode = DoubleZenonSerializableNodeImpl; 
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

    [Fact]
    public void TestDoubleZenonSerializableAttribute()
    {
      DoubleZenonSerializableAttribute doubleZenonSerializableAttribute = DoubleZenonSerializableAttributeImpl;
      Assert.ThrowsAny<Exception>(() => doubleZenonSerializableAttribute.ExportAsString());
    }
    #endregion
  }
}
