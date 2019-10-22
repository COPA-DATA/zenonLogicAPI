namespace zenonApi.Serialization
{
  /// <summary>
  /// Object status information state.
  /// </summary>
  public enum ZenonSerializableStatusEnum
  {
    /// <summary>
    /// Default object state after constructor call.
    /// </summary>
    New,
    /// <summary>
    /// Object state after deserialization from XML input format.
    /// </summary>
    Loaded,
    /// <summary>
    /// Object state after object modification by any setter call.
    /// </summary>
    Modified,
    /// <summary>
    /// Object state after serialization to XML output format.
    /// </summary>
    Deserialized
  }
}
