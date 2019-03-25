using zenonApi.Serialization;

namespace zenonApi.Logic.SequenceFlowChart
{
  /// <summary>
  /// The kind of a <see cref="Step"/>.
  /// </summary>
  public enum StepKind
  {
    /// <summary>A default step.</summary>
    [zenonSerializableEnum("default")]
    Default,
    /// <summary>An initial step.</summary>
    [zenonSerializableEnum("init")]
    InitialStep,
    /// <summary>Beginning step in the body of a macro-step.</summary>
    [zenonSerializableEnum("begin")]
    Begin,
    /// <summary>Terminating step in the body of a macro-step.</summary>
    [zenonSerializableEnum("end")]
    End
  }
}
