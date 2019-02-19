using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Core;

namespace zenonApi.Logic
{
  public enum LogicProgramType
  {
    [zenonSerializableEnum("program")]
    Program,
    [zenonSerializableEnum("UDFB")]
    UDFB,
  }
}
