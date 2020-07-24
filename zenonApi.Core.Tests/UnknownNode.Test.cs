using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public partial class Unknowns
  {
    public class UnknownNodesClass : zenonSerializable<UnknownNodesClass>
    {
      public override string NodeName => "Test";

      [zenonSerializableNode("KnownEntity")]
      public int KnownEntity { get; set; }
    }

    [Fact(DisplayName = "Unknown nodes in different positions, also nested")]
    public void TestUnknownNodes()
    {
      var deserialized = UnknownNodesClass.Import(XElement.Parse(ComparisonValues.UnknownNodes));
      
      Assert.Equal(5, deserialized.KnownEntity);
      
      // We have three types of unknown nodes in our sample XML, here we just check sizes:
      Assert.NotEmpty(deserialized.UnknownNodes);
      Assert.Equal(3, deserialized.UnknownNodes.Count);

      Assert.NotNull(deserialized.UnknownAttributes);
      Assert.Empty(deserialized.UnknownAttributes);

      Assert.Null(deserialized.UnknownNodeContent);

      // Check the amount of unknown nodes per type according to our sample XML:
      var unknownA = deserialized.UnknownNodes["UnknownSingle"];
      Assert.Single(unknownA);

      var unknownB = deserialized.UnknownNodes["UnknownMultiple"];
      Assert.Equal(2, unknownB.Count);

      var unknownC = deserialized.UnknownNodes["UnknownNested"];
      Assert.Single(unknownC);

      // Check if stored correctly per unknown node type:
      Assert.Equal("5.3", unknownA.First().Value);
      Assert.Equal("TestString1", unknownB.First().Value);
      Assert.Equal("TestString2", unknownB.Skip(1).First().Value);

      // Content of the nested one is not checked, since the mechanisms are the same, so if the first two are ok, this will also
      // (Whole node is assigned to the unknown node, so no need to distinguish it here)

      // Serialize again, deserialize, and see if still everything is the same
      var result = deserialized.ExportAsString();
      var reDeserialized = UnknownNodesClass.Import(XElement.Parse(result));

      // We cannot use our deep equals method here, since we use default comparisons for value types (like the tuple in the unknown nodes)
      // and for classes, which implement their own equals method (like Guid or XNode).
      // Therefore we need to check the XNode values on their own:
      foreach (var nodeCategoryPair in deserialized.UnknownNodes)
      {
        var nodeCategory = nodeCategoryPair.Value;
        var otherNodeCategory = reDeserialized.UnknownNodes[nodeCategoryPair.Key];

        Assert.Equal(nodeCategory.Count, otherNodeCategory.Count);

        IEnumerator otherEnumerator = otherNodeCategory.GetEnumerator();
        foreach (var node in nodeCategory)
        {
          Assert.True(otherEnumerator.MoveNext());
          Assert.True(XNode.DeepEquals(node, (XElement)otherEnumerator.Current));
        }
      }
    }
  }
}
