using System.Collections;
using zenonApi.Serialization;

namespace zenonApi.Collections
{
  public abstract class ContainerAwareCollectionItem<TSelf> : zenonSerializable<TSelf>, IContainerAwareCollectionItem
    where TSelf : class, IZenonSerializable<TSelf>
  {
    // The following interface properties are hidden by intent
    IList IContainerAwareCollectionItem.ItemContainer { get; set; }
    object IContainerAwareCollectionItem.ItemContainerParent { get; set; }
    object IContainerAwareCollectionItem.ItemContainerRoot { get; set; }

    /// <summary>
    /// Removes this child from its parent and root. No exception is thrown if no
    /// containing collection knows about this object.
    /// </summary>
    public virtual void Remove()
    {
      IContainerAwareCollectionItem self = this;
      if (self.ItemContainer == null)
      {
        return;
      }

      self.ItemContainer.Remove(this);
    }
  }
}
