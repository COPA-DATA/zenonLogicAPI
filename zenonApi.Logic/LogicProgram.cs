using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Core;

namespace zenonApi.Logic
{
  public class LogicProgram : zenonSerializable<LogicProgram, ILogicFileContainer>
  {
    private LogicProgram() { }

    internal LogicProgram(LogicFolder parent)
    {
      this.Parent = parent;
    }

    [zenonSerializableAttribute("Name")]
    public string Name { get; set; }

    public override ILogicFileContainer Parent { get; protected set; }
    protected override string NodeName => "Program";
  }
}
