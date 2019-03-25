using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class LogicSimulationCodeOptions : zenonSerializable<LogicSimulationCodeOptions>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "cmpsimulcode";
    #endregion 

    /// <summary>
    /// Options of the definition of the code generated for simulation stored as key-value pairs.
    /// Default values of this property are the default settings for a zenon logic project version 8.00.
    /// </summary>
    /// <value>
    /// Property descriptions found in documentation:
    /// target: Target name of the simulator
    /// suffix: Suffix of simulator code file
    /// motorolaendian: Code for the simulator is in big endian format
    /// t5style: Simulator is based on standard T5 runtime
    /// </value>
    [zenonSerializableNode("opt", NodeOrder = 0)]
    public ExtendedObservableCollection<LogicOptionTuple> OptionTuples { get; protected set; }
      = new ExtendedObservableCollection<LogicOptionTuple>
      {
        new LogicOptionTuple("target","T5SIMUL"),
        new LogicOptionTuple("suffix",".XWS"),
        new LogicOptionTuple("motorolaendian","OFF"),
        new LogicOptionTuple("t5style","ON")
      };
  }
}
