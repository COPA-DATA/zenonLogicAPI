using System;
using System.Runtime.InteropServices;
using System.Text;

namespace zenonApi.Zenon.K5Srv
{
  internal static class DbSrv
  {
    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetVersion")]
    public static extern int GetVersion(); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetKindOfObject")]
    public static extern uint GetKindOfObject(uint dwId);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_IsObjectLocked")]
    public static extern uint IsObjectLocked(uint dwId); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CheckSymbol")]
    public static extern uint CheckSymbol(string szName); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetGhostName")]
    private static extern IntPtr _GetGhostName(uint dwId); 

    public static string GetGhostName(uint dwId)
    {
      IntPtr propertyPointer = _GetGhostName(dwId);
      return Marshal.PtrToStringAnsi(propertyPointer);
    }

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_Connect")]
    public static extern uint Connect(IntPtr handleWindowCallback, uint msgCallback,
      uint dwFlags, uint dwData,
      string szClientName); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SetCallback")]
    public static extern uint
      SetCallback(uint hClient, IntPtr handleWindowCallback, uint msgCallback); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_Disconnect")]
    public static extern void Disconnect(uint client);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetClientName")]
    private static extern IntPtr _GetClientName(uint client); 

    public static string GetClientName(uint client)
    {
      IntPtr ptrProp = _GetClientName(client);
      return Marshal.PtrToStringAnsi(ptrProp);
    }

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetMessage")]
    private static extern IntPtr _GetMessage(uint dwRc); 

    public static string GetMessage(uint dwRc)
    {
      IntPtr ptrProp = _GetMessage(dwRc);
      return Marshal.PtrToStringAnsi(ptrProp);
    }

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetEventDesc")]
    private static extern IntPtr _GetEventDesc(uint dwEvent, ref uint pdwFlags); 

