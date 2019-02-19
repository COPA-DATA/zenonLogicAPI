using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Core;

namespace zenonApi.Logic
{
  internal class _LogicPrograms : zenonSerializable<_LogicPrograms, LogicProject>
  {
    private _LogicPrograms() { }

    public _LogicPrograms(LogicProject parent)
    {
      this.Parent = parent;
    }

    public override LogicProject Parent { get; protected set; }

    protected override string NodeName => "programs";
  }
}
