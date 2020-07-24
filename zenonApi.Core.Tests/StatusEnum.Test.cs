using System.Xml.Linq;
using Xunit;
using zenonApi.Serialization;

namespace zenonApi.Core.Tests
{
  public class StatusEnum
  {
    public class StatusEnumClass : zenonSerializable<StatusEnumClass>
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
    [Fact(DisplayName = nameof(IZenonSerializable.ObjectStatus) + " after creation")]
    public void StatusStatusEnumNew()
    {
      var statusEnumClass = new StatusEnumClass();
      Assert.Equal(zenonSerializableStatusEnum.New, statusEnumClass.ObjectStatus);
    }
    #endregion


    [Fact(DisplayName = nameof(IZenonSerializable.ObjectStatus) + " after load, modify, export")]
    public void StatusStatusEnumLoaded()
    {
      var statusEnumClass = StatusEnumClass.Import(XElement.Parse(ComparisonValues.StatusEnum));
      Assert.Equal(zenonSerializableStatusEnum.Loaded, statusEnumClass.ObjectStatus);

      statusEnumClass.SimpleInteger = 1503;
      Assert.Equal(zenonSerializableStatusEnum.Modified, statusEnumClass.ObjectStatus);

      var exported = statusEnumClass.ExportAsString();

      Assert.Equal(zenonSerializableStatusEnum.Deserialized, statusEnumClass.ObjectStatus);

      var imported = StatusEnumClass.Import(XElement.Parse(exported));
      Assert.True(statusEnumClass.DeepEquals(imported, "ObjectStatus"));
    }
  }
}
