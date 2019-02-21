using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Serialization;
using zenonApi.Logic.SerializationConverters;

namespace zenonApi.Logic
{
  public class ApplicationTree : zenonSerializable<ApplicationTree, LogicProject, LogicProject>, ILogicFileContainer
  {
    private ApplicationTree() { }

    #region zenonSerializable implementation
    ILogicFileContainer IZenonSerializable<ILogicFileContainer, LogicProject>.Parent => null;
    protected override string NodeName => "Appli";
    #endregion

    #region Specific properties
    [zenonSerializableAttribute("Expand", Converter = typeof(YesNoConverter))]
    public bool Expand { get; protected set; } = true;

    [zenonSerializableNode("Folder")]
    public List<LogicFolder> Folders { get; set; }

    [zenonSerializableNode("Program")]
    public List<LogicProgram> Programs { get; set; }


    //[zenonSerializableNode("FieldBus")]
    //protected object FieldBus { get; set; }
    //[zenonSerializableNode("Binding")]
    //protected object Binding { get; set; }
    //[zenonSerializableNode("Profiles")]
    //protected object Profiles { get; set; }
    //[zenonSerializableNode("IOS")]
    //protected object IOS { get; set; }
    //[zenonSerializableNode("GlobalDefs")]
    //protected object GlobalDefinitions { get; set; }
    //[zenonSerializableNode("Vars")]
    //protected object Variables { get; set; }
    //[zenonSerializableNode("Types")]
    //protected object Types { get; set; }
    #endregion
  }
}
