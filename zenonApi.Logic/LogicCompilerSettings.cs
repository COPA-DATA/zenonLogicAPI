using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class LogicCompilerSettings : zenonSerializable<LogicCompilerSettings>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "compiler";
    #endregion 

    /// <summary>
    /// This property groups all the general options for the compiler.
    /// </summary>
    [zenonSerializableNode("cmpoptions", NodeOrder = 0)]
    public LogicCompilerOptions CompilerOptions { get; protected set; } = new LogicCompilerOptions();

    /// <summary>
    /// This property groups all the options for the definition of the code generated for simulation.
    /// </summary>
    [zenonSerializableNode("cmpsimulcode", NodeOrder = 1)]
    public LogicSimulationCodeOptions SimulationCodeOptions { get; protected set; } = new LogicSimulationCodeOptions();

    /// <summary>
    /// This property groups all the options for the definition of the code generated for the target runtime.
    /// </summary>
    [zenonSerializableNode("cmptargetcode", NodeOrder = 2)]
    public LogicTargetCodeOptions TargetCodeOptions { get; protected set; } = new LogicTargetCodeOptions();
  }
}
