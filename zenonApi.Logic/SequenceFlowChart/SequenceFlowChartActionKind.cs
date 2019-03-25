using zenonApi.Serialization;

namespace zenonApi.Logic.SequenceFlowChart
{
  /// <summary>
  /// The kind of an <see cref="zenonApi.Logic.SequenceFlowChart.SequenceFlowChartAction"/>.
  /// </summary>
  public enum SequenceFlowChartActionKind
  {
    /// <summary>List of simple action blocks in text format.</summary>
    [zenonSerializableEnum("default")]
    Default,
    /// <summary>P1 action block.</summary>
    [zenonSerializableEnum("P1")]
    InitialStep,
    /// <summary>N simple action block.</summary>
    [zenonSerializableEnum("N")]
    Begin,
    /// <summary>P simple action block.</summary>
    [zenonSerializableEnum("P0")]
    End
  }
}
