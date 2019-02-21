using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Core;

namespace zenonApi.Logic
{
  public interface ILogicVariableGroupContainer : IZenonSerializable<IZenonSerializable, LogicProject>
  {
    List<LogicVariableGroup> VariableGroups { get; }
  }
}
