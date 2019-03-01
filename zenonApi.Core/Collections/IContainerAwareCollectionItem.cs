using System.Collections;

namespace zenonApi.Collections
{
  public interface IContainerAwareCollectionItem
  {
    IList ItemContainer { get; set; }
    object ContainerItemParent { get; set; }
    object ContainerItemRoot { get; set; }

    void Remove();
  }
}
