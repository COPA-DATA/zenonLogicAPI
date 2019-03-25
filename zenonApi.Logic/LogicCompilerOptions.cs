using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class LogicCompilerOptions : zenonSerializable<LogicCompilerOptions>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "cmpoptions";
    #endregion 

    /// <summary>
    /// Compiler options stored as key-value pairs.
    /// Default values of this property are the default settings for a zenon logic project version 8.00.
    /// </summary>
    /// <value>
    /// Property descriptions found in documentation:
    /// trace: Display internal trace information
    /// tracetime: Display the compiling time
    /// simul: Generates code for the simulator
    /// lock: Describe the set of variables that can be locked at runtime
    /// debug: Compile the project in DEBUG mode
    /// fbdflow: Generate tracking of value for FDB lines
    /// sfcsafe: Run safety checks on SFC charts
    /// smbedsymbols: Embed all symbols in the code
    /// safearray: Check array bounds at runtime
    /// warning: Display warning messages
    /// </value>
    [zenonSerializableNode("opt", NodeOrder = 0)]
    public ExtendedObservableCollection<LogicOptionTuple> OptionTuples { get; protected set; }
      = new ExtendedObservableCollection<LogicOptionTuple>
      {
        new LogicOptionTuple("simul","ON"),
        new LogicOptionTuple("debug","ON"),
        new LogicOptionTuple("ctseg","ON"),
        new LogicOptionTuple("fbdflow","ON"),
        new LogicOptionTuple("embedsymbols","OFF"),
        new LogicOptionTuple("embedsybcase","ON"),
        new LogicOptionTuple("warning","ON"),
        new LogicOptionTuple("sfcsafe","ON"),
        new LogicOptionTuple("fbdoptim","OFF"),
        new LogicOptionTuple("ldoptim","OFF"),
        new LogicOptionTuple("checksybconflicts","OFF"),
        new LogicOptionTuple("ststrict","OFF"),
        new LogicOptionTuple("ieccheck","OFF"),
        new LogicOptionTuple("tracepousize","OFF"),
        new LogicOptionTuple("checkprofile","OFF"),
        new LogicOptionTuple("safearray","ON"),
        new LogicOptionTuple("norealeq","OFF"),
        new LogicOptionTuple("stan","OFF"),
        new LogicOptionTuple("vsi","OFF"),
        new LogicOptionTuple("fbcheck","OFF"),
        new LogicOptionTuple("noiostep","OFF"),
        new LogicOptionTuple("rtrigno0","OFF"),
        new LogicOptionTuple("ushr","OFF"),
        new LogicOptionTuple("optbool","OFF"),
        new LogicOptionTuple("loadupl","OFF"),
        new LogicOptionTuple("checkfbdinputs","OFF"),
        new LogicOptionTuple("updext","OFF"),
        new LogicOptionTuple("extdef","OFF"),
        new LogicOptionTuple("password","0"),
        new LogicOptionTuple("mwlog","OFF"),
        new LogicOptionTuple("vmheap","0"),
        new LogicOptionTuple("lock","NONE"),
        new LogicOptionTuple("cc",""),
        new LogicOptionTuple("cctooldir",""),
        new LogicOptionTuple("cctarget",""),
        new LogicOptionTuple("ccpure","OFF"),
        new LogicOptionTuple("ccload","OFF"),
        new LogicOptionTuple("ccshow","ON"),
        new LogicOptionTuple("cchot","OFF"),
        new LogicOptionTuple("spoptim","OFF"),
        new LogicOptionTuple("trace","OFF"),
        new LogicOptionTuple("tracetime","OFF"),
        new LogicOptionTuple("mapuint","ON")
      };
  }
}
