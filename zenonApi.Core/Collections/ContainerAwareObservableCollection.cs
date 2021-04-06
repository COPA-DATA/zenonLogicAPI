﻿// ReSharper disable UnusedMember.Global : This is an API, unused members and methods can occur.
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

namespace zenonApi.Collections
{
  public class ContainerAwareObservableCollection<TChildren>
    : ExtendedObservableCollection<TChildren> where TChildren : class, IContainerAwareCollectionItem
  {
    #region Ctor
    public ContainerAwareObservableCollection() { }

    public ContainerAwareObservableCollection(object parent = null, object root = null)
    {
      this._parent = parent;
      this._root = root;
    }

    public ContainerAwareObservableCollection(IEnumerable<TChildren> collection, object parent = null, object root = null) : base(collection)
    {
      this._parent = parent;
      this._root = root;

      foreach (var item in collection)
      {
        HandleChildAdd(item);
      }
    }

    public ContainerAwareObservableCollection(IList<TChildren> collection, object parent = null, object root = null) : base(collection)
    {
      this._parent = parent;
      this._root = root;

      foreach (var item in collection)
      {
        HandleChildAdd(item);
      }
    }
    #endregion


    #region Parent / Root
    private object _parent;
    private object _root;
    private bool _parentAndRootDerivedFromFirstEverAddedChild;

    /// <summary>
    /// Gets or sets the parent of all children.
    /// </summary>
    [DoNotNotify]
    public object Parent
    {
      get => _parent;
      set
      {
        _parent = value;
        foreach (var item in this.Items)
        {
          if (item != null)
          {
            item.ItemContainerParent = value;
          }
        }
      }
    }

    /// <summary>
    /// Gets or sets the root of all children.
    /// </summary>
    [DoNotNotify]
    public object Root
    {
      get => _root;
      set
      {
        _root = value;
        foreach (var item in this.Items)
        {
          if (item != null)
          {
            item.ItemContainerRoot = value;
          }
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
      item.ItemContainerParent = null;
      item.ItemContainerRoot = null;
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

      if (this._parent == null && this._root == null && !this._parentAndRootDerivedFromFirstEverAddedChild)
      {
        this._parent = item.ItemContainerParent;
        this._root = item.ItemContainerRoot;
        this._parentAndRootDerivedFromFirstEverAddedChild = true;
      }

      // Check if the item has already a parent, if so, adding it is invalid
      if ((item.ItemContainerParent != _parent || item.ItemContainerRoot != _root) && (item.ItemContainerParent != null || item.ItemContainerRoot != null))
      {
        throw new InvalidOperationException("Cannot add a child if it is possessed by another parent or root.");
      }

      item.ItemContainer = this;
      item.ItemContainerParent = _parent;
      item.ItemContainerRoot = _root;
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

    protected override void InsertItemRange(int index, IEnumerable<TChildren> collection)
    {
      foreach (var item in collection)
      {
        HandleChildAdd(item);
      }
      base.InsertItemRange(index, collection);
    }

    protected override void RemoveItemRange(IEnumerable<TChildren> collection)
    {
      foreach (var item in collection)
      {
        HandleChildRemoval(item);
      }
      base.RemoveItemRange(collection);
    }

    protected override void RemoveItemRange(int index, int count)
    {
      // Here we do not use the base class method, since we want to call HandleChildRemoval for all of them
      // (so we have to iterate anyways, we do not need to do this twice)
      for (int i = index + count - 1; i >= index; i--)
      {
        var item = this.Items[i];
        HandleChildRemoval(item);
        RemoveAt(i);
      }
    }
    #endregion
  }
}
