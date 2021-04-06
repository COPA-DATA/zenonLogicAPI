using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public sealed class LogicDefinitions : zenonSerializable<LogicDefinitions, LogicProject, LogicProject>
  {
    // ReSharper disable once UnusedMember.Local : Required default constructor for serialization.
    private LogicDefinitions() { }

    public LogicDefinitions(LogicProject parent) => this.Parent = this.Root = parent;

    #region zenonSerializable Implementation
    public override string NodeName => "definitions";
    #endregion

    /// <summary>
    /// This tag groups the COMMON and GLOBAL definitions.
    /// There can be two &lt;defines&gt; tags for COMMON and GLOBAL definitions.
    /// </summary>
    [zenonSerializableNode("defines", NodeOrder = 0)]
    public ExtendedObservableCollection<LogicDefine> Defines { get; private set; }
      = new ExtendedObservableCollection<LogicDefine>
    {
      new LogicDefine("(GLOBAL)")
    };
  }
}
