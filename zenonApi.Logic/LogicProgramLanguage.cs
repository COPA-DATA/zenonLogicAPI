using zenonApi.Core;

namespace zenonApi.Logic
{
  public enum LogicProgramLanguage
  {
    [zenonSerializableEnum("FBD")]
    FunctionBlockDiagram,
    [zenonSerializableEnum("ST")]
    StructuredText,
    [zenonSerializableEnum("SFC")]
    SequenceFlowChart //TODO; is this the correct name?
  }
}
