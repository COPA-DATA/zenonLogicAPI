using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic.Internal
{
  /// <summary>
  /// Lists all programs, sub-programs and UDFBs of the project.
  /// It is not intended to be manipulated by users of this API directly.
  /// </summary>
  // ReSharper disable once InconsistentNaming : Named the class with an underscore by intend to express
  // that it is not reachable via the public API.
  internal sealed class _LogicProgramsCollection : zenonSerializable<_LogicProgramsCollection, LogicProject, LogicProject>
  {
    /// <summary>Private default constructor for serialization.</summary>
    // ReSharper disable once UnusedMember.Local : Required default constructor for serialization.
    private _LogicProgramsCollection() { }

    public _LogicProgramsCollection(LogicProject parent) => this.Parent = this.Root = parent;

    public override string NodeName => "programs";

    [zenonSerializableNode("pou")]
    public ContainerAwareObservableCollection<_Pou> ProgramOrganizationUnits { get; private set; }
      = new ContainerAwareObservableCollection<_Pou>();
  }
}
