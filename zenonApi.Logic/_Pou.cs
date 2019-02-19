using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Core;

namespace zenonApi.Logic
{
  internal class _Pou : zenonSerializable<_Pou, _LogicPrograms>
  {
    private _Pou() { }
    public _Pou(_LogicPrograms parent)
    {
      this.Parent = parent;
    }

    public override _LogicPrograms Parent { get; protected set; }

    protected override string NodeName => "pou";

    [zenonSerializableAttribute("name")]
    public string Name { get; set; }
  }
}
