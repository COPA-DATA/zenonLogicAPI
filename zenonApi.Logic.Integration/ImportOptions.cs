using System;

namespace zenonApi.Zenon
{
  /// <summary>
  ///   Specifies import options to use for importing a <see cref="Logic.LogicProject"/> into zenon.
  /// </summary>
  [Flags]
  public enum ImportOptions
  {
    /// <summary>
    ///   The default setting for importing logic projects into zenon.<br />
    ///   This will not recreate variables and will merge import files with existing projects.<br />
    ///   This option is equal to calling the <c>K5B.exe</c> with argument <c>XMLMERGE</c>.
    /// </summary>
    Default = 0,
    /// <summary>
    ///   Imports or merges the logic project(s) and additionally recreates variables if needed.<br />
    ///   This option is equal to calling <c>K5B.exe</c> with argument <c>XMLMERGE-RV</c>.
    /// </summary>
    ReCreateVariables = 1,
    /// <summary>
    ///   Imports the logic project(s) but does not merge members.<br />
    ///   This option is equal to calling <c>K5B.exe</c> with argument <c>XMLMERGE-NM</c>.
    /// </summary>
    DoNotMerge = 2,
    /// <summary>
    ///   Imports the logic project(s) and recreates variables if needed, but does not merge members.<br />
    ///   This option is equal to calling <c>K5B.exe</c> with argument <c>XMLMERGE-RV-NM</c>.
    /// </summary>
    ReCreateVariablesDoNotMerge = 3,

    /// <summary>
    ///   Applies the online settings to the logic project.<br /> 
    /// </summary>
    ApplyOnlineSettings = 4
  }
}
