//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using zenonApi.Collections;
//using zenonApi.Serialization;

//namespace zenonApi.Logic
//{
//  /// <summary>
//  /// Lists all defined data types.
//  /// </summary>
//  public class LogicDataTypesCollection
//    : zenonSerializable<LogicDataTypesCollection, LogicProject, LogicProject>,
//    IList<LogicDataType>,
//    IEnumerable<LogicDataType>
//  {
//    private LogicDataTypesCollection() { }

//    public LogicDataTypesCollection(LogicProject parent) => this.Parent = this.Root = parent;


//    #region zenonSerializable Implementation
//    public override string NodeName => "types";
//    #endregion


//    #region Specific properties
//    /// <summary>
//    /// List of all defined data types.
//    /// </summary>
//    [zenonSerializableNode("type", NodeOrder = 0)]
//    private ExtendedObservableCollection<LogicDataType> DataTypes { get; set; }
//      = new ExtendedObservableCollection<LogicDataType>();
//    #endregion


//    #region Interface implementation
//    public int Count => DataTypes.Count;
//    public bool IsReadOnly => false;
//    public LogicDataType this[int index] { get => DataTypes[index]; set => DataTypes[index] = value; }

//    public int IndexOf(LogicDataType item) => DataTypes.IndexOf(item);
//    public void Insert(int index, LogicDataType item) => DataTypes.Insert(index, item);
//    public void RemoveAt(int index) => DataTypes.RemoveAt(index);
//    public void Add(LogicDataType item) => DataTypes.Add(item);
//    public void Clear() => DataTypes.Clear();
//    public bool Contains(LogicDataType item) => DataTypes.Contains(item);
//    public void CopyTo(LogicDataType[] array, int arrayIndex) => DataTypes.CopyTo(array, arrayIndex);
//    public bool Remove(LogicDataType item) => DataTypes.Remove(item);
//    public IEnumerator<LogicDataType> GetEnumerator() => DataTypes.GetEnumerator();
//    IEnumerator IEnumerable.GetEnumerator() => DataTypes.GetEnumerator();
//    #endregion
//  }
//}
