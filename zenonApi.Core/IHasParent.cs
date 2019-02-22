using System;
using System.Collections.Generic;
using System.Text;

namespace zenonApi
{
  public interface IHasParent
  {
    object Parent { get; set; }
  }
}
