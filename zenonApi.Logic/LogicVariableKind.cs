using zenonApi.Core;

namespace zenonApi.Logic
{
  public enum LogicVariableKind
  {
    /// <summary>
    /// Global variables
    /// </summary>
    [zenonSerializableEnum("GLOBAL")]
    Global,

    /// <summary>
    /// Global retain variables
    /// </summary>
    [zenonSerializableEnum("RETAIN")]
    Retain,

    /// <summary>
    /// Local variables of a program or UDFB
    /// </summary>
    [zenonSerializableEnum("LOCAL")]
    Local,

    /// <summary>
    /// Variables of an I/O board.
    /// </summary>
    [zenonSerializableEnum("IO")]
    IO
  }
}
