using zenonApi.Core;

namespace zenonApi.Logic
{
  internal class _Pou : zenonSerializable<_Pou, _LogicPrograms, LogicProject>
  {
    private _Pou() { }
    public _Pou(_LogicPrograms parent, LogicProject root)
    {
      this.Parent = parent;
      this.Root = root;
    }

    public override _LogicPrograms Parent { get; protected set; }
    public override LogicProject Root { get; protected set; }

    protected override string NodeName => "pou";

    [zenonSerializableAttribute("name")]
    public string Name { get; set; }

    [zenonSerializableAttribute("kind")]
    public LogicProgramType Kind { get; set; }

    [zenonSerializableAttribute("lge")]
    public LogicProgramLanguage Language { get; set; }

    [zenonSerializableAttribute("period")]
    public uint Period { get; set; }

    [zenonSerializableNode("sourceSTIL")]
    public string SourceCode { get; set; }
  }
}
