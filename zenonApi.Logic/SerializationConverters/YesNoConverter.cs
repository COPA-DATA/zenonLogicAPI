using System;
using zenonApi.Serialization;

namespace zenonApi.Logic.SerializationConverters
{
  internal class YesNoConverter : IZenonSerializationConverter
  {
    public string Convert(object source)
    {
      if (source is bool yes)
      {
        if (yes)
        {
          return "YES";
        }
        return "NO";
      }

      return null;
    }

    public object Convert(string source)
    {
      if (string.IsNullOrWhiteSpace(source))
      {
        return false;
      }
      else if (source.Equals("YES", StringComparison.OrdinalIgnoreCase))
      {
        return true;
      }

      return false;
    }
  }
}
