using System;
using zenonApi.Logic.Resources;
using zenonApi.Serialization;

namespace zenonApi.Logic.SerializationConverters
{
  /// <summary>
  /// Converts dimensions of zenon Logic array variables from string e.g. (0,0,0)
  /// to object of type <see cref="LogicDimension"/>
  /// </summary>
  internal class DimensionConverter : IZenonSerializationConverter
  {
    private const int MaximumSupportedDimensions = 3;

    public string Convert(object source)
    {
      if (source == null)
      {
        return null;
      }

      string dimensionString = null;
      if (source is LogicDimension dimension)
      {
        // [0,0,0] -> null so tag can be omitted in xml
        if (dimension.X == 0 && dimension.Y == 0 && dimension.Z == 0)
        {
          return null;
        }

        if (dimension.X != 0)
        {
          dimensionString += dimension.X.ToString();
        }

        // [0,x,x] -> invalid configuration
        if (dimension.X == 0 && dimension.Y != 0)
        {
          throw new FormatException(string.Format(Strings.DimensionTypeInvalidFormat,
            nameof(LogicDimension), $"[{dimension.X},{dimension.Y},{dimension.Z}]"));
        }

        if (dimension.Y != 0)
        {
          dimensionString += $",{dimension.Y}";
        }

        // [0,0,x], [x,0,x], [0,x,x] -> invalid configuration
        if ((dimension.X == 0 || dimension.Y == 0) && dimension.Z != 0)
        {
          throw new FormatException(string.Format(Strings.DimensionTypeInvalidFormat, 
            nameof(LogicDimension), $"[{dimension.X},{dimension.Y},{dimension.Z}]"));
        }

        if (dimension.Z != 0)
        {
          dimensionString += $",{dimension.Z}";
        }

        return dimensionString;
      }

      throw new FormatException(string.Format(Strings.DimensionConverterFormatExcp, nameof(DimensionConverter)));
    }

    public object Convert(string source)
    {
      if (string.IsNullOrWhiteSpace(source))
      {
        return null;
      }

      var str = source;
      var splitString = str.Split(',');

      if (splitString.Length > MaximumSupportedDimensions)
      {
        throw new ArgumentOutOfRangeException(Strings.DimensionConverterArgumOutOfRangeExcp);
      }

      uint xCoord = 0, yCoord = 0, zCoord = 0;

      var success = uint.TryParse(splitString[0], out xCoord);
      if (splitString.Length > 1)
      {
        success &= uint.TryParse(splitString[1], out yCoord);
      }
      if (splitString.Length > 2)
      {
        success &= uint.TryParse(splitString[2], out zCoord);
      }

      if (!success)
      {
        throw new FormatException(Strings.DimensionConverterParseException);
      }

      return LogicDimension.Create(xCoord, yCoord, zCoord);
    }
  }
}
