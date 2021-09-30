namespace zenonApi.Zenon.K5Srv
{
  public class K5SrvConstants
  {
    public const string K5DbGroupNameGlobal = "(Global)";
    public const string K5DbGroupNameRentain = "(Retain)";

    // index of the first event text
    public const int K5DbEventTextIndex = 100;

    // first available id for declared objects
    public const int K5DbIdUser = 256; 

    // Client connection flags
    public const uint K5DbSelfNotif = 0x00000001; // flag: self-notifications wished

    #region Method return constants

    /// <summary>
    ///   Enum for the different K5Srv error codes
    /// </summary>
    public enum K5DbErr
    {
      Ok = 0, // OK

      Internal = 1, // internal error - not documented
      SameRename = 2, // rename with same name

      CreateProject = 3, // cant create project
      UnknownClient = 4, // invalid client handle
      UnknownProject = 5, // invalid project handle
      ProjectShared = 6, // project is shared - cant be deleted
      DeleteProject = 7, // cant delete project
      RenameProject = 8, // cant rename project
      CopyProject = 9, // cant rename project

      UnknownProgram = 10, // unknown program
      DuplicateProgram = 11, // program name already used
      BadProgramName = 12, // invalid program name
      DeleteParentProgram = 13, // parent program cant be deleted
      ProgramLocked = 14, // program is locked
      BadProgramMove = 15, // invalid program move in hierarchy
      TooManyPrograms = 16, // too many programs
      BadProgramSection = 17, // invalid section
      BadProgramLanguage = 18, // invalid language

      UnknownGroup = 19, // unknown group
      CantDeleteGroup = 20, // cant delete group (not an IO)
      GroupLocked = 21, // group is locked

      CantDeleteVariable = 22, // cant delete variable in an IO group
      UnknownVariable = 23, // unknown variable in group
      VariableLocked = 24, // variable is locked
      CantRenameVariable = 25, // cant rename variable in an IO group
      DuplicateVariableName = 26, // variable name already used
      BadVariableName = 27, // variable name already used
      UnknownType = 28, // unknow type
      BadIoType = 29, // invalid change of IO type
      IoVariableDimension = 30, // IO cant have no dim
      TypeDimension = 31, // type cant have a dim
      BadDimension = 32, // invalid dim
      NoStringLength = 33, // type cant have a string length
      BadStringLength = 34, // invalid string length
      CantChangeAttribute = 35, // invalid change of attribute
      CantChangeRo = 36, // RO attribute cant be changed
      CantAliasMemoryVariable = 37, // aliases are only for IOs
      CantInitFb = 38, // no init value for FB instances
      CantInitArray = 39, // no init value for arrays
      BadInitValue = 40, // invalid init value
      NoFbIoInit = 41, // FB In/Out cannot have an initial value
      FbIoVariableArray = 42, // FB in/out cannot have dimension

      UnknownObject = 43, // unknown object (in project)

      BadCommentLanguage = 44, // invalid text language identifier
      NoVariableMultiLineComment = 45, // no multiline comm for variables
      NoRetainInstance = 46, // no instances in RETAIN group

      ExportCantCreate = 47, // cant create export file
      ImportCantRead = 48, // cant open import file
      ImportBadFile = 49, // invalid import file
      ImportCreateFile = 50, // cant create import program
      ExportHideUdfb = 51, // cant hide exported source if not UDFB
      ExportHideNoCode = 52, // cant hide exported source if UDFB not compiled

      HotEnabled = 53, // invalid operation when On Line change is enabled
      Xv = 54, // invalid use of "external" attributes

      DlgAskCreateVar = 55, // DlgAskCreateVar
      DlgType = 56, // DlgType
      DlgYes = 57, // DlgYes
      DlgNo = 58, // DlgNo
      DlgCancel = 59, // DlgCancel
      DlgCreateVarKo = 60, // DlgCreateVarKO

      CantDeleteFolder = 61, // del non empty folder
      CantRenameFolder = 62, // folder name used
      CantMoveChildToFolder = 63, // cant move child program to folder
      CantMoveTopFolder = 64, // cant move root folder
      CantMoveFolderSame = 65, // cant move to the same folder
      CantMoveFolderRecurs = 66, // cant move to chil folder

      FileLocked = 67, // file is locked
      BadFileExtention = 68, // invalid file extension
      DuplicateFileName = 69, // file name already used
      BadFileName = 70, // invalid file name

      Crypted = 71, // program is protected

      DlgAskVar = 72, // DlgAskVar
      DlgAskCreate = 73, // DlgAskCreate
      DlgAskRename = 74, // DlgAskRename
      DlgRange = 75, // DlgRange

      BadEnum = 76, // invalid enumerated value(s)
      DuplicateTypeName = 77, // type name already used

      DlgVariables = 78, // variables
      DlgKeywords = 79, // keywords and functions
      DlgNoGroup = 80, // (no user group)
      DlgAll = 81, // (all)

      DlgLocalOnly = 82, // local variables only
      DlgNoInstance = 83, // no FB instance

      Global = 84, // "GLOBAL"
      Retain = 85, // "RETAIN"
      PromptDisableProgram = 86, // prompt to disable instead of delete when hot enabled
    }
    #endregion

    #region Object types
    /// <summary>
    ///   Enum for the different K5Srv files
    /// </summary>
    public enum K5DbObj
    {
      Client = 1, // client
      Project = 2, // open project
      Program = 3, // program
      Group = 4, // group
      Type = 5, // type (may refer to a UDFB)
      Var = 6, // variable
      Unknown = 7, // external object (not stored)
      Folder = 8, // program folders
      File = 9, // program folders
    }
    #endregion
    /// <summary>
    ///   Enum for the different K5Srv events
    /// </summary>
    public enum K5DbEvent : uint
    {
      Build = 0x80000001, // application built
      BuildOptions = 0x80000002, // build options changed
      Download = 0x80000003, // application downloaded
      LoadChange = 0x80000004, // application changes downloaded
      HotChange = 0x80000005, // on line change
      EqvCom = 0x80000006, // common defines changed
      EqvGlobal = 0x80000007, // global defines changed
      HistoryOn = 0x80000008, // history activated
      HistoryOff = 0x80000009, // history deactivated
      WatchFiles = 0x8000000a, // watch file(s) have changed - obsolete
      ConfigChanged = 0x8000000b, // active configuration has changed
      LibsChanged = 0x8000000c, // libraries have been updated
      BeforeLoop = 0x8000000d, // internal use
      AfterLoop = 0x8000000e, // internal use

      HotChanged = 0x000a0001, // hot settings have changed

      FileNew = 0x000e0001, // new file has been created
      FileRenamed = 0x000e0002, // file name has been changed
      FileDeleted = 0x000e0003, // file has been deleted
      FileLocked = 0x000e0004, // file has been locked
      FileUnlocked = 0x000e0005, // file has been unlocked
      FileChanged = 0x000e0006, // file contents has been changed
      FilesReloaded = 0x000e0007, // files have been re-scanned

      ProjectRenamed = 0x00010001, // project has been renamed / moved on disk
      ProjectReloaded = 0x00010002, // project has been reloaded

      ProgramNew = 0x00020001, // new program has been created
      ProgramDuplicatd = 0x00020002, // new program has been created by copy
      ProgramRenamed = 0x00020003, // program has been renamed
      ProgramDeleted = 0x00020004, // program has been deleted
      ProgramMoved = 0x00020005, // program has been moved
      ProgramCopied = 0x00020006, // program has been copied
      ProgramLocked = 0x00020007, // program has been locked for editing
      ProgramChanged = 0x00020008, // edited program has been changed and saved
      ProgramUnlocked = 0x00020009, // program has been unlocked after editing
      ProgramVariablesChanged = 0x0002000a, // local variables have changed

      CommentLanguageChanged = 0x00090001, // selected language has changed
      CommentChanged = 0x00090003, // a comment has changed

      TypeNew = 0x00050001, // new type has been created
      TypeRenamed = 0x00050003, // type has been renamed
      TypeDeleted = 0x00050004, // type has been deleted
      TypeChanged = 0x00050007, // type has been changed

      GroupNew = 0x00040001, // new group has been created
      GroupRenamed = 0x00040002, // group has been renamed
      GroupDeleted = 0x00040003, // group has been deleted
      GroupLocked = 0x00040004, // group has been locked
      GroupChanged = 0x00040005, // group has been changed (itself - not vars inside)
      GroupUnlocked = 0x00040006, // group has been unlocked
      GroupMoved = 0x00040007, // group has been moved

      GroupOwned = 0x00040008, // group + its vars has been locked
      GroupReleased = 0x00040009, // group + its vars has been locked

      K5PropertyChanged = 0x00080001, // K5 reserved property has changed
      ExternalPropertyChanged = 0x00080002, // external property has changed

      VariableNew = 0x00070001, // new variable has been created
      VariableRenamed = 0x00070002, // variable name or IO alias has been changed
      VariableDeleted = 0x00070003, // variable has been deleted
      VariableLocked = 0x00070004, // variable has been locked
      VariableChanged = 0x00070005, // variable has been changed
      VariableUnlocked = 0x00070006, // variable has been unlocked
      VariableMoved = 0x00070007, // variable has been moved within its parent group
      VariableBeginMove = 0x00070008, // variable will be moved (arg: hVar before)
      VariableEndMove = 0x00070009, // move is finished (arg: hVar after or NULL if fail)
    }

    /// <summary>
    ///   Enum for the different K5Srv sections
    /// </summary>
    public enum K5DbSection
    {
      Begin = 0x00000001, // BEGIN (non sfc)
      Sfc = 0x00000002, // SFC: main only
      End = 0x00000004, // END (non sfc)
      Child = 0x00000008, // SFC children (all)
      Udfb = 0x00000010, // UDFBs
      Main = 0x00000007, // all main programs - not UDFBs
      Prog = 0x0000000f, // all programs
      Any = 0x000000ff, // all programs and UDFBs
    }

    /// <summary>
    ///   Enum for the different K5Srv programming languages
    /// </summary>
    public enum K5DbLanguage : uint
    {
      // programming languages
      Sfc = 0x00000001, // SFC
      St = 0x00000002, // ST
      Fbd = 0x00000004, // FBD
      Ld = 0x00000008, // LD
      Il = 0x00000010, // IL
      FreeFormSfc = 0x00000020, // free form SFC
      Any = 0x0000001f, // any language
    }

    /// <summary>
    ///   Enum for the different K5Srv move actions
    /// </summary>
    public enum K5DbMoveProgram
    {
      Up = 1, // up in the same section
      Down = 2, // down in the same section
      Begin = 3, // to BEGIN section
      End = 4, // to END section
      SfcMain = 5, // to top of SFC section
      ChildOf = 6, // under the specified SFC parent program
      Before = 7, // before the specified program
      After = 8, // after the specified program
    }

    /// <summary>
    ///   Enum for the different K5Srv types
    /// </summary>
    public enum K5DbType
    {
      Invalid = 0x00001000, // invalid data type
      Basic = 0x00000001, // simple data type (predefined)
      String = 0x00000002, // string length is required
      Io = 0x00000004, // type can be used for an IO
      CStruct = 0x00000008, // structure defined in libray
      StandardFb = 0x00000010, // that's a standard or "C" function block
      Cfb = 0x00000020, // that's a standard or "C" function block
      Udfb = 0x00000040, // thats a UDFB
      Fb = 0x000000f0, // that's a function block or a UDFB
      Enum = 0x00000100, // enumeratade data type (user defined)
      BitField = 0x00000200, // bit field integer data type (user defined)

      Single = (Basic | Enum | BitField),
    }

    /// <summary>
    ///   Enum for the different K5Srv groups
    /// </summary>
    public enum K5DbGroup : uint
    {
      Global = 1, // global variables
      Retain = 2, // global RETAIN variables
      Input = 3, // input IO board
      Output = 4, // output IO board
      ComplexInput = 5, // input IO board within a complex device
      ComplexOutput = 6, // output IO board within a complex device
      Local = 7, // variables local to a program		
      Udfb = 8, // private data and parameters of a UDFB

      SortDefault = 0, // natural order
      SortIoLast = 1, // IOs are the last ones
      SortNatural = 2, // natural order - sorted by class
      SortLocal = 0x80000000, // 1 local first - OR combined with local group handle
    }

    /// <summary>
    ///   Enum for the different K5Srv max values
    /// </summary>
    public enum K5DbMax
    {
      Dim = 65534,
      String = 255,
    }

    /// <summary>
    ///   Enum for the different K5Srv variable properties
    /// </summary>
    public enum K5DbVar
    {
      ReadOnly = 0x00000001, // read only attribute (always TRUE for inputs)
      Input = 0x00000002, // physical input
      Output = 0x00000004, // physical output
      FbInput = 0x00000008, // input parameter of a FB or UDFB
      FbOutput = 0x00000010, // output parameter of a FB or UDFB
      Extern = 0x00000020, // shared external variable
      InOut = 0x00000040, // FB input is INOUT
      Added = 0x00000100, // added for on line change
      Deleted = 0x00000200, // deleted for on line change
      LocalRetain = 0x00000400, // local RETAIN flag

      FindExact = 0x00000001, // find exact name
      FindPvid = 0x00000002, // find by PVID

      SortByName = 1, // sort by variable name
      SortByType = 2, // sort by data type
      SortByAttribute = 3, // sort by attribute
      SortByProperties = 4, // sort by properties (profile & embedded)
    }

    /// <summary>
    ///   Enum for the different K5Srv move parameters
    /// </summary>
    public enum KbDbMoveParam
    {
      Up = 1, // move command: FB parameter UP
      Down = 2, // move command: FB parameter DOWN
      ToEnd = 3, // move command: move to end of group (vars only)
    }

    /// <summary>
    ///   Enum for the different K5Srv properties
    /// </summary>
    public enum K5DbProperty
    {
      User = 4096, // 1rst available prop ID for external apps

      // predefined properties - project
      CycleTiming = 1, // cycle triggering (project's property)
      TargetSizing = 2, // target memory sizing for On Line Change (syntax in RTL)
      EnableHot = 3, // enable on line change (when defined)
      LastChange = 4, // date stamp of last change bad for on line change
      ProjectHistory = 5, // project history settings
      Versioning = 6, // project version info
      IoChannelBase = 7, // base index for IO channels
      TargetCtSizing = 8, // min size of CT segment (for On Line Change)
      Config = 9, // active configuration name
      IsLibrary = 10, // non null if project is library
      LastLibraryUpdate = 11, // time stamp of last updates of libraries
      PaScript = 12, // no null if PA scripting project
      FbUndef = 13, // list of undef fieldbuses (\t separated)
      CommunicationSetting = 14, // communication settings
      BindPort = 15, // port use for the binding
      CommunicationDriver = 16, // communication driver
      K5MonitoringDestination = 17, // Monitoring builder default destination folder
      NonIecSyb = 18, // allow non IEC variable names
      OnlineConstants = 19, // Constants and init values can be changed On Line
      PrjVarUid = 20, // next unique ID for On Line Change
      DdkHide = 21, // list of hidden configs (DLL prefixes, separated by ',')
      LibList = 128, // properties after are library folders
      Folder = 256, // properties after this number are reserved to K5DBSRV

      // predefined properties - programs and types
      ProgramSchedule = 1, // program scheduling: '%period=offset'
      ProgramOnCall = 2, // program flag: 'on call' exec mode
      ProgramTask = 3, // task name (NULL = default cycle)
      UdfbStructure = 4, // UDFB is a data structure
      ProgramCheck = 5, // program validity: "#FALSE#" if not valid
      ProgramPath = 6, // path in folder tree
      ProgramVisible = 7, // reserved for WB
      ProgramFamily = 8, // group family name
      Library = 9, // library path
      Crypt = 10, // program has been crypred
      Hidden = 11, // program is hidden in workspace
      InstianciableSfc = 12, // instianciable SFC langguage
      ProgramOemEdit = 13, // OEM dll for editing
      ProgramColor = 14, // optional RGB color
      UdfbSama = 15, // UDFB is usable in SAMA
      UdfbSamaShape1 = 16, // customized shape for SAMA UDFB
      UdfbSamaShape2 = 17, // customized shape for SAMA UDFB
      UdfbSamaShape3 = 18, // customized shape for SAMA UDFB
      UdfbSamaShape4 = 19, // customized shape for SAMA UDFB
      UdfbSamaShape5 = 20, // customized shape for SAMA UDFB
      UdfbSamaShape6 = 21, // customized shape for SAMA UDFB
      UdfbSamaShape7 = 22, // customized shape for SAMA UDFB
      UdfbSamaShape8 = 23, // customized shape for SAMA UDFB
      ProgramOwned = 40, // this program is managed by another part of straton

      // predefined properties - IO groups
      IoGroupKey = 1, // driver key for an IO board
      IoGroupOem = 2, // first OEM defined property
      IoGroupName = 256, // driver name for an IO board / name of a complex
      IoParentName = 257, // parent driver name for an complex IO board
      IoVirtual = 258, // parent driver name for an complex IO board

      // predefined properties - variables
      VariableEmbedded = 1, // variable embedded properties
      VariableProfile = 2, // variable profile

      VariableDimension = 3, // array multi dimensions: readable: D1,D2[,D3]

      // undefined if less than 2 dimensions
      VariableHide = 4, // hide variable: [ "<EDIT>" ] [ "<SEL>" ]
      VariableFdbFlow = 5, // FBD flow (hidden) - NULL or "FBDFLOW"
      VariablePvid = 6, // zenon PVID
      VariableShared = 7, // shared for multitasking
      VariableUid = 8, // unique ID for On Line Change
      VariableUserGroup1 = 256, // user defined group name
      VariableUserGroup2 = 257, // user defined group name
      VariableUserGroup3 = 258, // user defined group name
    }

    /// <summary>
    ///   Enum for the different K5Srv comments
    /// </summary>
    public enum K5DbComment
    {
      Short = 1, // short tag for graphic languages
      Long = 2, // long description text
      Multiline = 3, // multiline description text
    }

    /// <summary>
    ///   Enum for the different K5Srv variable selections
    /// </summary>
    public enum K5DbVariableSelection
    {
      NoTextSelected = 0x00000001, // no text selected if edit box
      CreateVariable = 0x00000002, // create var if not existing
      Debug = 0x00000004, // debug mode (False=edit)
      FocusEdit = 0x00000008, // focus edit box at open
      DefaultPosition = 0x00000010, // dont apply window position
      Complete = 0x00000020, // completion mode (filter possible vars)
      ShowAlways = 0x00000040, // always display box (even if 1 choice)
      CreateOnly = 0x00000080, // no selection - check for exist and create
      FilterType = 0x00000100, // use preferred type for filtering list
      FromList = 0x00000200, // list of items passed by the caller
    }
  }
}
