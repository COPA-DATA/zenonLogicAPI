using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public partial class Unknowns
  {
    public class UnknownAttributesClass : zenonSerializable<UnknownAttributesClass>
    {
      public override string NodeName => "Test";

      [zenonSerializableNode("KnownEntity")]
      public IEnumerable<UnknownAttributesChildClass> KnownEntities { get; set; }
    }

    public class UnknownAttributesChildClass : zenonSerializable<UnknownAttributesChildClass>
    {
      public override string NodeName => "Child";

      [zenonSerializableAttribute("KnownValue")]
      public int KnownValue { get; set; }
    }

    [Fact(DisplayName = "Unknown attributes in different positions")]
    public void TestUnknownAttributes()
    {
      var deserialized = UnknownAttributesClass.Import(XElement.Parse(ComparisonValues.UnknownAttributes));

      Assert.NotNull(deserialized.KnownEntities);
      Assert.Equal(4, deserialized.KnownEntities.Count());

      Assert.Empty(deserialized.UnknownNodes);
      Assert.NotNull(deserialized.UnknownAttributes);
      Assert.True(deserialized.UnknownAttributes.Count == 1);
      Assert.Null(deserialized.UnknownNodeContent);

      // Check the amount of unknown nodes per type according to our sample XML:
      var unknownTopLevel = deserialized.UnknownAttributes.First();
      Assert.Equal("123", unknownTopLevel.Value);

      var consider = deserialized.KnownEntities.ToList();
      for (int i = 0; i < consider.Count; i++)
      {
        var child = consider[i];

        Assert.Empty(child.UnknownNodes);
        Assert.Null(child.UnknownNodeContent);
        Assert.Equal(5, child.KnownValue);

        if (i != consider.Count - 1)
        {
          Assert.True(child.UnknownAttributes.Count == 2);
          Assert.Equal("Hello", child.UnknownAttributes.First(x => x.Key == "Unknown1").Value);
          Assert.Equal("World", child.UnknownAttributes.First(x => x.Key == "Unknown2").Value);
        }
        else
        {
          Assert.Empty(child.UnknownAttributes);
        }
      }
    }
  }
}
