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
      if (source is LogicDimension dimension) // TODO: Why is the logic here outside the dimension? does not make any sense.
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

      throw new FormatException(string.Format(Strings.ErrorMessageDimensionsInvalidDatatype, nameof(DimensionConverter)));
    }

    public object Convert(string source)
    {
      if (string.IsNullOrWhiteSpace(source))
      {
        return null;
      }

      var parts = source.Split(',');

      if (parts.Length > MaximumSupportedDimensions)
      {
        throw new ArgumentOutOfRangeException(Strings.ErrorMessageDimensionsOutOfRange);
      }

      uint y = 0;
      uint z = 0;

      var success = uint.TryParse(parts[0], out uint x);
      if (parts.Length > 1)
      {
        success &= uint.TryParse(parts[1], out y);
      }
      if (parts.Length > 2)
      {
        success &= uint.TryParse(parts[2], out z);
      }

      if (!success)
      {
        throw new FormatException(string.Format(Strings.ErrorMessageParsingDimensionsFailed, source));
      }

      return LogicDimension.Create(x, y, z);
    }
  }
}
