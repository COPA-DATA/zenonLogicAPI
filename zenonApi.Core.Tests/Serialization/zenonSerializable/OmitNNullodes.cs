using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests.Serialization.zenonSerializable
{
  public class OmitNNullodes
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
      // Arrange

      OmitPrimitivesWithoutOmitClass omitPrimitivesClass = OmitPrimitivesWithoutOmitClassImpl;
      omitPrimitivesClass.NullInteger = null;

      // Apply

      string result = omitPrimitivesClass.ExportAsString();

      // Assert

      Assert.Equal(zenonSerializableTestXmlComparison.OmitPrimitivesWithoutOmit, result);

    }
    
    [Fact]
    public void TestOmitPrimitivesWithoutOmitToXElement()
    {
      // Arrange

      OmitPrimitivesWithoutOmitClass omitPrimitivesClass = OmitPrimitivesWithoutOmitClassImpl;
      omitPrimitivesClass.NullInteger = null;

      // Apply

      XElement result = omitPrimitivesClass.ExportAsXElement();

      // Assert

      Assert.True(XNode.DeepEquals(XElement.Parse(zenonSerializableTestXmlComparison.OmitPrimitivesWithoutOmit), result));

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
      // Arrange

      OmitPrimitivesWithOmitClass omitPrimitivesClass = OmitPrimitivesWithOmitClassImpl;
      omitPrimitivesClass.NullInteger = null;

      // Apply

      string result = omitPrimitivesClass.ExportAsString();

      // Assert

      Assert.Equal(zenonSerializableTestXmlComparison.OmitPrimitivesWithOmit, result);

    }

    [Fact]
    public void TestOmitPrimitivesWithOmitToXElement()
    {
      // Arrange

      OmitPrimitivesWithOmitClass omitPrimitivesClass = OmitPrimitivesWithOmitClassImpl;
      omitPrimitivesClass.NullInteger = null;

      // Apply

      XElement result = omitPrimitivesClass.ExportAsXElement();

      // Assert

      Assert.True(XNode.DeepEquals(XElement.Parse(zenonSerializableTestXmlComparison.OmitPrimitivesWithOmit), result));

    }

    #endregion

    #region OmitComplexWithoutOmit

    public class OmitComplexWithoutOmitClass: zenonSerializable<OmitComplexWithoutOmitClass>
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
      // Arrange
      OmitComplexWithoutOmitClass omitComplexWithoutOmitClass = OmitComplexWithoutOmitClassImpl;
      omitComplexWithoutOmitClass.NullSimpleSingleSerializationClass = null;

      // Apply

      string result = omitComplexWithoutOmitClass.ExportAsString();

      // Assert

      Assert.Equal(zenonSerializableTestXmlComparison.OmitComplexWithoutOmit, result);

    }

    [Fact]
    public void TestOmitComplexWithoutOmitToXElement()
    {
      // Arrange
      OmitComplexWithoutOmitClass omitComplexWithoutOmitClass = OmitComplexWithoutOmitClassImpl;
      omitComplexWithoutOmitClass.NullSimpleSingleSerializationClass = null;

      // Apply

      XElement result = omitComplexWithoutOmitClass.ExportAsXElement();

      // Assert

      Assert.True(XNode.DeepEquals(XElement.Parse(zenonSerializableTestXmlComparison.OmitComplexWithoutOmit), result));

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
      // Arrange
      OmitComplexWithOmitClass omitComplexWithoutOmitClass = OmitComplexWithOmitClassImpl;
      omitComplexWithoutOmitClass.NullSimpleSingleSerializationClass = null;

      // Apply

      string result = omitComplexWithoutOmitClass.ExportAsString();

      // Assert

      Assert.Equal(zenonSerializableTestXmlComparison.OmitComplexWithOmit, result);

    }

    [Fact]
    public void TestOmitComplexWithOmitToXElement()
    {
      // Arrange
      OmitComplexWithOmitClass omitComplexWithoutOmitClass = OmitComplexWithOmitClassImpl;
      omitComplexWithoutOmitClass.NullSimpleSingleSerializationClass = null;

      // Apply

      XElement result = omitComplexWithoutOmitClass.ExportAsXElement();

      // Assert

      Assert.True(XNode.DeepEquals(XElement.Parse(zenonSerializableTestXmlComparison.OmitComplexWithOmit), result));

    }


    #endregion


  }
}
