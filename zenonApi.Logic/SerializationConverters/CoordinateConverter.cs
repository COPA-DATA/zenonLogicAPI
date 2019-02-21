using System;
using zenonApi.Serialization;

namespace zenonApi.Logic.SerializationConverters
{
  internal class CoordinateConverter : IZenonSerializationConverter
  {
    public string Convert(object source)
    {
      //TODO: check order in which zenon writes the Dim1 - Dim3

      if (source is Tuple<int, int, int> coords)
      {
        return $"{coords.Item1}{coords.Item2}{coords.Item3}";
      }

      return "0,0,0";
    }

    public object Convert(string source)
    {
      if (string.IsNullOrWhiteSpace(source))
      {
        return new Tuple<int, int, int>(0, 0, 0);
      }

      var str = source;
      var splitString = str.Split(';');

      if (splitString.Length > 3)
      {
        //TODO: Exception or retorn value with something like 0,0,0?
        throw new ArgumentOutOfRangeException();
      }

      var success = int.TryParse(splitString[0], out int xCoord);
      success &= int.TryParse(splitString[1], out int yCoord);
      success &= int.TryParse(splitString[2], out int zCoord);

      if (!success)
      {
        //TODO: Exception or retorn value with something like 0,0,0?
        throw new FormatException();
      }

      return new Tuple<int, int, int>(xCoord, yCoord, zCoord);
    }
  }
}
