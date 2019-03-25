using System.Collections;

namespace zenonApi.Collections
{
  public interface IContainerAwareCollectionItem
  {
    IList ItemContainer { get; set; }
    object ItemContainerParent { get; set; }
    object ItemContainerRoot { get; set; }

    void Remove();
  }
}
