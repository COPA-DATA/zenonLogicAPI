using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Core;

namespace zenonApi.Logic
{
  public class LogicProject : zenonSerializable<LogicProject, IZenonSerializable>
  {
    #region zenonSerializable Implementation
    protected override string NodeName => "K5project";
    // TODO: Who holds the logic projects? Nobody? If yes, then leave this to null
    public override IZenonSerializable Parent { get; protected set; }
    #endregion

    [zenonSerializableAttribute("version")]
    public string Version { get; protected set; }

    [zenonSerializableAttribute("path")]
    public string Path { get; protected set; }

    [zenonSerializableNode("Appli")]
    public ApplicationTree ApplicationTree { get; protected set; }
  }
}
