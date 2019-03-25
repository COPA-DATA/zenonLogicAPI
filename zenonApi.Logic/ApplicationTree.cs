using zenonApi.Collections;
using zenonApi.Logic.SerializationConverters;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public class ApplicationTree : zenonSerializable<ApplicationTree, LogicProject, LogicProject>, ILogicFileContainer
  {
    private ApplicationTree() { }

    public ApplicationTree(LogicProject parent) => this.Parent = this.Root = parent;

    #region interface implementation
    public override string NodeName => "Appli";
    ILogicFileContainer IZenonSerializable<ILogicFileContainer, ILogicFileContainer, LogicProject>.Parent
    {
      get => null; // No more parent file container, this is the logical root for the file structure
    }
    #endregion

    #region Specific properties
    [zenonSerializableAttribute("Expand", Converter = typeof(YesNoConverter))]
    public bool Expand { get; protected set; } = true;

    [zenonSerializableNode("Folder")]
    public ContainerAwareObservableCollection<LogicFolder> Folders { get; protected set; }
      = new ContainerAwareObservableCollection<LogicFolder>();

    [zenonSerializableNode("Program")]
    public ContainerAwareObservableCollection<LogicProgram> Programs { get; protected set; }
      = new ContainerAwareObservableCollection<LogicProgram>();



    //[zenonSerializableNode("FieldBus")]
    //protected object FieldBus { get; set; }
    //[zenonSerializableNode("Binding")]
    //protected object Binding { get; set; }
    //[zenonSerializableNode("Profiles")]
    //protected object Profiles { get; set; }
    //[zenonSerializableNode("IOS")]
    //protected object IOS { get; set; }
    //[zenonSerializableNode("GlobalDefs")]
    //protected object GlobalDefinitions { get; set; }
    //[zenonSerializableNode("Vars")]
    //protected object Variables { get; set; }
    //[zenonSerializableNode("Types")]
    //protected object Types { get; set; }
    #endregion
  }
}
