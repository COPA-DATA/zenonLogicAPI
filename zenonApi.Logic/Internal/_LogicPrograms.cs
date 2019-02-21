using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Serialization;

namespace zenonApi.Logic.Internal
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

    protected override string NodeName => "programs";

    [zenonSerializableNode("pou")]
    public List<_Pou> ProgramOrganizationUnits { get; set; }
  }
}
