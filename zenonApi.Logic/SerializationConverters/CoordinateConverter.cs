using System;
using zenonApi.Logic.Resources;
using zenonApi.Serialization;

namespace zenonApi.Logic.SerializationConverters
{
  internal class CoordinateConverter : IZenonSerializationConverter
  {
    public string Convert(object source)
    {
      if (source == null)
      {
        return null;
      }

      if (source is ValueTuple<int, int, int> coords)
      {
        return $"{coords.Item1},{coords.Item2},{coords.Item3}";
      }

      throw new FormatException(string.Format(Strings.CoordConverterFormatExcp, nameof(CoordinateConverter)));
    }

    public object Convert(string source)
    {
      if (string.IsNullOrWhiteSpace(source))
      {
        return null;
      }

      var str = source;
      var splitString = str.Split(',');

      if (splitString.Length > 3)
      {
        throw new ArgumentOutOfRangeException(Strings.CoordConverterArgumOutOfRangeExcp);
      }

      int xCoord = 0, yCoord = 0, zCoord = 0;

      var success = int.TryParse(splitString[0], out xCoord);
      if (splitString.Length > 1)
      {
        success &= int.TryParse(splitString[1], out yCoord);
      }
      if (splitString.Length > 2)
      {
        success &= int.TryParse(splitString[2], out zCoord);
      }

      if (!success)
      {
        throw new FormatException(Strings.CoordConverterParseException);
      }

      return (xCoord, yCoord, zCoord);
    }
  }
}
