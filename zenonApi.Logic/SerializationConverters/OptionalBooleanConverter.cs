using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Serialization;

namespace zenonApi.Logic.SerializationConverters
{
  public class OptionalBooleanConverter : IZenonSerializationConverter
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

      return null;
    }

    public object Convert(string source)
    {
      if (source == "1")
      {
        return true;
      }

      return false;
    }
  }
}
