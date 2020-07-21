using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests.Serialization.zenonSerializable
{
  public class StatusEnum
  {

    public class StatusEnumClass: zenonSerializable<StatusEnumClass>
    {

      private int _simpleInteger;

      [zenonSerializableNode(nameof(SimpleInteger))]
      public int SimpleInteger
      {
        get => this._simpleInteger;
        set
        {
          this._simpleInteger = value;
          this.OnPropertyChanged();
        }
      }

      [zenonSerializableNode(nameof(SimpleDouble))]
      public double SimpleDouble { get; set; }

      [zenonSerializableNode(nameof(SimpleString))]
      public string SimpleString { get; set; }
    }

    public static StatusEnumClass StatusEnumClassImpl =>
      new StatusEnumClass
      {
        SimpleInteger = 5,
        SimpleDouble = 5.3,
        SimpleString = "TestString"
      };


    #region New

    [Fact]
    public void StatusStatusEnumNew()
    {
      // Arrange Apply

      StatusEnumClass statusEnumClass = new StatusEnumClass();

      // Assert

      Assert.Equal(zenonSerializableStatusEnum.New, statusEnumClass.ObjectStatus);

    }

    #endregion


    #region Loaded

    [Fact]
    public void StatusStatusEnumLoaded()
    {
      StatusEnumClass statusEnumClass = StatusEnumClass.Import(XElement.Parse(zenonSerializableTestXmlComparison.StatusEnum)); 
      Assert.Equal(zenonSerializableStatusEnum.Loaded, statusEnumClass.ObjectStatus);
    }
    #endregion


    #region Modified

    [Fact]
    public void StatusStatusEnumModified()
    {
      // Arrange

      StatusEnumClass statusEnumClass = StatusEnumClassImpl;

      // Apply

      statusEnumClass.SimpleInteger = 1503;

      // Assert

      Assert.Equal(zenonSerializableStatusEnum.Modified, statusEnumClass.ObjectStatus);

    }

    #endregion


    #region Deserialized

    [Fact]
    public void StatusStatusEnumDeserialized()
    {
      // Arrange

      StatusEnumClass statusEnumClass = StatusEnumClassImpl;

      // Apply

      statusEnumClass.ExportAsString();

      // Assert

      Assert.Equal(zenonSerializableStatusEnum.Deserialized, statusEnumClass.ObjectStatus);

    }

    #endregion
  }
}
