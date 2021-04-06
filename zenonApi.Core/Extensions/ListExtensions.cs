using System.Collections.Generic;
using zenonApi.Collections;

namespace zenonApi.Extensions
{
  public static class ListExtensions
  {
    public static void Remove(this IEnumerable<IContainerAwareCollectionItem> self)
    {
      // I guess this is the most simple extension method ever, however it is extremely useful.
      // What can be done thanks to it: myFullCollection.Where(x => x.Name.StartsWith("Something")).Remove();
      foreach (var item in self)
      {
        item.Remove();
      }
    }
  }
}
