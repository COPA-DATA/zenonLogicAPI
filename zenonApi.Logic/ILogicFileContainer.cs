using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Core;

namespace zenonApi.Logic
{
  public interface ILogicFileContainer : IZenonSerializable<ILogicFileContainer, LogicProject>
  {
    List<LogicFolder> Folders { get; }

    List<LogicProgram> Programs { get; }
  }
}
