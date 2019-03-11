using zenonApi.Serialization;

namespace zenonApi.Logic.Network
{
  public class LogicNetworkBinding : zenonSerializable<LogicNetworkBinding>
  {
    #region zenonSerializable Implementation
    public override string NodeName => "binding";
    #endregion

    /// <summary>
    /// This tag groups all the public variables configurated for binding.
    /// </summary>
    [zenonSerializableNode("bindpublic", NodeOrder = 0)]
    public LogicPublicBinding PublicBinding { get; set; }

    /// <summary>
    /// This tag groups all public and extern definitions for binding.
    /// </summary>
    [zenonSerializableNode("bindextern", NodeOrder = 1)]
    public LogicExternBinding ExternBinding { get; set; }
  }
}
