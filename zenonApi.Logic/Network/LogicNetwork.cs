using System.Xml.Linq;
using zenonApi.Serialization;

namespace zenonApi.Logic.Network
{
  public sealed class LogicNetwork : zenonSerializable<LogicNetwork, LogicProject, LogicProject>
  {
    // ReSharper disable once UnusedMember.Local : Required default constructor for serialization.
    private LogicNetwork() { }

    public LogicNetwork(LogicProject parent) => this.Parent = this.Root = parent;

    #region zenonSerializable Implementation
    public override string NodeName => "networks";
    #endregion

    //TODO: check definition string from docu - was already used once for definitions tag of project
    /// <summary>
    /// This property groups the COMMON and GOLBAL definitions.
    /// </summary>
    [zenonSerializableNode("binding", NodeOrder = 0)]
    public LogicNetworkBinding Binding { get; private set; } = new LogicNetworkBinding();

    /// <summary>
    /// This property groups the whole MODBUS slave or master configuration.
    /// </summary>
    [zenonSerializableRawFormat("modbus", NodeOrder = 1, OmitIfNull = true)]
    public XElement Modbus { get; set; }

    /// <summary>
    /// This property describes the whole AS-i configuration.
    /// </summary>
    [zenonSerializableRawFormat("asi", NodeOrder = 2, OmitIfNull = true)]
    public XElement ActuatorSensorInterface { get; set; }
  }
}