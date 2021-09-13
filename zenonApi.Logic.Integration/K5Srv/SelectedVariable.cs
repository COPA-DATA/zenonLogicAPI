using System;
using System.Runtime.InteropServices;

namespace zenonApi.Zenon.K5Srv
{
  [StructLayout(LayoutKind.Sequential)]
  public struct SelectedVariable
  {
    public uint parentGroup;
    public uint prefType;
    public IntPtr handleWindowParent;
    public K5Point pointerPos;
    public uint dwOptions;
  }

  //structures for K5DB_SelectVar
  [StructLayout(LayoutKind.Sequential)]
  public struct K5Point
  {
    public int x;
    public int y;
  }
}