    public static string GetEventDesc(uint dwEvent, ref uint pdwFlags)
    {
      IntPtr ptrProp = _GetEventDesc(dwEvent, ref pdwFlags);
      return Marshal.PtrToStringAnsi(ptrProp);
    }

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_DispatchEvent")]
    public static extern uint
      DispatchEvent(uint hClient, uint hProject, uint dwEvent, uint dwArg); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_EnableEvents")]
    public static extern uint
      EnableEvents(uint hClient, uint hProject, uint dwEnable); 


    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_OpenProject")]
    public static extern uint OpenProject(uint hClient, string szProjectPath); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CloseProject")]
    public static extern void CloseProject(uint hClient, uint hProject); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetProjectPath")]
    public static extern uint GetProjectPath(uint hClient, uint hProject, StringBuilder sPath);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SaveProject")]
    public static extern uint SaveProject(uint hClient, uint hProject); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetProjectDateStamp")]
    public static extern uint GetProjectDateStamp(uint hClient, uint hProject); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SetProjectModified")]
    public static extern uint SetProjectModified(uint hClient, uint hProject);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetNbProgram")]
    public static extern uint
      GetNbProgram(uint hClient, uint hProject, uint dwSection);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetPrograms")]
    public static extern uint
      GetPrograms(uint hClient, uint hProject, uint dwSection,
        uint[] arrPrg); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetNbChildren")]
    public static extern uint
      GetNbChildren(uint hClient, uint hProject, uint hParent); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetChildren")]
    public static extern uint
      GetChildren(uint hClient, uint hProject, uint hParent,
        uint[] arrChild);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_FindProgram")]
    public static extern uint FindProgram(uint hClient, uint hProject, string szProgName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetProgramDesc")]
    public static extern IntPtr GetProgramDesc(uint hClient, uint hProject, uint hProgram,
      ref uint dwLanguage, ref uint dwSection, ref uint hParent);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CanCreateProgram")]
    public static extern uint CanCreateProgram(uint hClient, uint hProject,
      uint dwLanguage, uint dwSection, uint hParent,
      string szProgName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CreateProgram")]
    public static extern uint CreateProgram(uint hClient, uint hProject,
      uint dwLanguage, uint dwSection, uint hParent,
      string szProgName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CopyProgram")]
    public static extern uint CopyProgram(uint hClient, uint hProject,
      uint hProgram, uint dwSection, uint hParent,
      string szProgName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_RenameProgram")]
    public static extern uint RenameProgram(uint hClient, uint hProject,
      uint hProgram, string szNewName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_MoveProgram")]
    public static extern uint MoveProgram(uint hClient, uint hProject,
      uint hProgram, uint dwMove, uint hParent);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_DeleteProgram")]
    public static extern uint DeleteProgram(uint hClient, uint hProject, uint hProgram);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_LockProgram")]
    public static extern uint
      LockProgram(uint hClient, uint hProject, uint hProgram);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SaveProgramChanges")]
    public static extern uint
      SaveProgramChanges(uint hClient, uint hProject, uint hProgram);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_UnlockProgram")]
    public static extern uint
      UnlockProgram(uint hClient, uint hProject, uint hProgram);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetProgramOwner")]
    private static extern IntPtr
      _GetProgramOwner(uint hClient, uint hProject, uint hProgram);

    public static string GetProgramOwner(uint hClient, uint hProject, uint hProgram)
    {
      IntPtr ptrProp = _GetProgramOwner(hClient, hProject, hProgram);
      return Marshal.PtrToStringAnsi(ptrProp);
    }

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetProgramPath")]
    public static extern uint GetProgramPath(uint hClient, uint hProject, uint hProgram,
      StringBuilder szPath);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetProgramSchedule")]
    public static extern uint GetProgramSchedule(uint hClient, uint hProject, uint hProgram,
      ref uint pdwPeriod, ref uint pdwOffset);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SetProgramSchedule")]
    public static extern uint SetProgramSchedule(uint hClient, uint hProject, uint hProgram,
      uint dwPeriod, uint dwOffset);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetProgramOnCallFlag")]
    public static extern uint
      GetProgramOnCallFlag(uint hClient, uint hProject, uint hProgram);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SetProgramOnCallFlag")]
    public static extern uint
      SetProgramOnCallFlag(uint hClient, uint hProject, uint hProgram, uint dwOnCall);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_ExportProgram")]
    public static extern uint
      ExportProgram(uint hClient, uint hProject, uint hProgram, string szExportFileName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_ImportProgram")]
    public static extern uint ImportProgram(uint hClient, uint hProject, string szImportFileName,
      string szWishedName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetImportProgramName")]
    public static extern uint GetImportProgramName(uint hClient, string szImportFileName,
      StringBuilder szNameBuffer, uint dwBufSize);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetNbType")]
    public static extern uint GetNbType(uint hClient, uint hProject);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetTypes")]
    public static extern uint
      GetTypes(uint hClient, uint hProject, uint[] arrTypes);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_FindType")]
    public static extern uint FindType(uint hClient, uint hProject, string szType);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetTypeDesc")]
    public static extern IntPtr
      GetTypeDesc(uint hClient, uint hProject, uint hType, ref uint dwFlags);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetTypeUDFB")]
    public static extern uint
      GetTypeUDFB(uint hClient, uint hProject, uint hType);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetNbTypeParam")]
    public static extern uint
      GetNbTypeParam(uint hClient, uint hProject, uint hType);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetTypeParams")]
    public static extern uint
      GetTypeParams(uint hClient, uint hProject, uint hType,
        uint[] arrParams);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetUDFBNbIO")]
    public static extern uint GetUDFBNbIO(uint hClient, uint hProject, uint hType, ref uint pdwNbIn,
      ref uint pdwNbOut); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_RenameType")]
    public static extern uint
      RenameType(uint hClient, uint hProject, uint hType, string szNewName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_DeleteType")]
    public static extern uint
      DeleteType(uint hClient, uint hProject, uint hType); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CreateEnumType")]
    public static extern uint
      CreateEnumType(uint hClient, uint hProject, string szName,
        string szValues); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetEnumTypeValues")]
    public static extern IntPtr
      GetEnumTypeValues(uint hClient, uint hProject, uint hType); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SetEnumTypeValues")]
    public static extern uint
      SetEnumTypeValues(uint hClient, uint hProject, uint hType,
        string szValues); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CreateBitFieldType")]
    public static extern uint CreateBitFieldType(uint hClient, uint hProject, uint hBaseType, string szName,
      string szBits); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetBitFieldType")]
    public static extern IntPtr
      GetBitFieldType(uint hClient, uint hProject, uint hType,
        ref uint phBaseType); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SetBitFieldType")]
    public static extern uint SetBitFieldType(uint hClient, uint hProject, uint hType, uint hBaseType,
      string szBits); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetNbGroup")]
    public static extern uint GetNbGroup(uint hClient, uint hProject); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetGroups")]
    public static extern uint
      GetGroups(uint hClient, uint hProject, uint[] arrGroups); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetSortedGroups")]
    public static extern uint
      GetSortedGroups(uint hClient, uint hProject, uint[] arrGroups,
        uint dwSort); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_FindGroup")]
    public static extern uint FindGroup(uint hClient, uint hProject, string szGroupName); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetGroupDesc")]
    public static extern IntPtr GetGroupDesc(uint hClient, uint hProject, uint hGroup,
      ref uint dwGroupStyle, ref uint hIoType,
      ref uint dwSlot, ref uint dwSubSlot,
      ref uint nbVar); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_LockGroup")]
    public static extern uint LockGroup(uint hClient, uint hProject, uint hGroup);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_UnlockGroup")]
    public static extern uint
      UnlockGroup(uint hClient, uint hProject, uint hGroup);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetGroupOwner")]
    private static extern IntPtr
      _GetGroupOwner(uint hClient, uint hProject, uint hGroup);

    public static string GetGroupOwner(uint hClient, uint hProject, uint hGroup)
    {
      IntPtr ptrProp = _GetGroupOwner(hClient, hProject, hGroup);
      return Marshal.PtrToStringAnsi(ptrProp);
    }

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_LockGroupEx")]
    public static extern uint
      LockGroupEx(uint hClient, uint hProject, uint hGroup);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_UnlockGroupEx")]
    public static extern uint
      UnlockGroupEx(uint hClient, uint hProject, uint hGroup);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetNbVar")]
    public static extern uint
      GetNbVar(uint hClient, uint hProject, uint hGroup); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetVars")]
    public static extern uint GetVars(uint hClient, uint hProject, uint hGroup,
      uint[] arrVars);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetSortedVars")]
    public static extern uint GetSortedVars(uint hClient, uint hProject, uint hGroup,
      uint dwMethod, uint[] arrVars);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_FindVarInGroup")]
    public static extern uint FindVarInGroup(uint hClient, uint hProject, uint hGroup,
      string szVarName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CreateVar")]
    public static extern uint CreateVar(uint hClient, uint hProject, uint hGroup,
      uint dwPosId, uint hType, uint dwDim, uint dwStringLength,
      uint dwFlags, string szVarName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_DeleteVar")]
    public static extern uint
      DeleteVar(uint hClient, uint hProject, uint hGroup, uint hVar);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_RenameVar")]
    public static extern uint RenameVar(uint hClient, uint hProject, uint hGroup, uint hVar,
      string szNewName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CanRenameVar")]
    public static extern uint CanRenameVar(uint hClient, uint hProject, uint hGroup, uint hVar,
      string szNewName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetVarPrevName")]
    private static extern IntPtr
      _GetVarPrevName(uint hClient, uint hProject, uint hVar);

    public static string GetVarPrevName(uint hClient, uint hProject, uint hVar)
    {
      IntPtr ptrProp = _GetVarPrevName(hClient, hProject, hVar);
      return Marshal.PtrToStringAnsi(ptrProp);
    }

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_MoveVar")]
    public static extern uint MoveVar(uint hClient, uint hProject, uint hGroup, uint hVar, uint dwPosId);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetVarGroup")]
    public static extern uint
      GetVarGroup(uint hClient, uint hProject, uint hVar);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetVarDesc")]
    public static extern IntPtr GetVarDesc(uint hClient, uint hProject, uint hGroup, uint hVar,
      ref uint hType, ref uint dwDim, ref uint dwStringLength,
      ref uint dwFlags);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SetVarDesc")]
    public static extern uint SetVarDesc(uint hClient, uint hProject, uint hGroup, uint hVar,
      uint hType, uint dwDim, uint dwStringLength, uint dwFlags);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CanSetVarDesc")]
    public static extern uint CanSetVarDesc(uint hClient, uint hProject, uint hGroup, uint hVar,
      uint hType, uint dwDim, uint dwStringLength, uint dwFlags);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SetVarInitValue")]
    public static extern uint SetVarInitValue(uint hClient, uint hProject, uint hGroup, uint hVar,
      string szValue);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CheckVarInitValue")]
    public static extern uint CheckVarInitValue(uint hClient, uint hProject, uint hGroup, uint hVar,
      string szValue);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetVarInitValue")]
    private static extern IntPtr _GetVarInitValue(uint hClient, uint hProject, uint hGroup, uint hVar,
      ref uint pdwValid);

    public static string GetVarInitValue(uint hClient, uint hProject, uint hGroup, uint hVar,
      ref uint pdwValid)
    {
      IntPtr ptrProp = _GetVarInitValue(hClient, hProject, hGroup, hVar, ref pdwValid);
      return Marshal.PtrToStringAnsi(ptrProp);
    }

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_LockVar")]
    public static extern uint
      LockVar(uint hClient, uint hProject, uint hGroup, uint hVar);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_UnlockVar")]
    public static extern uint
      UnlockVar(uint hClient, uint hProject, uint hGroup, uint hVar);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetVarOwner")]
    private static extern IntPtr
      _GetVarOwner(uint hClient, uint hProject, uint hVar);

    public static string GetVarOwner(uint hClient, uint hProject, uint hVar)
    {
      IntPtr ptrProp = _GetVarOwner(hClient, hProject, hVar);
      return Marshal.PtrToStringAnsi(ptrProp);
    }

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_FindVar")]
    public static extern uint FindVar(uint hClient, uint hProject, string szSymbol, uint hStartPos,
      uint hLocalGroup, uint dwFlags);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_FindProgramVar")]
    public static extern uint FindProgramVar(uint hClient, uint hProject, uint hProgram, string szVarName,
      ref uint phGroup, ref uint pbLocal);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetVarDimensions")]
    public static extern uint GetVarDimensions(uint hClient, uint hProject, uint hGroup, uint hVar,
      uint dwBufSize, uint[] arrFims);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SetVarDimensions")]
    public static extern uint SetVarDimensions(uint hClient, uint hProject, uint hGroup, uint hVar,
      uint dwBufSize, uint[] arrFims);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SerUserGroups")]
    public static extern uint SerUserGroups(uint hClient, uint hProject);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetNbProp")]
    public static extern uint
      GetNbProp(uint hClient, uint hProject, uint hObject);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetProps")]
    public static extern uint
      GetProps(uint hClient, uint hProject, uint hObject, uint[] arrProps);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SetProperty")]
    public static extern uint SetProperty(uint hClient, uint hProject, uint hObject, uint hProp,
      string szValue); // assign a value to a property

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetProperty")]
    private static extern IntPtr
      _GetProperty(uint hClient, uint hProject, uint hObject, uint hProp);

    public static string GetProperty(uint hClient, uint hProject, uint hObject, K5SrvConstants.K5DbProperty hProp)
    {
      IntPtr ptrProp = _GetProperty(hClient, hProject, hObject, (uint)hProp);
      return Marshal.PtrToStringAnsi(ptrProp);
    }

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_RemoveProperties")]
    public static extern uint
      RemoveProperties(uint hClient, uint hProject, uint hObject); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SetComment")]
    public static extern uint SetComment(uint hClient, uint hProject, uint hObject,
      uint dwCommType, string szValue);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetComment")]
    public static extern uint GetComment(uint hClient, uint hProject, uint hObject,
      uint dwCommType,
      StringBuilder szBuffer, uint dwBufSize);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetCommentLength")]
    public static extern uint GetCommentLength(uint hClient, uint hProject, uint hObject,
      uint dwCommType);
    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SelectVar")]
    public static extern bool SelectVar(uint hClient, uint hProject, string szText, ref SelectedVariable selectedVariable,
      ref uint phVar);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetSerBuffer")]
    public static extern IntPtr GetSerBuffer(uint hClient);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_RealeaseSerBuffer")]
    public static extern void ReleaseSerBuffer(uint hClient);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SetSerBuffer")]
    public static extern void SetSerBuffer(uint hClient, string szText);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SerializeVar")]
    public static extern uint
      SerializeVar(uint hClient, uint hProject, uint hGroup, uint hVar);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_PasteSerializedVar")]
    public static extern uint
      PasteSerializedVar(uint hClient, uint hProject, uint hGroup, uint dwPosId);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetGlobalFilePath")]
    public static extern uint GetGlobalFilePath(uint hClient, uint hProject, string szSuffix,
      StringBuilder szPath); 

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetEqvPath")]
    public static extern uint GetEqvPath(uint hClient, uint hProject, uint bCommon, uint hProgram,
      StringBuilder szPath);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_PutHotSettings")]
    public static extern uint PutHotSettings(uint hClient, uint hProject, uint dwEnabled, uint dwStamp,
      string szSettings);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_IsHotEnabled")]
    public static extern uint IsHotEnabled(uint hClient, uint hProject);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SetHotSizing")]
    public static extern void SetHotSizing(uint hClient, uint hProject, string szSettings);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_EnableHot")]
    public static extern void EnableHot(uint hClient, uint hProject);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_DisableHot")]
    public static extern void DisableHot(uint hClient, uint hProject);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CleanHotTracks")]
    public static extern uint CleanHotTracks(uint hClient, uint hProject);

    // enumerate child sub items of a complex item ////////////////////////
    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetExpSubItems")]
    public static extern uint GetExpSubItems(uint hClient, uint hProject, uint hParentProgram, string szText,
      ref uint dwMinIndex, ref uint dwMaxIndex, ref uint hVar);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetNbFile")]
    public static extern uint
      GetNbFile(uint hClient, uint hProject, string szExt);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetFiles")]
    public static extern uint
      GetFiles(uint hClient, uint hProject, string szExt, uint[] arrFile);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_FindFile")]
    public static extern uint FindFile(uint hClient, uint hProject, string szName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_GetFileDesc")]
    public static extern IntPtr GetFileDesc(uint hClient, uint hProject, uint hFile);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CanCreateFile")]
    public static extern uint CanCreateFile(uint hClient, uint hProject, string szName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CreateFile")]
    public static extern uint CreateFile(uint hClient, uint hProject, string szName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_RenameFile")]
    public static extern uint
      RenameFile(uint hClient, uint hProject, uint hFile, string szNewName);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_DeleteFile")]
    public static extern uint DeleteFile(uint hClient, uint hProject, uint hFile);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_LockFile")]
    public static extern uint LockFile(uint hClient, uint hProject, uint hFile);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_UnlockFile")]
    public static extern uint UnlockFile(uint hClient, uint hProject, uint hFile);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_SaveFileChanges")]
    public static extern uint SaveFileChanges(uint hClient, uint hProject, uint hFile);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_LookForNewFiles")]
    public static extern uint LookForNewFiles(uint hClient, uint hProject);

    [DllImport("K5DBSrv.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "K5DB_CopyFile")]
    public static extern uint
      CopyFile(uint hClient, uint hProject, uint hFile, string szDstName);
  }
}
