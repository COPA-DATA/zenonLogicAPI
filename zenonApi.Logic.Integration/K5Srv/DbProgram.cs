namespace zenonApi.Zenon.K5Srv
{
  public partial class K5SrvWrapper
  {
    public class DbProgram
    {
      public uint ProgramId;
      public string Name = string.Empty;

      public uint Language = (uint)K5SrvConstants.K5DbLanguage.Any;
      public uint Section = (uint)K5SrvConstants.K5DbSection.Any;
      public uint ParentHandle;

      public string Path = string.Empty;
    }
  }
}