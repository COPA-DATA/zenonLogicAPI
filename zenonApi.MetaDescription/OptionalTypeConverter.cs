using System;
using zenonApi.Serialization;

namespace zenonApi.MetaDescription
{
  class OptionalTypeConverter : IZenonSerializationConverter
  {
    public string Convert(object source)
    {
      return source.ToString();
    }

    public object Convert(string source)
    {
      try
      {
        return Type.GetType(source);
      }
      catch
      {
        return null;
      }
    }
  }
}
