namespace zenonApi.Serialization
{
  /// <summary>
  /// Specifies, if a <see cref="zenonSerializable{TSelf,TParent,TRoot}"/> was loaded from a deserialized file,
  /// was newly created via code, was modified and if it was already deserialized to a file.
  /// </summary>
  // ReSharper disable once InconsistentNaming : "zenon" is always written lowercase.
  public enum zenonSerializableStatusEnum
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
