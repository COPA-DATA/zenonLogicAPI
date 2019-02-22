using zenonApi.Serialization;
using zenonApi.Collections;

namespace zenonApi.Logic
{
  public interface ILogicVariableGroupContainer : IZenonSerializable<IZenonSerializable, LogicProject>
  {
    ContainerAwareObservableCollection<LogicVariableGroup> VariableGroups { get; }
  }
}
