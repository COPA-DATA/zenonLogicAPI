using zenonApi.Collections;
using zenonApi.Logic.SerializationConverters;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public sealed class ApplicationTree : zenonSerializable<ApplicationTree, LogicProject, LogicProject>, ILogicFileContainer
  {
    // ReSharper disable once UnusedMember.Local : Required default constructor for serialization.
    private ApplicationTree()
    {
      Programs = new LogicProgramCollection(this, null);
      Folders = new ContainerAwareObservableCollection<LogicFolder>(this);
    }

    // ReSharper disable once VirtualMemberCallInConstructor : This is intended.
    public ApplicationTree(LogicProject parent) : this() => Parent = Root = parent;

    #region interface implementation
    public override string NodeName => "Appli";
    ILogicFileContainer IZenonSerializable<ILogicFileContainer, ILogicFileContainer, LogicProject>.Parent
    {
      get => null; // No more parent file container, this is the logical root for the file structure
    }
    #endregion

    #region Specific properties
    [zenonSerializableAttribute("Expand", Converter = typeof(YesNoConverter))]
    public bool Expand { get; private set; } = true;

    [zenonSerializableNode("Folder")]
    public ContainerAwareObservableCollection<LogicFolder> Folders { get; private set; }

    [zenonSerializableNode("Program")]
    public LogicProgramCollection Programs { get; private set; }

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

    #region Convenience methods
    public void Add(LogicFolder folder) => Folders.Add(folder);

    public void Add(LogicProgram program) => Programs.Add(program);
    #endregion
  }
}
