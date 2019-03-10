using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  /// <summary>
  /// Lists all defined data types.
  /// </summary>
  public class LogicDataTypesCollection : zenonSerializable<LogicDataTypesCollection, LogicProject, LogicProject>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "types";
    #endregion

    #region Specific properties
    /// <summary>
    /// List of all defined data types.
    /// </summary>
    [zenonSerializableNode("type", NodeOrder = 0)]
    public ExtendedObservableCollection<LogicDataType> DataTypes { get; set; }
    #endregion
  }
}
