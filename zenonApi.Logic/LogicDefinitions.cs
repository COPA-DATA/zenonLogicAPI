using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class LogicDefinitions : zenonSerializable<LogicDefinitions, LogicProject, LogicProject>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "definitions";
    #endregion

    /// <summary>
    /// This tag groups the COMMON and GOLBAL definitions.
    /// There can be two <defines> tags for COMMON and GLOBAL definitions.
    /// </summary>
    [zenonSerializableNode("defines", NodeOrder = 0)]
    public ExtendedObservableCollection<LogicDefine> Defines { get; protected set; } 
      = new ExtendedObservableCollection<LogicDefine>
      {
        new LogicDefine("(GLOBAL)")
      };
  }
}
