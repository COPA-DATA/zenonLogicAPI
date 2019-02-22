using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public interface ILogicFileContainer : IZenonSerializable<ILogicFileContainer, LogicProject>
  {
    ContainerAwareObservableCollection<LogicFolder> Folders { get; }

    ContainerAwareObservableCollection<LogicProgram> Programs { get; }
  }
}
