using zenonApi.Serialization;

namespace zenonApi.Logic
{
  public enum LogicVariableInformationTypeKind
  {
    /// <summary>
    /// Variable tag (short comment)
    /// </summary>
    [zenonSerializableEnum("tag")]
    Tag,
    [zenonSerializableEnum("desc")]
    Description,
    /// <summary>
    /// Name of the embedded profile.
    /// </summary>
    [zenonSerializableEnum("profile")]
    Profile,
    /// <summary>
    /// Set of embedded properties.
    /// </summary>
    [zenonSerializableEnum("embed")]
    Embed,
    //TODO: confirm this tag´s string
    [zenonSerializableEnum("usergroup")]
    UserGroup,
    /// <summary>
    /// Variable embedded properties.
    /// </summary>
    [zenonSerializableEnum("1")]
    VariableEmbeddedProperties,
    [zenonSerializableEnum("2")]
    VariableProfile,
    /// <summary>
    /// Array multi dimensions: readable: D1,D2[,D3]
    /// undefined if less than 2 dimensions.
    /// </summary>
    [zenonSerializableEnum("3")]
    VariableDimension,
    /// <summary>
    /// Hide variable: [ "&lt;EDIT&gt;" ] [ "&lt;SEL&gt;" ]
    /// </summary>
    [zenonSerializableEnum("4")]
    VariableHide,
    /// <summary>
    /// Function block diagram (hidden) - NULL or "FBDFLOW"
    /// </summary>
    [zenonSerializableEnum("5")]
    VariableFunctionBlockDiagramFlow,
    /// <summary>
    /// zenon PVID
    /// </summary>
    [zenonSerializableEnum("6")]
    VariablePvId,
    /// <summary>
    /// Shared for multitasking.
    /// </summary>
    [zenonSerializableEnum("7")]
    VariableShared,
    /// <summary>
    /// Unique ID for online change.
    /// </summary>
    [zenonSerializableEnum("8")]
    VariableUniqueId,
    /// <summary>
    /// Variable is shared on MT: [ Public | Extern ]
    /// </summary>
    [zenonSerializableEnum("9")]
    VariableMtShared,
    /// <summary>
    /// Task name of the owner (for externs).
    /// </summary>
    [zenonSerializableEnum("10")]
    VariableMtTask,
    /// <summary>
    /// Variable sub-groups (for retains and globals)
    /// </summary>
    [zenonSerializableEnum("11")]
    VariableSubGroups,
    /// <summary>
    /// The developer of this property was clear about the fact that no one should touch this property.
    /// </summary>
    [zenonSerializableEnum("12")]
    VariableXflags,
    /// <summary>
    /// Previous IO alias.
    /// </summary>
    [zenonSerializableEnum("13")]
    VariablePreviousAlias,
    /// <summary>
    /// Default alias which is used when the user deleted the current alias.
    /// </summary>
    [zenonSerializableEnum("14")]
    VariableDefaultAlias,
    /// <summary>
    /// User defined group name.
    /// </summary>
    [zenonSerializableEnum("256")]
    VariableUserGroupName1,
    /// <summary>
    /// User defined group name.
    /// </summary>
    [zenonSerializableEnum("257")]
    VariableUserGroupName2,
    /// <summary>
    /// User defined group name.
    /// </summary>
    [zenonSerializableEnum("258")]
    VariableUserGroupName3
  }
}
