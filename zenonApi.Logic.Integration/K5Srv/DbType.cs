using System.Collections.Generic;

namespace zenonApi.Zenon.K5Srv
{
  public class DbType
  {
    public uint TypeId;
    public string Name = string.Empty;

    public uint Flags;

    public uint UdfbHandle;
    public uint In;
    public uint Out;

    public List<DbVar> ParameterList = new List<DbVar>();
  }
}