using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public partial class Unknowns
  {
    public class UnknownContentClass : zenonSerializable<UnknownContentClass>
    {
      public override string NodeName => "Test";

      [zenonSerializableNode("KnownEntity")]
      public IEnumerable<UnknownContentChildClass> KnownEntities { get; set; }

      [zenonSerializableNode("OtherKnownEntity")]
      public UnknownContentChildClass OtherKnownEntity { get; set; }
    }

    public class UnknownContentChildClass : zenonSerializable<UnknownContentChildClass>
    {
      public override string NodeName => "Child";

      [zenonSerializableAttribute("KnownAttribute")]
      public int KnownAttribute { get; set; }

      [zenonSerializableNode("KnownNode")]
      public int KnownNode { get; set; }
    }

    [Fact(DisplayName = "Unknown node content in top-level/nested nodes")]
    public void TestUnknownContent()
    {
      var deserialized = UnknownContentClass.Import(XElement.Parse(ComparisonValues.UnknownContent));

      // Check of the top level node
      Assert.NotNull(deserialized.KnownEntities);
      Assert.Equal(2, deserialized.KnownEntities.Count());
      Assert.NotNull(deserialized.OtherKnownEntity);

      Assert.Empty(deserialized.UnknownNodes);
      Assert.Empty(deserialized.UnknownAttributes);
      Assert.NotNull(deserialized.UnknownNodeContent);
      Assert.Equal("UnknownContent1", deserialized.UnknownNodeContent.Trim());

      // Check of expected unknown nodes in childs
      var first = deserialized.KnownEntities.First();
      var second = deserialized.KnownEntities.Skip(1).First();
      var third = deserialized.OtherKnownEntity;
      
      Assert.Null(first.UnknownNodeContent);
      Assert.Equal("UnknownContent2", second.UnknownNodeContent.Trim());
      Assert.Null(third.UnknownNodeContent);

      // Check if nothing else broke
      Assert.Empty(first.UnknownAttributes);
      Assert.Empty(first.UnknownNodes);
      Assert.Equal(17, first.KnownNode);
      Assert.Equal(18, first.KnownAttribute);

      Assert.Empty(second.UnknownAttributes);
      Assert.Empty(second.UnknownNodes);
      Assert.Equal(19, second.KnownNode);
      Assert.Equal(20, second.KnownAttribute);

      Assert.True(third.UnknownAttributes.Count == 1);
      Assert.Empty(third.UnknownNodes);
      Assert.Equal(21, third.KnownNode);
      Assert.Equal(22, third.KnownAttribute);
    }
  }
}
