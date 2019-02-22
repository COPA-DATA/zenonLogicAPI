using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using zenonApi.Serialization;

namespace zenonApi.Collections
{
  public class ContainerAwareObservableCollection<TChildren> : ExtendedObservableCollection<TChildren>
    where TChildren : ContainerAwareCollectionItem<TChildren>
  {
    #region Ctor
    public ContainerAwareObservableCollection() : base() { }

    public ContainerAwareObservableCollection(object parent = null, object root = null) : base()
    {
      this.parent = parent;
      this.root = root;
    }

    public ContainerAwareObservableCollection(IEnumerable<TChildren> collection, object parent = null, object root = null) : base(collection)
    {
      this.parent = parent;
      this.root = root;

      foreach (var item in collection)
      {
        HandleChildAdd(item);
      }
    }

    public ContainerAwareObservableCollection(IList<TChildren> collection, object parent = null, object root = null) : base(collection)
    {
      this.parent = parent;
      this.root = root;

      foreach (var item in collection)
      {
        HandleChildAdd(item);
      }
    }
    #endregion


    #region Parent / Root
    private object parent = null;
    private object root = null;
    private bool parentAndRootDerivedFromFirstEverAddedChild = false;

    /// <summary>
    /// Gets or sets the parent of all children.
    /// </summary>
    public object Parent
    {
      get => parent;
      set
      {
        parent = value;
        foreach (var item in this.Items)
        {
          if (item != null)
          {
            item.ContainerItemParent = value;
          }
        }
      }
    }

    /// <summary>
    /// Gets or sets the root of all children.
    /// </summary>
    public object Root
    {
      get => root;
      set
      {
        root = value;
        foreach (var item in this.Items)
        {
          if (item != null)
          {

          }
          item.ContainerItemRoot = value;
        }
      }
    }
    #endregion


    #region Set, Add, Remove, etc.
    /// <summary>
    /// Protected method to handle removal of children, therefore removal of their parent and root if appropriate.
    /// </summary>
    /// <param name="item">The item for which the parent and root shall be removed.</param>
    protected void HandleChildRemoval(TChildren item)
    {
      if (item == null)
      {
        return;
      }

      // Find all occurrences of the child
      var found = Items.Where(x => x != null && x.Equals(item)).ToArray();
      if (found.Length == 0 || found.Length > 1)
      {
        // No or more than just one entry is contained, the parent must stay the same
        return;
      }

      // Exactly one item is contained, remove the parent
      item.ContainerItemParent = null;
      item.ContainerItemRoot = null;
      item.ItemContainer = null;
    }

    /// <summary>
    /// Protected method to handle adding children, therefore setting their parent and root.
    /// </summary>
    /// <param name="item">The item to assign the parent and root.</param>
    protected void HandleChildAdd(TChildren item)
    {
      if (item == null)
      {
        return;
      }

      if (this.parent == null && this.root == null && !this.parentAndRootDerivedFromFirstEverAddedChild)
      {
        this.parent = item.ContainerItemParent;
        this.root = item.ContainerItemRoot;
        this.parentAndRootDerivedFromFirstEverAddedChild = true;
      }

      // Check if the item has already a parent, if so, adding it is invalid
      if ((item.ContainerItemParent != parent || item.ContainerItemRoot != root) && (item.ContainerItemParent != null || item.ContainerItemRoot != null))
      {
        throw new InvalidOperationException("Cannot add a child if it is possessed by another parent or root.");
      }

      item.ItemContainer = this;
      item.ContainerItemParent = parent;
      item.ContainerItemRoot = root;
    }
    
    /// <summary>
    /// Replaces the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to replace.</param>
    /// <param name="item">The new value for the element at the specified index.</param>
    protected override void SetItem(int index, TChildren item)
    {
      HandleChildRemoval(item);
      HandleChildAdd(item);

      base.SetItem(index, item);
    }

    /// <summary>
    /// Removes all items from the collection.
    /// </summary>
    protected override void ClearItems()
    {
      foreach (var item in this.Items)
      {
        HandleChildRemoval(item);
      }

      base.ClearItems();
    }

    /// <summary>
    /// Removes the item at the specified index of the collection.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    protected override void RemoveItem(int index)
    {
      if (index >= 0 && index < this.Items.Count)
      {
        // if this code is not reached, then the index is out of bounds.
        // However, let the base implementation throw the exception in base.RemoveItem(index) below.
        TChildren item = this.Items[index];
        HandleChildRemoval(item);
      }

      base.RemoveItem(index);
    }

    /// <summary>
    /// Inserts an item into the collection at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which the item should be inserted.</param>
    /// <param name="item">The object to insert.</param>
    protected override void InsertItem(int index, TChildren item)
    {
      HandleChildAdd(item);
      base.InsertItem(index, item);
    }
    #endregion
  }
}
