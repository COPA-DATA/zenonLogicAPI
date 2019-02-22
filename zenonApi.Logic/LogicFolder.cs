using zenonApi.Serialization;
using zenonApi.Logic.SerializationConverters;
using zenonApi.Collections;

namespace zenonApi.Logic
{
  public class LogicFolder : zenonSerializable<LogicFolder, ILogicFileContainer, LogicProject>, ILogicFileContainer
  {
    #region zenonSerializable impelementation
    protected override string NodeName => "Folder";
    #endregion

    #region Specific properties
    [zenonSerializableNode("Folder")]
    public ContainerAwareObservableCollection<LogicFolder> Folders { get; protected set; }

    [zenonSerializableNode("Program")]
    public ContainerAwareObservableCollection<LogicProgram> Programs { get; protected set; }

    [zenonSerializableAttribute("Expand", Converter = typeof(YesNoConverter))]
    protected bool Expand { get; set; } = true;

    [zenonSerializableAttribute("Name")]
    public string Name { get; set; }
    #endregion
  }
}
