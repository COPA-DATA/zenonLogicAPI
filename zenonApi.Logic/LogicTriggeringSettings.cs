using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class LogicTriggeringSettings : zenonSerializable<LogicTriggeringSettings>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "triggering";
    #endregion

    /// <summary>
    /// Cycle timing expressed as a number of microseconds.
    /// Value "0" indicates that cycles are performed as fast as possible (no wait).
    /// </summary>
    [zenonSerializableAttribute("cycletiming", AttributeOrder = 0)]
    public uint CycleTime { get; set; } = 10000;
  }
}
