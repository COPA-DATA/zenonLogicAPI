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
      var omitPrimitivesClass = OmitPrimitivesWithoutOmitClassImpl;
      omitPrimitivesClass.NullInteger = null;

      var result = omitPrimitivesClass.ExportAsString();
      Assert.Equal(ComparisonValues.OmitPrimitivesWithoutOmit, result);

      var deserialized = OmitPrimitivesWithoutOmitClass.Import(XElement.Parse(result));
      Assert.True(omitPrimitivesClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestOmitPrimitivesWithoutOmitToXElement()
    {
      var omitPrimitivesClass = OmitPrimitivesWithoutOmitClassImpl;
      omitPrimitivesClass.NullInteger = null;

      var result = omitPrimitivesClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.OmitPrimitivesWithoutOmit), result));

      var deserialized = OmitPrimitivesWithoutOmitClass.Import(result);
      Assert.True(omitPrimitivesClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
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
      var omitPrimitivesClass = OmitPrimitivesWithOmitClassImpl;
      omitPrimitivesClass.NullInteger = null;

      var result = omitPrimitivesClass.ExportAsString();
      Assert.Equal(ComparisonValues.OmitPrimitivesWithOmit, result);

      var deserialized = OmitPrimitivesWithOmitClass.Import(XElement.Parse(result));
      Assert.True(omitPrimitivesClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestOmitPrimitivesWithOmitToXElement()
    {
      var omitPrimitivesClass = OmitPrimitivesWithOmitClassImpl;
      omitPrimitivesClass.NullInteger = null;

      var result = omitPrimitivesClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.OmitPrimitivesWithOmit), result));

      var deserialized = OmitPrimitivesWithOmitClass.Import(result);
      Assert.True(omitPrimitivesClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
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
      var omitComplexWithoutOmitClass = OmitComplexWithoutOmitClassImpl;
      omitComplexWithoutOmitClass.NullSimpleSingleSerializationClass = null;

      var result = omitComplexWithoutOmitClass.ExportAsString();
      Assert.Equal(ComparisonValues.OmitComplexWithoutOmit, result);

      var deserialized = OmitComplexWithoutOmitClass.Import(XElement.Parse(result));
      // Those cannot be the same, since if a type is not omitted when being null, then it is added to the XML.
      // Example: MyType { MyNestedType = null } --> <MyType><MyNestedType/></MyType>
      // Reading it back: MyNestedType will not be null, but an unmodified default instance of MyNestedType
      Assert.False(omitComplexWithoutOmitClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
      
      // Here, the NullSimpleSingleSerializationClass entry is also ignored, therefore the contents should match again.
      Assert.True(omitComplexWithoutOmitClass.DeepEquals(
        deserialized, 
        nameof(IZenonSerializable.ObjectStatus), 
        nameof(OmitComplexWithoutOmitClass.NullSimpleSingleSerializationClass)));
    }

    [Fact]
    public void TestOmitComplexWithoutOmitToXElement()
    {
      var omitComplexWithoutOmitClass = OmitComplexWithoutOmitClassImpl;
      omitComplexWithoutOmitClass.NullSimpleSingleSerializationClass = null;

      var result = omitComplexWithoutOmitClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.OmitComplexWithoutOmit), result));

      var deserialized = OmitComplexWithoutOmitClass.Import(result);
      // We know, that there is a default instance of the NullSimpleSingleSerializationClass, since it was not omitted
      // (see comment in the test case above). By explicitely setting it back to null, it should equal.
      deserialized.NullSimpleSingleSerializationClass = null;
      Assert.True(omitComplexWithoutOmitClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
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
      var omitComplexWithOmitClass = OmitComplexWithOmitClassImpl;
      omitComplexWithOmitClass.NullSimpleSingleSerializationClass = null;

      var result = omitComplexWithOmitClass.ExportAsString();
      Assert.Equal(ComparisonValues.OmitComplexWithOmit, result);

      var deserialized = OmitComplexWithOmitClass.Import(XElement.Parse(result));
      Assert.True(omitComplexWithOmitClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }

    [Fact]
    public void TestOmitComplexWithOmitToXElement()
    {
      var omitComplexWithOmitClass = OmitComplexWithOmitClassImpl;
      omitComplexWithOmitClass.NullSimpleSingleSerializationClass = null;

      var result = omitComplexWithOmitClass.ExportAsXElement();
      Assert.True(XNode.DeepEquals(XElement.Parse(ComparisonValues.OmitComplexWithOmit), result));

      var deserialized = OmitComplexWithOmitClass.Import(result);
      Assert.True(omitComplexWithOmitClass.DeepEquals(deserialized, nameof(IZenonSerializable.ObjectStatus)));
    }
    #endregion
  }
}
