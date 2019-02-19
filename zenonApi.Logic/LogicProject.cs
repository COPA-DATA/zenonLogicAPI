using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using zenonApi.Core;

namespace zenonApi.Logic
{
  /// <summary>
  /// The root of a K5 project structure.
  /// </summary>
  public class LogicProject : zenonSerializable<LogicProject, IZenonSerializable, LogicProject>
  {
    #region zenonSerializable Implementation
    protected override string NodeName => "K5project";
    // TODO: Who holds the logic projects? Nobody? If yes, then leave this to null
    public override IZenonSerializable Parent { get; protected set; }
    public override LogicProject Root { get; protected set; }
    #endregion

    /// <summary>
    /// The mandatory version of the K5 project.
    /// </summary>
    [zenonSerializableAttribute("version")]
    public string Version { get; protected set; }

    /// <summary>
    /// The original pathname of the K5 project's folder.
    /// </summary>
    [zenonSerializableAttribute("path")]
    public string Path { get; protected set; }

    /// <summary>
    /// Contains the logical folder structure of the programs and UDFBs.
    /// </summary>
    [zenonSerializableNode("Appli")]
    public ApplicationTree ApplicationTree { get; protected set; }

    /// <summary>
    /// Lists all programs, sub-programs and UDFBs of the project.
    /// It is not intended to be manipulated by users of this API directly.
    /// </summary>
    [Browsable(false)]
    [zenonSerializableNode("programs")]
    internal _LogicPrograms Programs { get; set; }
  }
}
