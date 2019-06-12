using System;
using zenonApi.Logic.Resources;

namespace zenonApi.Logic
{
  /// <summary>
  /// Describes the X, Y, Z dimensions of a zenon Logic array variable dimension
  /// </summary>
  public class LogicDimension
  {
    internal LogicDimension() { }

    public static LogicDimension Create(uint x, uint y = 0, uint z = 0)
    {
      // [0,x,x] -> invalid configuration
      if (x == 0 && y != 0)
      {
        throw new FormatException(string.Format(Strings.DimensionTypeInvalidFormat, nameof(LogicDimension), $"[{x},{y},{z}]"));
      }

      // [0,0,x], [x,0,x], [0,x,x] -> invalid configuration
      if ((x == 0 || y == 0) && z != 0)
      {
        throw new FormatException(string.Format(Strings.DimensionTypeInvalidFormat, nameof(LogicDimension), $"[{x},{y},{z}]"));
      }

      return new LogicDimension
      {
        X = x,
        Y = y,
        Z = z
      };
    }

    /// <summary>
    /// First dimension which specifies a one dimensional array variable
    /// </summary>
    public uint X { get; set; } = 0;

    /// <summary>
    /// Second dimension which specifies a two dimensional array variable
    /// </summary>
    public uint Y { get; set; } = 0;

    /// <summary>
    /// Third dimension which specifies a three dimension array variable
    /// </summary>
    public uint Z { get; set; } = 0;
  }
}
