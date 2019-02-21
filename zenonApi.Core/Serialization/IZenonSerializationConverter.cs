using System;
using System.Collections.Generic;
using System.Text;

namespace zenonApi.Serialization
{
  public interface IZenonSerializationConverter
  {
    string Convert(object source);
    object Convert(string source);
  }
}
