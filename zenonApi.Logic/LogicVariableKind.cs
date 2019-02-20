using zenonApi.Core;

namespace zenonApi.Logic
{
  public enum LogicVariableKind
  {
    [zenonSerializableEnum("GLOBAL")]
    Global,
    [zenonSerializableEnum("RETAIN")]
    Retain,
    [zenonSerializableEnum("LOCAL")]
    Local,
    [zenonSerializableEnum("IO")]
    IO
  }
}
