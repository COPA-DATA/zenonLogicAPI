using System.Xml.Linq;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class LogicProjectSettings : zenonSerializable<LogicProjectSettings, LogicProject, LogicProject>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "settings";
    #endregion  

    /// <summary>
    /// This tag describes the triggering of runtime cycles in the project settings.
    /// </summary>
    [zenonSerializableNode("triggering", NodeOrder = 0)]
    public string TriggerTime { get; set; }

    /// <summary>
    /// This tag groups all the options for the compiler.
    /// </summary>
    [zenonSerializableNode("compiler", NodeOrder = 1)]
    public string CompilerSettings { get; protected set; }

    /// <summary>
    /// This tag groups all the options for the compiler regarding the On Line Change capability.
    /// </summary>
    [zenonSerializableNode("onlinechange", NodeOrder = 2)]
    public string OnlineChangeCompilerSettings { get; protected set; }

    //TODO: Ask StefanH about this property (not in docu)
    [zenonSerializableRawFormat("fbundef", NodeOrder = 3)]
    public XElement FbUndef { get; set; }

    //TODO: Ask StefanH about this property (not in docu)
    [zenonSerializableRawFormat("onlinecsts", NodeOrder = 4)]
    public XElement OnlineCasts { get; set; }

  }
}
