using System;
using zenonApi.Core;

namespace zenonApi.Logic.Converters
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
      }

      return "NO";
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

      // TODO: Maybe throw exceptions if not a valid value?

      return false;
    }
  }
}
