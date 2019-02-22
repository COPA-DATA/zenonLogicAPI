namespace zenonApi.Collections
{
  public abstract class ContainerAwareCollectionItem<TSelf>
    where TSelf : ContainerAwareCollectionItem<TSelf>
  {
    internal object ContainerItemParent;
    internal object ContainerItemRoot;

    internal ContainerAwareObservableCollection<TSelf> ItemContainer { get; set; } = null;

    /// <summary>
    /// Removes this child from its parent and root. No exception is thrown if no
    /// containing collection knows about this object.
    /// </summary>
    public virtual void Remove()
    {
      if (this.ItemContainer == null)
      {
        return;
      }

      ItemContainer.Remove((TSelf)this);
    }
  }
}
