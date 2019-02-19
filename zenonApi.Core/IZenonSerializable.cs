using System;
using System.Collections.Generic;
using System.Text;

namespace zenonApi.Core
{
  public interface IZenonSerializable<TParent> : IZenonSerializable
  {
    TParent Parent { get; }
  }

  public interface IZenonSerializable
  {
  }
}
