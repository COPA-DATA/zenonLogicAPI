using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zenonApi.Core;
using zenonApi.Logic.Converters;

namespace zenonApi.Logic
{
  public class LogicFolder : zenonSerializable<LogicFolder, ILogicFileContainer>, ILogicFileContainer
  {
    private LogicFolder() { }

    internal LogicFolder(ApplicationTree applicationTree, LogicFolder parent)
    {
      // TODO applicationTree
      this.Parent = parent;
    }

    #region zenonSerializable impelementation
    public override ILogicFileContainer Parent { get; protected set; }
    protected override string NodeName => "Folder";
    #endregion

    #region Specific properties
    [zenonSerializableNode("Folder")]
    public List<LogicFolder> Folders { get; protected set; }

    [zenonSerializableNode("Program")]
    public List<LogicProgram> Programs { get; protected set; }

    [zenonSerializableAttribute("Expand", Converter = typeof(YesNoConverter))]
    protected bool Expand { get; set; } = true;

    [zenonSerializableAttribute("Name")]
    public string Name { get; set; }
    #endregion
  }
}
