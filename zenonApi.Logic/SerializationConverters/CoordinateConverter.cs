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

      var success = int.TryParse(splitString[0], out int xCoord);
      success &= int.TryParse(splitString[1], out int yCoord);
      success &= int.TryParse(splitString[2], out int zCoord);

      if (!success)
      {
        throw new FormatException(Strings.CoordConverterParseException);
      }

      return new Tuple<int, int, int>(xCoord, yCoord, zCoord);
    }
  }
}
