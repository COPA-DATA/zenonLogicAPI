using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class OmitNullNodes
  {
    #region OmitPrimitivesWithoutOmit
    public class OmitPrimitivesWithoutOmitClass : zenonSerializable<OmitPrimitivesWithoutOmitClass>
    {
      [zenonSerializableNode(nameof(NullInteger), OmitIfNull = false)]
      public int? NullInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleInteger))]
      public int SimpleInteger { get; set; }

    }

    public OmitPrimitivesWithoutOmitClass OmitPrimitivesWithoutOmitClassImpl => new OmitPrimitivesWithoutOmitClass
    {
      SimpleInteger = 12,
      NullInteger = 15
    };

    [Fact]
    public void TestOmitPrimitivesWithoutOmitToString()
    {
      OmitPrimitivesWithoutOmitClass omitPrimitivesClass = OmitPrimitivesWithoutOmitClassImpl;
      omitPrimitivesClass.NullInteger = null;

      string result = omitPrimitivesClass.ExportAsString();
      Assert.Equal(ComparisonValues.OmitPrimitivesWithoutOmit, result);
    }

    [Fact]
    public void TestOmitPrimitivesWithoutOmitToXElement()
    {
      OmitPrimitivesWithoutOmitClass omitPrimitivesClass = OmitPrimitivesWithoutOmitClassImpl;
      omitPrimitivesClass.NullInteger = null;

      XElement result = omitPrimitivesClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.OmitPrimitivesWithoutOmit), result));
    }
    #endregion


    #region OmitPrimitivesWithOmitClass
    public class OmitPrimitivesWithOmitClass : zenonSerializable<OmitPrimitivesWithOmitClass>
    {
      [zenonSerializableNode(nameof(NullInteger), OmitIfNull = true)]
      public int? NullInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleInteger))]
      public int SimpleInteger { get; set; }

    }

    public OmitPrimitivesWithOmitClass OmitPrimitivesWithOmitClassImpl => new OmitPrimitivesWithOmitClass
    {
      SimpleInteger = 12,
      NullInteger = 15
    };

    [Fact]
    public void TestOmitPrimitivesWithOmitToString()
    {
      OmitPrimitivesWithOmitClass omitPrimitivesClass = OmitPrimitivesWithOmitClassImpl;
      omitPrimitivesClass.NullInteger = null;

      string result = omitPrimitivesClass.ExportAsString();
      Assert.Equal(ComparisonValues.OmitPrimitivesWithOmit, result);
    }

    [Fact]
    public void TestOmitPrimitivesWithOmitToXElement()
    {
      OmitPrimitivesWithOmitClass omitPrimitivesClass = OmitPrimitivesWithOmitClassImpl;
      omitPrimitivesClass.NullInteger = null;

      XElement result = omitPrimitivesClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.OmitPrimitivesWithOmit), result));
    }
    #endregion


    #region OmitComplexWithoutOmit
    public class OmitComplexWithoutOmitClass : zenonSerializable<OmitComplexWithoutOmitClass>
    {
      [zenonSerializableNode(nameof(SimpleSingleSerializationClass))]
      public SimpleSingleSerialization.SimpleSingleSerializationClass SimpleSingleSerializationClass { get; set; }

      [zenonSerializableNode(nameof(NullSimpleSingleSerializationClass), OmitIfNull = false)]
      public SimpleSingleSerialization.SimpleSingleSerializationClass NullSimpleSingleSerializationClass { get; set; }
    }

    private OmitComplexWithoutOmitClass OmitComplexWithoutOmitClassImpl => new OmitComplexWithoutOmitClass
    {
      SimpleSingleSerializationClass = SimpleSingleSerialization.SimpleSingleSerializationImpl,
      NullSimpleSingleSerializationClass = SimpleSingleSerialization.SimpleSingleSerializationImpl
    };

    [Fact]
    public void TestOmitComplexWithoutOmitToString()
    {
      OmitComplexWithoutOmitClass omitComplexWithoutOmitClass = OmitComplexWithoutOmitClassImpl;
      omitComplexWithoutOmitClass.NullSimpleSingleSerializationClass = null;

      string result = omitComplexWithoutOmitClass.ExportAsString();
      Assert.Equal(ComparisonValues.OmitComplexWithoutOmit, result);
    }

    [Fact]
    public void TestOmitComplexWithoutOmitToXElement()
    {
      OmitComplexWithoutOmitClass omitComplexWithoutOmitClass = OmitComplexWithoutOmitClassImpl;
      omitComplexWithoutOmitClass.NullSimpleSingleSerializationClass = null;

      XElement result = omitComplexWithoutOmitClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.OmitComplexWithoutOmit), result));
    }
    #endregion


    #region OmitComplexWithOmit
    public class OmitComplexWithOmitClass : zenonSerializable<OmitComplexWithOmitClass>
    {
      [zenonSerializableNode(nameof(SimpleSingleSerializationClass))]
      public SimpleSingleSerialization.SimpleSingleSerializationClass SimpleSingleSerializationClass { get; set; }

      [zenonSerializableNode(nameof(NullSimpleSingleSerializationClass), OmitIfNull = true)]
      public SimpleSingleSerialization.SimpleSingleSerializationClass NullSimpleSingleSerializationClass { get; set; }
    }

    private OmitComplexWithOmitClass OmitComplexWithOmitClassImpl => new OmitComplexWithOmitClass
    {
      SimpleSingleSerializationClass = SimpleSingleSerialization.SimpleSingleSerializationImpl,
      NullSimpleSingleSerializationClass = SimpleSingleSerialization.SimpleSingleSerializationImpl
    };

    [Fact]
    public void TestOmitComplexWithOmitToString()
    {
      OmitComplexWithOmitClass omitComplexWithoutOmitClass = OmitComplexWithOmitClassImpl;
      omitComplexWithoutOmitClass.NullSimpleSingleSerializationClass = null;

      string result = omitComplexWithoutOmitClass.ExportAsString();
      Assert.Equal(ComparisonValues.OmitComplexWithOmit, result);
    }

    [Fact]
    public void TestOmitComplexWithOmitToXElement()
    {
      OmitComplexWithOmitClass omitComplexWithoutOmitClass = OmitComplexWithOmitClassImpl;
      omitComplexWithoutOmitClass.NullSimpleSingleSerializationClass = null;

      XElement result = omitComplexWithoutOmitClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.OmitComplexWithOmit), result));
    }
    #endregion
  }
}
