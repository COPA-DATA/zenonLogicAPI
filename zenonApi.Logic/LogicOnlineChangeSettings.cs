using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class LogicOnlineChangeSettings : zenonSerializable<LogicOnlineChangeSettings>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "onlinechange";
    #endregion 

    /// <summary>
    /// Options for the compiler regarding the On Line Change capability.
    /// Default values for this property are the default settings for a zenon logic project version 8.00.
    /// <value></value>
    /// </summary>
    /// <value>
    /// Property descriptions found in documentation:
    /// enable: On Line Change is enabled
    /// size_d8: minimum number of BOOL/SINT variables
    /// size_d16: minimum number of INT variables
    /// size_d32: miminum number of DINT/REAL variables
    /// size_d64: minimum number of LINT/LREAL variables
    /// size_tmr: minimum number of TIME variables
    /// size_actmr: minimum number of active TIME variables
    /// size_str: minimum number of STRING variables
    /// size_strbuf: minimum global size allocated for string buffers
    /// size_fbi: minimum number of FB instances
    /// size_fbibuf: minimum global size allocated for data of FB instances
    /// size_publish: minimum number of published variables
    /// size_xv: minimum number of external variables
    /// </value>
    [zenonSerializableNode("opt", NodeOrder = 0)]
    public ExtendedObservableCollection<LogicOptionTuple> OptionTuples { get; protected set; }
    = new ExtendedObservableCollection<LogicOptionTuple>
      {
        new LogicOptionTuple("enable","0"),
        new LogicOptionTuple("size_d8","0"),
        new LogicOptionTuple("size_d16","0"),
        new LogicOptionTuple("size_d32","0"),
        new LogicOptionTuple("size_d64","0"),
        new LogicOptionTuple("size_tmr","0"),
        new LogicOptionTuple("size_actmr","0"),
        new LogicOptionTuple("size_str","0"),
        new LogicOptionTuple("size_strbuf","0"),
        new LogicOptionTuple("size_fbi","0"),
        new LogicOptionTuple("size_fbibuf","0"),
        new LogicOptionTuple("size_publish","0"),
        new LogicOptionTuple("size_xv","0"),
        new LogicOptionTuple("size_ct","0"),
        new LogicOptionTuple("size_pou","0")
      };
  }
}
