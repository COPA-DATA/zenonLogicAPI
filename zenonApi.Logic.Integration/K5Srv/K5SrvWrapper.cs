using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace zenonApi.Zenon.K5Srv
{
  public partial class K5SrvWrapper : IDisposable
  {
    private bool isDisposed = false;
    private uint _clientHandle;
    private uint _projectHandle;
    private K5SrvWrapper() {}

    /// <summary>
    ///   Open a connection to the logic database
    /// </summary>
    /// <returns> K5SvrWrapper object if successful</returns>
    /// <param name="handleWindowCallback">Window callback of current application.</param>
    /// <param name="messageCallback">Message callback handle.</param>
    /// <param name="projectPath">Path to the zenon logic project.</param>
    /// <param name="clientName">Name of the accessing application.</param>
    /// <param name="flags">Flags to specify connection.</param>
    public static K5SrvWrapper TryConnect(IntPtr handleWindowCallback, uint messageCallback, string projectPath, string clientName, uint flags)
    {
      K5SrvWrapper srv = new K5SrvWrapper();

      if (srv.Open(handleWindowCallback, messageCallback, projectPath, clientName, flags))
      {
        return srv;
      }
      
      return null;
    }

    /// <summary>
    ///   Open a connection to the logic database
    /// </summary>
    /// <returns> Bool, true if successful</returns>
    /// <param name="handleWindowCallback">Window callback of current application.</param>
    /// <param name="messageCallback">Message callback handle.</param>
    /// <param name="projectPath">Path to the zenon logic project.</param>
    /// <param name="clientName">Name of the accessing application.</param>
    /// <param name="flags">Flags to specify connection.</param>
    public bool Open(IntPtr handleWindowCallback, uint messageCallback, string projectPath, string clientName, uint flags)
    {
      Close();

      _clientHandle = DbSrv.Connect(handleWindowCallback, messageCallback, flags, 0, clientName);
      if (_clientHandle != 0)
      {
        _projectHandle = DbSrv.OpenProject(_clientHandle, projectPath);
        if (_projectHandle == 0)
        {
          DbSrv.Disconnect(_clientHandle);
          _clientHandle = 0;
        }
      }

      return IsReady;
    }

    public bool OpenQuiet(string projectPath)
    {
      IntPtr ptr = new IntPtr(0);
      return Open(ptr, 0, projectPath, "", 0);
    }

    /// <summary>
    ///   Closes the connection
    /// </summary>
    public void Close()
    {
      if (_projectHandle != 0)
      {
        DbSrv.CloseProject(_clientHandle, _projectHandle);
        _projectHandle = 0;
      }

      if (_clientHandle != 0)
      {
        DbSrv.Disconnect(_clientHandle);
        _clientHandle = 0;
      }
    }

    /// <summary>
    ///   Handle of the logic project
    /// </summary>
    public uint ProjectHandle => _projectHandle;

    /// <summary>
    ///   Handle of the client
    /// </summary>
    public uint ClientHandle => _clientHandle;

    /// <summary>
    ///   Indicates if project can be accessed
    /// </summary>
    public bool IsReady => _projectHandle != 0;

    /// <summary>
    ///   Name of the client
    /// </summary>
    public string ClientName => DbSrv.GetClientName(_clientHandle);

    /// <summary>
    ///   Indicates if project is a library
    /// </summary>
    public bool IsLibrary
    {
      get
      {
        if (_projectHandle == 0)
          return false;

        CheckIfDisposed();

        string property = DbSrv.GetProperty(_clientHandle, _projectHandle, _projectHandle, K5SrvConstants.K5DbProperty.IsLibrary);

        return property != null;
      }
    }

    /// <summary>
    ///   Indicates if project is a PA script
    /// </summary>
    public bool IsPaScript
    {
      get
      {
        if (_projectHandle == 0)
          return false;

        CheckIfDisposed();

        string property = DbSrv.GetProperty(_clientHandle, _projectHandle, _projectHandle,
          K5SrvConstants.K5DbProperty.PaScript);

        return property != null;
      }
    }

    /// <summary>
    ///   Returns the object type of the object
    /// </summary>
    /// <returns> Type of object</returns>
    /// <param name="id">ObjectID.</param>
    public K5SrvConstants.K5DbObj GetKindOfObject(uint id)
    {
      CheckIfDisposed();
      return (K5SrvConstants.K5DbObj)DbSrv.GetKindOfObject(id);
    }

    /// <summary>
    ///   Checks if the object is locked
    /// </summary>
    public bool IsObjectLocked(uint id)
    {
      CheckIfDisposed();
      return DbSrv.IsObjectLocked(id) != 0;
    }


    public uint CheckSymbol(string name)
    {
      CheckIfDisposed();
      return DbSrv.CheckSymbol(name);
    }

    public string GetGhostName(uint id)
    {
      CheckIfDisposed();
      return DbSrv.GetGhostName(id);
    }

    public string GetMessage(uint messageId)
    {
      CheckIfDisposed();
      return DbSrv.GetMessage(messageId);
    }

    public string GetEventDescription(uint eventId, ref uint flags)
    {
      CheckIfDisposed();
      return DbSrv.GetEventDesc(eventId, ref flags);
    }

    public bool DispatchEvent(uint eventId, uint arguments)
    {
      CheckIfDisposed();
      return DbSrv.DispatchEvent(_clientHandle, _projectHandle, eventId, arguments) == (uint) K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Enables events in the logic project
    /// </summary>
    public uint EnableEvents(uint enable)
    {
      CheckIfDisposed();
      return DbSrv.EnableEvents(_clientHandle, _projectHandle, enable);
    }

    /// <summary>
    ///   Return the path of the logic project
    /// </summary>
    public string GetProjectPath()
    {
      CheckIfDisposed();

      StringBuilder pathBuild = new StringBuilder(256);
      DbSrv.GetProjectPath(_clientHandle, _projectHandle, pathBuild);

      string path = pathBuild.ToString();
      return path;
    }

    /// <summary>
    ///   Saves changes in the logic project
    /// </summary>
    public bool SaveProject()
    {
      CheckIfDisposed();
      return DbSrv.SaveProject(_clientHandle, _projectHandle) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    public uint ProjectTimeStamp()
    {
      CheckIfDisposed();
      return DbSrv.GetProjectDateStamp(_clientHandle, _projectHandle);
    }

    /// <summary>
    ///   Sets project state to modified
    /// </summary>
    public bool SetProjectModified()
    {
      CheckIfDisposed();
      return DbSrv.SetProjectModified(_clientHandle, _projectHandle) == (uint)K5SrvConstants.K5DbErr.Ok;
    } 

    /// <summary>
    ///   Requests the program list from the database which are available after this method executed in parameter
    /// </summary>
    /// <returns>List which contains the found programs</returns>
    /// <param name="section">The section of which programs shall be retrieved.</param>
    public List<DbProgram> GetPrograms(K5SrvConstants.K5DbSection section)
    {
      CheckIfDisposed();

      List<DbProgram> programs = new List<DbProgram>();

      if (_projectHandle == 0)
        return programs;

      uint nbProg = DbSrv.GetNbProgram(_clientHandle, _projectHandle, (uint)section);
      if (nbProg == 0)
        return programs;

      //create program array
      uint[] arrPrg = new uint[nbProg];
      DbSrv.GetPrograms(_clientHandle, _projectHandle, (uint)section, arrPrg);

      for (uint uPrg = 0; uPrg < nbProg; uPrg++)
      {
        DbProgram prog = new DbProgram();
        programs.Add(prog);
        GetProgramDesc(arrPrg[uPrg], ref prog);
      }

      return programs;
    }

    /// <summary>
    ///   Get description of the specified program
    /// </summary>
    /// <returns>Bool, true if description found</returns>
    /// <param name="programHandle">The program handle.</param>
    /// <param name="dbProgram">The program (object DbProgram).</param>
    public bool GetProgramDesc(uint programHandle, ref DbProgram dbProgram)
    {
      CheckIfDisposed();

      dbProgram.ProgramId = programHandle;

      IntPtr ptrName = DbSrv.GetProgramDesc(_clientHandle, _projectHandle, programHandle, ref dbProgram.Language,
        ref dbProgram.Section, ref dbProgram.ParentHandle);
      dbProgram.Name = Marshal.PtrToStringAnsi(ptrName);

      StringBuilder strPath = new StringBuilder(256);
      DbSrv.GetProgramPath(_clientHandle, _projectHandle, programHandle, strPath);
      dbProgram.Path = strPath.ToString();

      return dbProgram.Name != null;
    }

    /// <summary>
    ///   Get children of a parent SFC
    /// </summary>
    /// <returns>List which contains the found children</returns>
    /// <param name="programHandle">The parent sfc program.</param>
    public List<DbProgram> GetChildren(uint programHandle)
    {
      CheckIfDisposed();

      List<DbProgram> programList = new List<DbProgram>();

      if (_projectHandle == 0)
        return programList;

      uint nbChild = DbSrv.GetNbChildren(_clientHandle, _projectHandle, programHandle);
      if (nbChild == 0)
        return programList;

      uint[] arrChild = new uint[nbChild];
      DbSrv.GetChildren(_clientHandle, _projectHandle, programHandle, arrChild);
      for (uint uChild = 0; uChild < nbChild; uChild++)
      {
        DbProgram progChild = new DbProgram();
        programList.Add(progChild);
        GetProgramDesc(arrChild[uChild], ref progChild);
      }

      return programList;
    }

    /// <summary>
    ///   Gets the related program id of a group or type
    /// </summary>
    /// <returns>ID of the program</returns>
    /// <param name="id">ID of group or type.</param>
    public uint GetRelatedProgramId(uint id) // group or type
    {
      CheckIfDisposed();

      uint programId = 0;
      switch (GetKindOfObject(id))
      {
        case K5SrvConstants.K5DbObj.Program:
          programId = id;
          break;

        case K5SrvConstants.K5DbObj.Group:
          DbGroup group = new DbGroup();
          if (GetGroupDesc(id, ref group))
            programId = FindProgram(group.Name);
          break;

        case K5SrvConstants.K5DbObj.Type:
          DbType type = new DbType();
          if (GetTypeDesc(id, ref type))
            programId = FindProgram(type.Name);
          break;
      }

      return programId;
    }

    /// <summary>
    ///   Checks if a object is a structure
    /// </summary>
    /// <returns> Bool, true if successful</returns>
    /// <param name="id">ID of object.</param>
    public bool IsStructure(uint id)
    {
      CheckIfDisposed();

      id = GetRelatedProgramId(id);
      string prop = GetProperty(id, K5SrvConstants.K5DbProperty.UdfbStructure);

      return prop != null;
    }

    /// <summary>
    ///   Returns program id
    /// </summary>
    /// <param name="programName">Name of the program.</param>
    public uint FindProgram(string programName)
    {
      CheckIfDisposed();

      return DbSrv.FindProgram(_clientHandle, _projectHandle, programName);
    }

    /// <summary>
    ///   Checks if a program can be created
    /// </summary>
    /// <param name="language">Language of the program.</param>
    /// <param name="section">Section of the program.</param>
    /// <param name="handleParent">Handle of the parent object.</param>
    /// <param name="programName">Program name.</param>
    public bool CanCreateProgram(K5SrvConstants.K5DbLanguage language, K5SrvConstants.K5DbSection section, uint handleParent, string programName)
    {
      CheckIfDisposed();
      return DbSrv.CanCreateProgram(_clientHandle, _projectHandle, (uint)language, (uint)section, handleParent, programName) ==
             (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Creates a program in zenon logic
    /// </summary>
    /// <returns> ID of the program</returns>
    /// <param name="language">Language of the program.</param>
    /// <param name="section">Section of the program.</param>
    /// <param name="handleParent">Handle of the parent object.</param>
    /// <param name="programName">Program name.</param>
    public uint CreateProgram(uint languageId, uint sectionId, uint handleParent, string programName)
    {
      CheckIfDisposed();
      return DbSrv.CreateProgram(_clientHandle, _projectHandle, languageId, sectionId, handleParent, programName);
    }

    /// <summary>
    ///   Copies a program
    /// </summary>
    /// <returns> ID of the program</returns>
    /// <param name="handleProgram">Handle of the existing program.</param>
    /// <param name="section">Section of the program.</param>
    /// <param name="handleParent">Handle of the parent object.</param>
    /// <param name="programName">Program name.</param>
    public uint CopyProgram(uint handleProgram, uint section, uint handleParent, string programName)
    {
      CheckIfDisposed();
      return DbSrv.CopyProgram(_clientHandle, _projectHandle, handleProgram, section, handleParent, programName);
    }

    /// <summary>
    ///   Renames a program
    /// </summary>
    /// <param name="handleProgram">Handle of the existing program.</param>
    /// <param name="newName">New program name.</param>
    public bool RenameProgram(uint handleProgram, string newName)
    {
      CheckIfDisposed();
      return DbSrv.RenameProgram(_clientHandle, _projectHandle, handleProgram, newName) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Moves the program
    /// </summary>
    /// <param name="handleProgram">Handle of the program.</param>
    /// <param name="move">Move location.</param>
    /// <param name="handleParent">Handle of the parent object.</param>
    public bool MoveProgram(uint handleProgram, K5SrvConstants.K5DbMoveProgram move, uint handleParent)
    {
      CheckIfDisposed();
      return DbSrv.MoveProgram(_clientHandle, _projectHandle, handleProgram, (uint)move, handleParent) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Deletes a program
    /// </summary>
    /// <returns> ID of the program</returns>
    /// <param name="handleProgram">Handle of the program.</param>
    public bool DeleteProgram(uint handleProgram)
    {
      CheckIfDisposed();
      return DbSrv.DeleteProgram(_clientHandle, _projectHandle, handleProgram) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Locks the program
    /// </summary>
    /// <returns> ID of the program</returns>
    /// <param name="handleProgram">Handle of the program.</param>
    public bool LockProgram(uint handleProgram)
    {
      CheckIfDisposed();
      return DbSrv.LockProgram(_clientHandle, _projectHandle, handleProgram) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Saves changes of the program
    /// </summary>
    /// <returns> ID of the program</returns>
    /// <param name="handleProgram">Handle of the program.</param>
    public bool SaveProgramChanges(uint handleProgram)
    {
      CheckIfDisposed();
      return DbSrv.SaveProgramChanges(_clientHandle, _projectHandle, handleProgram) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Unlocks the program
    /// </summary>
    /// <returns> ID of the program</returns>
    /// <param name="handleProgram">Handle of the program.</param>
    public bool UnlockProgram(uint handleProgram)
    {
      CheckIfDisposed();
      return DbSrv.UnlockProgram(_clientHandle, _projectHandle, handleProgram) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Return the owner of the program
    /// </summary>
    /// <returns> ID of the program</returns>
    /// <param name="handleProgram">Handle of the program.</param>
    public string GetProgramOwner(uint handleProgram)
    {
      CheckIfDisposed();
      return DbSrv.GetProgramOwner(_clientHandle, _projectHandle, handleProgram);
    }
     
    public bool GetProgramSchedule(uint handleProgram, ref uint period, ref uint offset)
    {
      CheckIfDisposed();
      return DbSrv.GetProgramSchedule(_clientHandle, _projectHandle, handleProgram, ref period, ref offset) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    public bool SetProgramSchedule(uint handleProgram, uint period, uint offset)
    {
      CheckIfDisposed();
      return DbSrv.SetProgramSchedule(_clientHandle, _projectHandle, handleProgram, period, offset) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    public bool GetProgramOnCallFlag(uint handleProgram)
    {
      CheckIfDisposed();
      return DbSrv.GetProgramOnCallFlag(_clientHandle, _projectHandle, handleProgram) != 0;
    }

    public bool SetProgramOnCallFlag(uint handleProgram, uint onCallFlag)
    {
      CheckIfDisposed();
      return DbSrv.SetProgramOnCallFlag(_clientHandle, _projectHandle, handleProgram, onCallFlag) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Exports a program
    /// </summary>
    /// <returns> ID of the program</returns>
    /// <param name="handleProgram">Handle of the program.</param>
    /// <param name="exportFileName">Path of the exported file.</param>
    public bool ExportProgram(uint handleProgram, string exportFileName)
    {
      CheckIfDisposed();
      return DbSrv.ExportProgram(_clientHandle, _projectHandle, handleProgram, exportFileName) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Imports a program
    /// </summary>
    /// <returns> ID of the program</returns>
    /// <param name="importFileName">Path of the program to import.</param>
    /// <param name="wishedName">Name of the program</param>
    public bool ImportProgram(string importFileName, string wishedName)
    {
      CheckIfDisposed();
      return DbSrv.ImportProgram(_clientHandle, _projectHandle, importFileName, wishedName) == (uint)K5SrvConstants.K5DbErr.Ok;
    }


    public bool GetImportProgramName(string importFileName, StringBuilder nameBuffer, uint bufferSize)
    {
      CheckIfDisposed();
      return DbSrv.GetImportProgramName(_clientHandle, importFileName, nameBuffer, bufferSize) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///  Get the type list in database
    /// </summary>
    /// <returns>List which contains the found types</returns>
    public List<DbType> GetTypes()
    {
      CheckIfDisposed();

      List<DbType> typeList = new List<DbType>();

      if (_projectHandle == 0)
        return typeList;

      uint nbType = DbSrv.GetNbType(_clientHandle, _projectHandle);
      if (nbType == 0)
        return typeList;

      //create type array
      uint[] arrType = new uint[nbType];
      DbSrv.GetTypes(_clientHandle, _projectHandle, arrType);

      for (uint uType = 0; uType < nbType; uType++)
      {
        DbType type = new DbType();
        typeList.Add(type);
        GetTypeDesc(arrType[uType], ref type);
      }

      return typeList;
    }

    /// <summary>
    ///   Get description of the specified type
    /// </summary>
    /// <returns> Bool, true if description found</returns>
    /// <param name="typeHandle">The type handle.</param>
    /// <param name="type">The type (object DbType).</param>
    public bool GetTypeDesc(uint typeHandle, ref DbType type)
    {
      CheckIfDisposed();

      type.TypeId = typeHandle;

      IntPtr ptrName = DbSrv.GetTypeDesc(_clientHandle, _projectHandle, typeHandle, ref type.Flags);
      type.Name = Marshal.PtrToStringAnsi(ptrName);

      type.UdfbHandle = DbSrv.GetTypeUDFB(_clientHandle, _projectHandle, typeHandle);
      DbSrv.GetUDFBNbIO(_clientHandle, _projectHandle, typeHandle, ref type.In, ref type.Out);

      uint nbParam = DbSrv.GetNbTypeParam(_clientHandle, _projectHandle, typeHandle);
      if (nbParam != 0)
      {
        uint[] arrParam = new uint[nbParam];
        DbSrv.GetTypeParams(_clientHandle, _projectHandle, typeHandle, arrParam);

        for (uint uParam = 0; uParam < nbParam; uParam++)
        {
          DbVar var = new DbVar();
          type.ParameterList.Add(var);
          GetVarDesc(arrParam[uParam], ref var);
        }
      }

      return type.Name != null;
    }

    /// <summary>
    ///   Searches the defined type
    /// </summary>
    /// <returns> ID of the found type</returns>
    /// <param name="type">Name of the type.</param>
    public uint FindType(string type)
    {
      CheckIfDisposed();
      return DbSrv.FindType(_clientHandle, _projectHandle, type);
    }

    /// <summary>
    ///   Renames a type
    /// </summary>
    /// <param name="typeHandle">Handle of the type.</param>
    /// <param name="newName">New name of the type.</param>
    public bool RenameType(uint typeHandle, string newName)
    {
      CheckIfDisposed();
      return DbSrv.RenameType(_clientHandle, _projectHandle, typeHandle, newName) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Deletes the type
    /// </summary>
    /// <param name="typeHandle">Handle of the type to rename.</param>
    public bool DeleteType(uint typeHandle)
    {
      CheckIfDisposed();
      return DbSrv.DeleteType(_clientHandle, _projectHandle, typeHandle) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Creates an enum type
    /// </summary>
    /// <returns> ID of the created enum</returns>
    /// <param name="name">Name of the enum.</param>
    /// <param name="values">Values of the enum.</param>
    public uint CreateEnumType(string name, string values)
    {
      CheckIfDisposed();
      return DbSrv.CreateEnumType(_clientHandle, _projectHandle, name, values);
    }

    /// <summary>
    ///   Returns the values of an enum type
    /// </summary>
    /// <param name="typeHandle">Handle of the enum type.</param>
    public string GetEnumTypeValues(uint typeHandle)
    {
      CheckIfDisposed();
      IntPtr ptr = DbSrv.GetEnumTypeValues(_clientHandle, _projectHandle, typeHandle);
      return Marshal.PtrToStringAnsi(ptr);
    }

    /// <summary>
    ///   Sets values of an enum type
    /// </summary>
    /// <param name="typeHandle">Handle of the enum type.</param>
    /// <param name="values">values of the enum.</param>
    public bool SetEnumTypeValues(uint typeHandle, string values)
    {
      CheckIfDisposed();
      return DbSrv.SetEnumTypeValues(_clientHandle, _projectHandle, typeHandle, values) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    public uint CreateBitFieldType(uint baseTypeHandle, string name, string bits)
    {
      CheckIfDisposed();
      return DbSrv.CreateBitFieldType(_clientHandle, _projectHandle, baseTypeHandle, name, bits);
    }

    public string GetBitFieldType(uint typeHandle, ref uint baseTypeHandle)
    {
      CheckIfDisposed();
      IntPtr ptr = DbSrv.GetBitFieldType(_clientHandle, _projectHandle, typeHandle, ref baseTypeHandle);
      return Marshal.PtrToStringAnsi(ptr);
    }

    public bool SetBitFieldType(uint typeHandle, uint baseTypeHandle, string bits)
    {
      CheckIfDisposed();
      return DbSrv.SetBitFieldType(_clientHandle, _projectHandle, typeHandle, baseTypeHandle, bits) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Get the group list in database
    /// </summary>
    /// <returns> List which contains the found groups</returns>
    /// <param name="sortMethod">The sorting method to retrieve groups.</param>
    public List<DbGroup> GetGroups(K5SrvConstants.K5DbGroup sortMethod = K5SrvConstants.K5DbGroup.SortDefault)
    {
      CheckIfDisposed();

      List<DbGroup> groupList = new List<DbGroup>();

      if (_projectHandle == 0)
        return groupList;

      uint nbGroup = DbSrv.GetNbGroup(_clientHandle, _projectHandle);
      if (nbGroup == 0)
        return groupList;

      //create group array
      uint[] arrGroup = new uint[nbGroup];
      DbSrv.GetSortedGroups(_clientHandle, _projectHandle, arrGroup, (uint)sortMethod);

      for (uint uGroup = 0; uGroup < nbGroup; uGroup++)
      {
        DbGroup group = new DbGroup();
        groupList.Add(group);
        GetGroupDesc(arrGroup[uGroup], ref group);
      }

      return groupList;
    }

    /// <summary>
    ///   Get description of the specified group
    /// </summary>
    /// <returns> Bool, true if description found</returns>
    /// <param name="groupHandle">The group handle.</param>
    /// <param name="group">The group (object DbGroup).</param>
    public bool GetGroupDesc(uint groupHandle, ref DbGroup group)
    {
      CheckIfDisposed();

      group.Id = groupHandle;

      IntPtr ptrName = DbSrv.GetGroupDesc(_clientHandle, _projectHandle, groupHandle, ref group.GroupStyle,
        ref group.IoType, ref group.Slot, ref group.SubSlot, ref group.Var);
      group.Name = Marshal.PtrToStringAnsi(ptrName);

      return group.Name != null;
    }

    /// <summary>
    ///   Returns the group id
    /// </summary>
    /// <param name="group">Name of the group.</param>
    public uint FindGroup(string group)
    {
      CheckIfDisposed();
      return DbSrv.FindGroup(_clientHandle, _projectHandle, group);
    }

    /// <summary>
    ///   Locks the group
    /// </summary>
    public bool LockGroup(uint groupHandle, bool groupLock, bool withVar = false)
    {
      CheckIfDisposed();

      bool bSet;

      if (groupLock)
      {
        if (withVar)
          bSet = DbSrv.LockGroupEx(_clientHandle, _projectHandle, groupHandle) == (uint)K5SrvConstants.K5DbErr.Ok;
        else
          bSet = DbSrv.LockGroup(_clientHandle, _projectHandle, groupHandle) == (uint)K5SrvConstants.K5DbErr.Ok;
      }
      else
      {
        if (withVar)
          bSet = DbSrv.UnlockGroupEx(_clientHandle, _projectHandle, groupHandle) == (uint)K5SrvConstants.K5DbErr.Ok;
        else
          bSet = DbSrv.UnlockGroup(_clientHandle, _projectHandle, groupHandle) == (uint)K5SrvConstants.K5DbErr.Ok;
      }

      return bSet;
    }

    /// <summary>
    ///   Gets the related group id from a program or type
    /// </summary>
    /// <param name="id">Id of the program or type.</param>
    public uint GetRelatedGroupId(uint id) // program or type
    {
      CheckIfDisposed();

      uint group = 0;

      switch (GetKindOfObject(id))
      {
        case K5SrvConstants.K5DbObj.Program:
          DbProgram prog = new DbProgram();
          if (GetProgramDesc(id, ref prog))
            group = FindGroup(prog.Name);
          break;

        case K5SrvConstants.K5DbObj.Group:
          group = id;
          break;

        case K5SrvConstants.K5DbObj.Type:
          DbType type = new DbType();
          if (GetTypeDesc(id, ref type))
            group = FindGroup(type.Name);
          break;
      }

      return group;
    }

    /// <summary>
    ///   Gets the owner of a group
    /// </summary>
    /// <param name="groupHandle">Handle of the group.</param>
    public string GetGroupOwner(uint groupHandle)
    {
      CheckIfDisposed();
      return DbSrv.GetGroupOwner(_clientHandle, _projectHandle, groupHandle);
    }

    /// <summary>
    ///   Get the var list in group from database
    /// </summary>
    /// <returns> List which contains the found variables.</returns>
    /// <param name="groupHandle">The group where variables are declared.</param>
    /// <param name="sortMethod">The sorting method to retrieve variables.</param>
    public List<DbVar> GetVars(uint groupHandle, K5SrvConstants.K5DbVar sortMethod = K5SrvConstants.K5DbVar.SortByName)
    {
      CheckIfDisposed();

      List<DbVar> variableList = new List<DbVar>();

      if (_projectHandle == 0)
        return variableList;

      uint nbVar = DbSrv.GetNbVar(_clientHandle, _projectHandle, groupHandle);
      if (nbVar == 0)
        return variableList;

      //create group array
      uint[] arrVar = new uint[nbVar];
      DbSrv.GetSortedVars(_clientHandle, _projectHandle, groupHandle, (uint)sortMethod, arrVar);

      for (uint uVar = 0; uVar < nbVar; uVar++)
      {
        DbVar var = new DbVar();
        variableList.Add(var);
        GetVarDesc(arrVar[uVar], ref var);
      }

      return variableList;
    }

    /// <summary>
    ///   Get description of the specified variable
    /// </summary>
    /// <returns> Bool, true if description found.</returns>
    /// <param name="variableHandle">The variable handle.</param>
    /// <param name="var">The variable (object DbVar).</param>
    public bool GetVarDesc(uint variableHandle, ref DbVar var)
    {
      CheckIfDisposed();

      var.Id = variableHandle;

      uint groupHandle = GetVarGroup(variableHandle);
      IntPtr ptrName = DbSrv.GetVarDesc(_clientHandle, _projectHandle, groupHandle, variableHandle, ref var.Type, ref var.Dim,
        ref var.StringLength, ref var.Flags);
      var.Name = Marshal.PtrToStringAnsi(ptrName);

      uint dwValid = 0;
      var.InitialValue = GetVarInitValue(groupHandle, variableHandle, ref dwValid);

      return var.Name != null;
    }

    /// <summary>
    ///   Gets the group of a variable
    /// </summary>
    /// <param name="variableHandle">Handle of the variable.</param>
    public uint GetVarGroup(uint variableHandle)
    {
      CheckIfDisposed();
      return DbSrv.GetVarGroup(_clientHandle, _projectHandle, variableHandle);
    }

    /// <summary>
    ///   Searches a variable in a group and returns variable id
    /// </summary>
    /// <param name="groupHandle">Handle of the group.</param>
    /// <param name="varName">Variable name.</param>
    public uint FindVarInGroup(uint groupHandle, string varName)
    {
      CheckIfDisposed();
      return DbSrv.FindVarInGroup(_clientHandle, _projectHandle, groupHandle, varName);
    }

    /// <summary>
    ///   Searches variable in project
    /// </summary>
    /// <param name="name"Variable name.</param>
    public uint FindVarAnywhere(string name, uint localGroupHandle = 0, bool findExact = false)
    {
      CheckIfDisposed();

      uint flags = 0;

      if (findExact)
        flags = (uint)K5SrvConstants.K5DbVar.FindExact;
      return DbSrv.FindVar(_clientHandle, _projectHandle, name, 0xFFFFFFFF, localGroupHandle, flags);
    }

    /// <summary>
    ///   Creates variable
    /// </summary>
    /// <param name="groupHandle">Handle of group where variable is created.</param>
    /// <param name="typeHandle">Handle of the variable type.</param>
    /// <param name="dimension">Dimension.</param>
    /// <param name="stringLength">Max string length.</param>
    /// <param name="flags">Flags.</param>
    /// <param name="varName">Handle of the variable type.</param>
    public uint CreateVar(uint groupHandle, uint typeHandle, uint dimension, uint stringLength, uint flags,
      string varName)
    {
      CheckIfDisposed();
      return DbSrv.CreateVar(_clientHandle, _projectHandle, groupHandle, 0xFFFFFFFF, typeHandle, dimension, stringLength, flags,
        varName);
    }

    /// <summary>
    ///   Deltes the variable
    /// </summary>
    /// <param name="groupHandle">Handle of the group.</param>
    /// <param name="typeHandle">Handle of the variable.</param>
    public bool DeleteVar(uint groupHandle, uint variableHandle)
    {
      CheckIfDisposed();
      return DbSrv.DeleteVar(_clientHandle, _projectHandle, groupHandle, variableHandle) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Renames a variable
    /// </summary>
    /// <param name="groupHandle">Handle of group.</param>
    /// <param name="variableHandle">Handle of the variable.</param>
    /// <param name="newName">New name of the variable.</param>
    public bool RenameVar(uint groupHandle, uint variableHandle, string newName)
    {
      CheckIfDisposed();
      return DbSrv.RenameVar(_clientHandle, _projectHandle, groupHandle, variableHandle, newName) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Checks if variables can be renamed
    /// </summary>
    /// <param name="groupHandle">Handle of group.</param>
    /// <param name="variableHandle">Handle of the variable.</param>
    /// <param name="newName">New name of the variable.</param>
    public bool CanRenameVar(uint groupHandle, uint variableHandle, string newName)
    {
      CheckIfDisposed();
      return DbSrv.CanRenameVar(_clientHandle, _projectHandle, groupHandle, variableHandle, newName) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    public string GetVarPrevName(uint variableHandle)
    {
      CheckIfDisposed();
      return DbSrv.GetVarPrevName(_clientHandle, _projectHandle, variableHandle);
    }

    public bool SetVarDesc(uint groupHandle, uint variableHandle, uint typeHandle, uint dimension, uint stringLength,
      uint flags)
    {
      CheckIfDisposed();
      return DbSrv.SetVarDesc(_clientHandle, _projectHandle, groupHandle, variableHandle, typeHandle, dimension, stringLength, flags) ==
             (uint)K5SrvConstants.K5DbErr.Ok;
    }

    public bool CanSetVarDesc(uint groupHandle, uint variableHandle, uint typeHandle, uint dimension, uint stringLength,
      uint flags)
    {
      CheckIfDisposed();
      return DbSrv.CanSetVarDesc(_clientHandle, _projectHandle, groupHandle, variableHandle, typeHandle, dimension, stringLength, flags) ==
             (uint)K5SrvConstants.K5DbErr.Ok;
    }

    public bool SetVarInitValue(uint groupHandle, uint variableHandle, string value)
    {
      CheckIfDisposed();
      return DbSrv.SetVarInitValue(_clientHandle, _projectHandle, groupHandle, variableHandle, value) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    public bool CheckVarInitValue(uint groupHandle, uint variableHandle, string value)
    {
      CheckIfDisposed();
      return DbSrv.CheckVarInitValue(_clientHandle, _projectHandle, groupHandle, variableHandle, value) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    public string GetVarInitValue(uint groupHandle, uint variableHandle, ref uint valid)
    {
      CheckIfDisposed();
      return DbSrv.GetVarInitValue(_clientHandle, _projectHandle, groupHandle, variableHandle, ref valid);
    }

    public bool LockVar(uint groupHandle, uint variableHandle, bool lockVariable)
    {
      CheckIfDisposed();

      if (lockVariable)
      {
        return DbSrv.LockVar(_clientHandle, _projectHandle, groupHandle, variableHandle) == (uint)K5SrvConstants.K5DbErr.Ok;
      }
      
      return DbSrv.UnlockVar(_clientHandle, _projectHandle, groupHandle, variableHandle) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Returns owner of the variable
    /// </summary>
    /// <param name="variableHandle">Handle of the variable.</param>
    public string GetVarOwner(uint variableHandle)
    {
      CheckIfDisposed();
      return DbSrv.GetVarOwner(_clientHandle, _projectHandle, variableHandle);
    }

    public uint FindVar(string symbol, uint startPosHandle, uint localGroupHandle, uint flags)
    {
      CheckIfDisposed();
      return DbSrv.FindVar(_clientHandle, _projectHandle, symbol, startPosHandle, localGroupHandle, flags);
    }

    public uint FindProgramVar(uint programHandle, string varName, ref uint groupHandle, ref uint local)
    {
      CheckIfDisposed();
      return DbSrv.FindProgramVar(_clientHandle, _projectHandle, programHandle, varName, ref groupHandle, ref local);
    }

    public uint GetVarDimensions(uint groupHandle, uint variableHandle, uint bufferSize, uint[] arrFims)
    {
      CheckIfDisposed();
      return DbSrv.GetVarDimensions(_clientHandle, _projectHandle, groupHandle, variableHandle, bufferSize, arrFims);
    }

    public bool SetVarDimensions(uint groupHandle, uint variableHandle, uint bufferSize, uint[] arrFims)
    {
      CheckIfDisposed();
      return DbSrv.SetVarDimensions(_clientHandle, _projectHandle, groupHandle, variableHandle, bufferSize, arrFims) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    public string SelectVar(string text, ref SelectedVariable selectedVariable, ref uint variableHandle)
    {
      CheckIfDisposed();

      string symbolNameNew = null;

      if (DbSrv.SelectVar(_clientHandle, _projectHandle, text, ref selectedVariable, ref variableHandle))
      {
        //when user click ok, the database can return the selected variable
        IntPtr ptr = DbSrv.GetSerBuffer(_clientHandle);
        symbolNameNew = Marshal.PtrToStringAnsi(ptr);
        DbSrv.ReleaseSerBuffer(_clientHandle);
      }

      return symbolNameNew;
    }

    public string SerializeVar(uint variableHandle)
    {
      CheckIfDisposed();

      DbSrv.ReleaseSerBuffer(_clientHandle);

      DbSrv.SerializeVar(_clientHandle, _projectHandle, GetVarGroup(variableHandle), variableHandle);
      IntPtr ptrSer = DbSrv.GetSerBuffer(_clientHandle);
      string serial = Marshal.PtrToStringAnsi(ptrSer);

      DbSrv.ReleaseSerBuffer(_clientHandle);

      return serial;
    }

    public uint PasteSerializedVar(uint groupHandle, string serial)
    {
      CheckIfDisposed();

      DbSrv.SetSerBuffer(_clientHandle, serial);
      uint hVar = DbSrv.PasteSerializedVar(_clientHandle, _projectHandle, groupHandle, 0xFFFFFFFF);
      DbSrv.ReleaseSerBuffer(_clientHandle);

      return hVar;
    }

    /// <summary>
    ///   Get the properties of a database object
    /// </summary>
    /// <returns> List which contains the found properties.</returns>
    /// <param name="objectHandle">The object Id.</param>
    public List<DbProp> GetProperties(uint objectHandle)
    {
      CheckIfDisposed();

      List<DbProp> propertyList = new List<DbProp>();

      if (_projectHandle == 0)
        return propertyList;

      uint nbProp = DbSrv.GetNbProp(_clientHandle, _projectHandle, objectHandle);
      if (nbProp == 0)
        return propertyList;

      //create prop array
      uint[] arrProp = new uint[nbProp];
      DbSrv.GetProps(_clientHandle, _projectHandle, objectHandle, arrProp);

      for (uint uProp = 0; uProp < nbProp; uProp++)
      {
        DbProp prop = new DbProp();
        propertyList.Add(prop);

        prop.Id = arrProp[uProp];
        prop.Value = GetProperty(objectHandle, (K5SrvConstants.K5DbProperty)prop.Id);
      }

      return propertyList;
    }

    /// <summary>
    ///   Sets a property of an object
    /// </summary>
    /// <param name="objectHandle">Handle of the object.</param>
    /// <param name="propertyHandle">Handle of the property.</param>
    /// <param name="value">New value of the object.</param>
    public bool SetProperty(uint objectHandle, K5SrvConstants.K5DbProperty propertyHandle, string value)
    {
      CheckIfDisposed();
      return DbSrv.SetProperty(_clientHandle, _projectHandle, objectHandle, (uint)propertyHandle, value) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Returns the value of a property
    /// </summary>
    /// <param name="objectHandle">Handle of the object.</param>
    /// <param name="propertyHandle">Handle of the property.</param>
    public string GetProperty(uint objectHandle, K5SrvConstants.K5DbProperty propertyHandle)
    {
      CheckIfDisposed();
      return DbSrv.GetProperty(_clientHandle, _projectHandle, objectHandle, propertyHandle);
    }

    /// <summary>
    ///   Removes all properties
    /// </summary>
    /// <param name="objectHandle">Handle of the object.</param>
    public bool RemoveProperties(uint objectHandle)
    {
      CheckIfDisposed();
      return DbSrv.RemoveProperties(_clientHandle, _projectHandle, objectHandle) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Returns comment of an object
    /// </summary>
    /// <param name="objectHandle">Handle of the object.</param>
    /// <param name="commentType">Type of comment.</param>
    public string GetComment(uint objectHandle, uint commentType)
    {
      CheckIfDisposed();

      string comment = string.Empty;

      uint nbComm = DbSrv.GetCommentLength(_clientHandle, _projectHandle, objectHandle, commentType);
      if (nbComm > 0)
      {
        StringBuilder szBuffer = new StringBuilder((int)nbComm + 1);
        DbSrv.GetComment(_clientHandle, _projectHandle, objectHandle, commentType, szBuffer, nbComm + 1);
        comment = szBuffer.ToString();
      }

      return comment;
    }

    /// <summary>
    ///   Sets the comment of an object
    /// </summary>
    /// <param name="objectHandle">Handle of the object.</param>
    /// <param name="commentType">Type of the comment.</param>
    /// <param name="value">Comment.</param>
    public bool SetComment(uint objectHandle, uint commentType, string value)
    {
      CheckIfDisposed();
      return DbSrv.SetComment(_clientHandle, _projectHandle, objectHandle, commentType, value) == (uint)K5SrvConstants.K5DbErr.Ok;
    }


    public string SerUserGroups()
    {
      CheckIfDisposed();

      DbSrv.SerUserGroups(_clientHandle, _projectHandle);
      IntPtr ptr = DbSrv.GetSerBuffer(_clientHandle);
      string sUserGroups = Marshal.PtrToStringAnsi(ptr);
      DbSrv.ReleaseSerBuffer(_clientHandle);

      return sUserGroups;
    }

    public string GetGlobalFilePath(string suffix)
    {
      CheckIfDisposed();

      StringBuilder str = new StringBuilder(260);
      DbSrv.GetGlobalFilePath(_clientHandle, _projectHandle, suffix, str);
      string path = str.ToString();

      return path;
    }

    public string GetEqvPath(bool common, uint programHandle)
    {
      CheckIfDisposed();

      StringBuilder str = new StringBuilder(260);
      if (common)
        DbSrv.GetEqvPath(_clientHandle, _projectHandle, 1, programHandle, str);
      else
        DbSrv.GetEqvPath(_clientHandle, _projectHandle, 0, programHandle, str);
      string sPath = str.ToString();

      return sPath;
    }

    public bool PutHotSettings(bool enabled, uint dwStamp, string settings)
    {
      CheckIfDisposed();

      uint dwEnabled = 0;
      if (enabled)
        dwEnabled = 1;
      return DbSrv.PutHotSettings(_clientHandle, _projectHandle, dwEnabled, dwStamp, settings) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    public string GetHotSettings(out bool enabled, out uint stamp)
    {
      CheckIfDisposed();

      string sProp = GetProperty(_projectHandle, K5SrvConstants.K5DbProperty.EnableHot);

      enabled = (sProp != null);

      string sStamp = GetProperty(_projectHandle, K5SrvConstants.K5DbProperty.LastChange);
      if (sStamp != null)
        stamp = uint.Parse(sStamp);
      else
        stamp = 0;

      string settings = GetProperty(_projectHandle, K5SrvConstants.K5DbProperty.TargetSizing);

      return settings;
    }

    /// <summary>
    ///   Retruns if hot is enabled in the project
    /// </summary>
    public bool IsHotEnabled()
    {
      CheckIfDisposed();
      return DbSrv.IsHotEnabled(_clientHandle, _projectHandle) != 0;
    }

    /// <summary>
    ///   Sets buffer size of HOT
    /// </summary>
    /// <param name="settings"Buffer size.</param>
    public void SetHotSizing(string settings)
    {
      CheckIfDisposed();
      DbSrv.SetHotSizing(_clientHandle, _projectHandle, settings);
    }

    /// <summary>
    ///   Toggles the hot setting
    /// </summary>
    /// <param name="enable">State of the hot setting.</param>
    public void SetHot(bool enable)
    {
      CheckIfDisposed();

      if (enable)
        DbSrv.EnableHot(_clientHandle, _projectHandle);
      else
        DbSrv.DisableHot(_clientHandle, _projectHandle);
    }

    public bool CleanHotTracks()
    {
      CheckIfDisposed();
      return DbSrv.CleanHotTracks(_clientHandle, _projectHandle) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Enumerate child sub items of a complex item
    /// </summary>
    /// <returns> list of sub items, separated by \t eg for an array: tab -> tab[% u] min = 0 max=9 str -> item1\titem2</returns>
    /// <param name="parentProgramHandle">Handle of parent program or NULL.</param>
    /// <param name="text">Input text = expression.</param>
    /// <param name="minIndex">Start index for enumeration (if % in result).</param>
    /// <param name="maxIndex">Stop index for enumeration (if % in result).</param>
    /// <param name="variableHandle">Handle of leave item.</param>
    public string GetExpSubItems(uint parentProgramHandle, string text, ref uint minIndex, ref uint maxIndex,
      ref uint variableHandle)
    {
      CheckIfDisposed();
      DbSrv.GetExpSubItems(_clientHandle, _projectHandle, parentProgramHandle, text, ref minIndex, ref maxIndex, ref variableHandle);

      IntPtr ptr = DbSrv.GetSerBuffer(_clientHandle);
      string str = Marshal.PtrToStringAnsi(ptr);
      DbSrv.ReleaseSerBuffer(_clientHandle);

      return str;
    }

    /// <summary>
    ///    Get the file list in database
    /// </summary>
    /// <returns> List which contains the found files</returns>
    /// <param name="suffixes">NULL or list of suffixes separated by '|' - ex: ".rcp|.spy".</param>
    public List<DbFile> GetFiles(string suffixes)
    {
      CheckIfDisposed();

      List<DbFile> fileList = new List<DbFile>();

      if (_projectHandle == 0)
        return fileList;

      uint nbFile = DbSrv.GetNbFile(_clientHandle, _projectHandle, suffixes);

      if (nbFile == 0)
        return fileList;

      uint[] arrFile = new uint[nbFile];
      DbSrv.GetFiles(_clientHandle, _projectHandle, suffixes, arrFile);

      for (uint uFile = 0; uFile < nbFile; uFile++)
      {
        DbFile file = new DbFile();
        fileList.Add(file);
        GetFileDesc(arrFile[uFile], ref file);
      }

      return fileList;
    }

    /// <summary>
    ///    Get description of the specified file
    /// </summary>
    /// <returns> List which contains the found files</returns>
    /// <param name="fileHandle">The file handle.</param>
    /// <param name="file">The file (object DbFile).</param>
    public bool GetFileDesc(uint fileHandle, ref DbFile file)
    {
      CheckIfDisposed();

      file.Id = fileHandle;

      IntPtr ptr = DbSrv.GetFileDesc(_clientHandle, _projectHandle, fileHandle);
      file.Value = Marshal.PtrToStringAnsi(ptr);

      return file.Value != null;
    }

    /// <summary>
    ///   Searches file
    /// </summary>
    /// <param name="name">Name of the file.</param>
    public uint FindFile(string name)
    {
      CheckIfDisposed();
      return DbSrv.FindFile(_clientHandle, _projectHandle, name);
    }

    /// <summary>
    ///   Checks if file can be created
    /// </summary>
    /// <param name="name">Name of the file.</param>
    public bool CanCreateFile(string name)
    {
      CheckIfDisposed();
      return DbSrv.CanCreateFile(_clientHandle, _projectHandle, name) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Creates file
    /// </summary>
    /// <param name="name">Name of the file.</param>
    public uint CreateFile(string name)
    {
      CheckIfDisposed();
      return DbSrv.CreateFile(_clientHandle, _projectHandle, name);
    }

    /// <summary>
    ///   Renames file
    /// </summary>
    /// <param name="name">Name of the file.</param>
    /// <param name="newName">New name of the file.</param>
    public bool RenameFile(uint handleFile, string newName)
    {
      CheckIfDisposed();
      return DbSrv.RenameFile(_clientHandle, _projectHandle, handleFile, newName) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Deletes files
    /// </summary>
    /// <param name="handleFile">Handle of the file.</param>
    public bool DeleteFile(uint handleFile)
    {
      CheckIfDisposed();
      return DbSrv.DeleteFile(_clientHandle, _projectHandle, handleFile) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Locks the file
    /// </summary>
    /// <param name="handleFile">Handle of the file.</param>
    /// <param name="fileLock">Lock state of file.</param>
    public bool LockFile(uint handleFile, bool fileLock)
    {
      CheckIfDisposed();

      if (fileLock)
        return DbSrv.LockFile(_clientHandle, _projectHandle, handleFile) == (uint)K5SrvConstants.K5DbErr.Ok;
      else
        return DbSrv.UnlockFile(_clientHandle, _projectHandle, handleFile) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Saves changes of files
    /// </summary>
    /// <param name="handleFile">Handle of the file.</param>
    public bool SaveFileChanges(uint handleFile)
    {
      CheckIfDisposed();
      return DbSrv.SaveFileChanges(_clientHandle, _projectHandle, handleFile) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    public bool LookForNewFiles()
    {
      CheckIfDisposed();
      return DbSrv.LookForNewFiles(_clientHandle, _projectHandle) == (uint)K5SrvConstants.K5DbErr.Ok;
    }

    /// <summary>
    ///   Copies a file
    /// </summary>
    /// <param name="fileHandle">Handle of the file.</param>
    /// <param name="destinationName">New location.</param>
    public uint CopyFile(uint fileHandle, string destinationName)
    {
      CheckIfDisposed();
      return DbSrv.CopyFile(_clientHandle, _projectHandle, fileHandle, destinationName);
    }

    /// <summary>
    ///   Closes the database connection
    /// </summary>
    public void Dispose()
    {
      if (isDisposed)
        return;

      Close();

      isDisposed = true;
    }

    /// <summary>
    ///   Checks if conection to database was closed
    /// </summary>
    public void CheckIfDisposed()
    {
      if(isDisposed)
        throw new ObjectDisposedException("K5Srv", "The instance was already disposed");
    }
  }
}
