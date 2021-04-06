using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public partial class Nullables
  {
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    private class NullablesInListsClass : zenonSerializable<NullablesInListsClass>
    {
      public override string NodeName => "Test";

      [zenonSerializableNode("DefaultList1")]
      public List<int?> DefaultList1 { get; set; }

      [zenonSerializableNode("NestedList1", EncapsulateChildsIfList = true)]
      public List<int?> NestedList1 { get; set; }

      [zenonSerializableNode("DefaultList2")]
      public List<TestEnum?> DefaultList2 { get; set; }

      [zenonSerializableNode("NestedList2", EncapsulateChildsIfList = true)]
      public List<TestEnum?> NestedList2 { get; set; }
    }
    // ReSharper restore UnusedAutoPropertyAccessor.Local

    private static NullablesInListsClass NullablesInListsImpl => new NullablesInListsClass
    {
      DefaultList1 = new List<int?> { null, 1, null, 2, null, 3 },
      NestedList1 = new List<int?> { null, 1, null, 2, null, 3 },
      DefaultList2 = new List<TestEnum?> { null, TestEnum.ValueA, null, TestEnum.ValueB, null, TestEnum.ValueC },
      NestedList2 = new List<TestEnum?> { null, TestEnum.ValueA, null, TestEnum.ValueB, null, TestEnum.ValueC }
    };

    [Fact(DisplayName = "Nullables within nested/non-nested lists")]
    public void TestNullableInLists()
    {
      var impl = NullablesInListsImpl;
      var result = impl.ExportAsString();

      Assert.Equal(ComparisonValues.NullablesInList, result);

      var deserialized = NullablesInListsClass.Import(XElement.Parse(result));

      // We do not serialize null items by intent, therefore we need to exclude those from the source list before
      // comparing them with the deserialized value:
      impl.DefaultList1.RemoveAll(x => x == null);
      impl.DefaultList2.RemoveAll(x => x == null);
      impl.NestedList1.RemoveAll(x => x == null);
      impl.NestedList2.RemoveAll(x => x == null);

      Assert.True(impl.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
  }
}
