using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class Nullables
  {
    private class NullablePrimitives
    {
      [zenonSerializableNode("NodeNotNull")]
      public int? NodeNotNull { get; set; }

      [zenonSerializableNode("NodeNull")]
      public int? NodeNull { get; set; }

      [zenonSerializableNode("AttributeNotNull")]
      public int? AttributeNotNull { get; set; }

      [zenonSerializableNode("AttributeNull")]
      public int? AttributeNull { get; set; }
    }

    private enum TestEnum
    {
      ValueA,
      ValueB,
      ValueC
    }

    private class NullableEnums
    {
      
    }

    private class NullableItemsInList
    {

    }

    private class NullableItemsInNestedList
    {

    }
  }
}
