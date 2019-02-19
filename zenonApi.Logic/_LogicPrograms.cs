using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Core;

namespace zenonApi.Logic
{
  /// <summary>
  /// Lists all programs, sub-programs and UDFBs of the project.
  /// It is not intended to be manipulated by users of this API directly.
  /// </summary>
  internal class _LogicPrograms : zenonSerializable<_LogicPrograms, LogicProject, LogicProject>
  {
    /// <summary>
    /// Private default constructor for serialization.
    /// </summary>
    private _LogicPrograms() { }

    public _LogicPrograms(LogicProject parent)
    {
      this.Parent = parent;
      // For this class, the parent is the same as the root:
      this.Root = parent;
    }

    public override LogicProject Parent { get; protected set; }
    public override LogicProject Root { get; protected set; }

    protected override string NodeName => "programs";

    [zenonSerializableNode("pou")]
    public List<_Pou> ProgramOrganizationUnits { get; set; }
  }
}
