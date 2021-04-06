using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public partial class Nullables
  {
    // ReSharper disable UnusedAutoPropertyAccessor.Local : Getters are used during tests, cannot be removed dear ReSharper.
    private class NullablePrimitivesClass : zenonSerializable<NullablePrimitivesClass>
    {
      public override string NodeName => "Test";

      [zenonSerializableNode("NodeNotNull")]
      public int? NodeNotNull { get; set; }

      [zenonSerializableNode("NodeNull")]
      public int? NodeNull { get; set; }

      [zenonSerializableNode("AttributeNotNull")]
      public int? AttributeNotNull { get; set; }

      [zenonSerializableNode("AttributeNull")]
      public int? AttributeNull { get; set; }

      [zenonSerializableNode("NodeEnumNull")]
      public TestEnum? NodeEnumNull { get; set; }

      [zenonSerializableNode("NodeEnumNotNull")]
      public TestEnum? NodeEnumNotNull { get; set; }

      [zenonSerializableNode("AttributeEnumNull")]
      public TestEnum? AttributeEnumNull { get; set; }

      [zenonSerializableNode("AttributeEnumNotNull")]
      public TestEnum? AttributeEnumNotNull { get; set; }
    }
    // ReSharper restore UnusedAutoPropertyAccessor.Local

    private static NullablePrimitivesClass NullablePrimitivesImpl => new NullablePrimitivesClass
    {
      NodeNotNull = 5,
      NodeNull = null,
      AttributeNotNull = 5,
      AttributeNull = null,
      NodeEnumNotNull = TestEnum.ValueB,
      NodeEnumNull = null,
      AttributeEnumNotNull = TestEnum.ValueB,
      AttributeEnumNull = null
    };

    private enum TestEnum
    {
      [zenonSerializableEnum("value_a")]
      ValueA,
      [zenonSerializableEnum("value_b")]
      ValueB,
      [zenonSerializableEnum("value_c")]
      ValueC
    }

    [Fact(DisplayName = "Nullable nodes and attributes")]
    public void TestNullablePrimitives()
    {
      var impl = NullablePrimitivesImpl;
      var result = impl.ExportAsString();

      Assert.Equal(ComparisonValues.NullablePrimitives, result);

      var deserialized = NullablePrimitivesClass.Import(XElement.Parse(result));
      Assert.True(impl.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
  }
}
