namespace zenonApi.Zenon.K5Srv
{
  public class DbVar
    {
      public uint Id;
      public string Name = string.Empty;

      public uint Type;
      public uint Dim;
      public uint StringLength;
      public uint Flags;

      public string InitialValue = string.Empty;
    }
}