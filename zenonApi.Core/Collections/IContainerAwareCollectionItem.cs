using System;
using System.Collections.Generic;
using System.Text;

namespace zenonApi.Collections
{
  public interface IContainerAwareCollectionItem<TParent, TRoot>
  {
    TParent Parent { get; set; }
    TRoot Root { get; set; }
  }
}
