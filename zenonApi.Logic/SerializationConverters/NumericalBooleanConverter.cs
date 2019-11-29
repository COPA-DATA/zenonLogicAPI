using System;
using zenonApi.Serialization;

namespace zenonApi.Logic.SerializationConverters
{
  internal class NumericalBooleanConverter : IZenonSerializationConverter
  {
    public string Convert(object source)
    {
      // This converter is only used by FunctionBlockDiagramLine.Negate
      if (source == null || !(source is IConvertible conv))
      {
        return null;
      }

      bool value = (bool)System.Convert.ChangeType(conv, typeof(bool));
      if (value)
      {
        return "1";
      }

      return "0";
    }

    public object Convert(string source)
    {
      if (source == "1" || (bool.TryParse(source, out bool result) && result))
      {
        return true;
      }

      return false;
    }
  }
}
