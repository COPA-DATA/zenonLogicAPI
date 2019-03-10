using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class LogicTargetCodeOptions : zenonSerializable<LogicTargetCodeOptions>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "cmptargetcode";
    #endregion 

    /// <summary>
    /// Options of the definition of the code generated for the target runtime
    /// Default values for this property are the default settings for a zenon logic project version 8.00.
    /// <value></value>
    /// </summary>
    /// <value>
    /// Property descriptions found in documentation:
    /// target: Target name of the runtime
    /// suffix: Suffix of downloaded code file
    /// motorolaendian: Code for the runtime is in big endian format
    /// t5style: Simulator is based on standard T5 runtime
    /// comment: Description of the runtime system
    /// configname: User defined name for the runtime system
    /// </value>
    [zenonSerializableNode("opt", NodeOrder = 0)]
    public ExtendedObservableCollection<LogicOptionTuple> OptionTuples { get; protected set; }
    = new ExtendedObservableCollection<LogicOptionTuple>
      {
        new LogicOptionTuple("target","T5RTI"),
        new LogicOptionTuple("suffix",".XTI"),
        new LogicOptionTuple("motorolaendian","OFF"),
        new LogicOptionTuple("t5style","ON"),
        new LogicOptionTuple("configname","T5RTI"),
        new LogicOptionTuple("comment","Straton T5 runtime (Intel like byte ordering)")
      };
  }
}
