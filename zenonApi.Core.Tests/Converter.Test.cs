using System.Reflection;
using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class Converter
  {
    #region NullConverter
    public class ConverterSerializationConverterNull : zenonSerializable<ConverterSerializationConverterNull>
    {
      [zenonSerializableAttribute(nameof(SimpleInteger), Converter = null)]
      public int SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleDouble))]
      public double SimpleDouble { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }
    }

    public static ConverterSerializationConverterNull ConverterSerializationConverterNullImpl = new ConverterSerializationConverterNull
    {
      SimpleInteger = 5,
      SimpleDouble = 5.25,
      SimpleString = "HelloWorld"
    };

    [Fact]
    public void TestNullConverterException()
    {
      ConverterSerializationConverterNull converterSerializationImpl = ConverterSerializationConverterNullImpl;
      Assert.Throws<CustomAttributeFormatException>(() => converterSerializationImpl.ExportAsString());
    }
    #endregion


    #region WrongConverter
    public class ConverterSerializationConverterWrong : zenonSerializable<ConverterSerializationConverterWrong>
    {
      [zenonSerializableAttribute(nameof(SimpleInteger), Converter = null)]
      public int SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleDouble))]
      public double SimpleDouble { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }
    }

    public static ConverterSerializationConverterWrong ConverterSerializationConverterWrongImpl = new ConverterSerializationConverterWrong
    {
      SimpleInteger = 5,
      SimpleDouble = 5.25,
      SimpleString = "HelloWorld"
    };

    [Fact]
    public void TestWrongConverterException()
    {
      ConverterSerializationConverterWrong converterSerializationConverterWrongImpl = ConverterSerializationConverterWrongImpl;
      Assert.Throws<CustomAttributeFormatException>(() => converterSerializationConverterWrongImpl.ExportAsString());
    }
    #endregion


    #region TryUsageOfConverter
    public class TestConverterSerializationConverterUsage : zenonSerializable<TestConverterSerializationConverterUsage>
    {
      [zenonSerializableAttribute(nameof(SimpleInteger), Converter = typeof(SimpleTestConverter))]
      public int SimpleInteger { get; set; }

      [zenonSerializableNode(nameof(SimpleDouble))]
      public double SimpleDouble { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }
    }

    public class SimpleTestConverter : IZenonSerializationConverter
    {
      private const string Padding = "Test + ";

      public string Convert(object source)
      {
        return Padding + source;
      }

      public object Convert(string source)
      {
        return int.Parse(source.Substring(Padding.Length));
      }
    }

    public static TestConverterSerializationConverterUsage TestConverterSerializationConverterUsageImpl
      = new TestConverterSerializationConverterUsage
      {
        SimpleInteger = 5,
        SimpleDouble = 5.25,
        SimpleString = "HelloWorld"
      };

    [Fact]
    public void TestConverterFunctionality()
    {
      TestConverterSerializationConverterUsage testConverterSerializationConverterUsageImpl
        = TestConverterSerializationConverterUsageImpl;
      string result = testConverterSerializationConverterUsageImpl.ExportAsString();
      Assert.Equal(ComparisonValues.ConverterFunctionalityTest, result);
    }

    [Fact]
    public void TestConverterFunctionalityBack()
    {
      TestConverterSerializationConverterUsage testConverterSerializationConverterUsageImpl = TestConverterSerializationConverterUsageImpl;
      string result = testConverterSerializationConverterUsageImpl.ExportAsString();

      TestConverterSerializationConverterUsage backTransformation =
        TestConverterSerializationConverterUsage.Import(XElement.Parse(result));

      Assert.Equal(testConverterSerializationConverterUsageImpl.SimpleInteger, backTransformation.SimpleInteger);
    }
    #endregion
  }
}
