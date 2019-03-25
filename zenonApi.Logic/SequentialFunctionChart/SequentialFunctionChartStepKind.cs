using zenonApi.Serialization;

namespace zenonApi.Logic.SequentialFunctionChart
{
  /// <summary>
  /// The kind of a <see cref="SequentialFunctionChartStep"/>.
  /// </summary>
  public enum SequentialFunctionChartStepKind
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
