using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using zenonApi.Core;
using zenonApi.Core.Extensions;

namespace zenonApi.Logic.Converters
{
  public class LogicVariableAttributeConverter : IZenonSerializationConverter
  {
    public string Convert(object source)
    {
      // TODO: is the logic herin correct? Are spaces between the "," and the next values required/optional/allowed?
      if (source is LogicVariableAttributes attributes)
      {
        StringBuilder sb = new StringBuilder();
        bool needsComma = false;

        if (attributes.In)
        {
          sb.Append("IN");
          needsComma = true;
        }
        if (attributes.Out)
        {
          sb.Append("OUT");
          needsComma = true;
        }
        if (attributes.Constant)
        {
          if (needsComma)
          {
            sb.Append(",");
          }
          sb.Append("constant");
          needsComma = true;
        }
        if (attributes.External)
        {
          if (needsComma)
          {
            sb.Append(",");
          }
          sb.Append("external");
        }

        return sb.ToString();
      }

      return null;
    }

    public object Convert(string source)
    {
      if (string.IsNullOrWhiteSpace(source))
      {
        return null;
      }

      LogicVariableAttributes attributes = new LogicVariableAttributes();
      source = source.Trim();

      // TODO: INOUT, IN, OUT must come at the beginning, right?
      if (source.StartsWith("INOUT"))
      {
        attributes.In = true;
        attributes.Out = true;
      }
      else if (source.StartsWith("IN"))
      {
        attributes.In = true;
      }
      else if (source.StartsWith("OUT"))
      {
        attributes.Out = true;
      }

      if (source.Contains("constant", StringComparison.OrdinalIgnoreCase))
      {
        attributes.Constant = true;
      }
      else if (source.Contains("external", StringComparison.OrdinalIgnoreCase))
      {
        attributes.External = true;
      }

      return attributes;
    }
  }
}
