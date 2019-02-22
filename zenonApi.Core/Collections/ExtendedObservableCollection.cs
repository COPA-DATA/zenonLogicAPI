using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace zenonApi.Collections
{
  public class ExtendedObservableCollection<T> : ObservableCollection<T>
  {
    #region Ctor
    public ExtendedObservableCollection() : base() { }
    public ExtendedObservableCollection(IEnumerable<T> collection) : base(collection) { }
    public ExtendedObservableCollection(IList<T> collection) : base(collection) { }
    #endregion

    #region Extended public functionality
    /// <summary>
    /// Adds the elements of the specified collection to the end of the current list.
    /// </summary>
    /// <param name="collection">
    /// The collection whose elements should be added to the end of the current collection. The collection itself
    /// cannot be null, but it can contain elements that are null, if type <typeparamref name="T"/> is a reference
    /// type.
    /// </param>
    /// <exception cref="ArgumentNullException"/>
    public void AddRange(IEnumerable<T> collection) => this.InsertItemRange(Count, collection);


    /// <summary>
    /// Inserts the elements of a collection into the current list at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
    /// <param name="collection">
    /// The collection whose elements should be inserted into the current collection.
    /// The collection itself cannot be null, but it can contain elements that are null, if type
    /// <typeparamref name="T"/> is a reference type.
    /// </param>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public void InsertRange(int index, IEnumerable<T> collection) => this.InsertItemRange(index, collection);


    /// <summary>
    /// Removes a range of elements from the current collection.
    /// </summary>
    /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
    /// <param name="count">The number of elements to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"/>
    /// <exception cref="ArgumentException"/>
    public void RemoveRange(int index, int count) => this.RemoveItemRange(index, count);


    /// <summary>
    /// Removes the first occurrence of each entry of the given collection. The collection itself must not be null.
    /// If an entry of the given collection is not found for removal, no exception is thrown.
    /// </summary>
    /// <param name="collection">The collection containing all items to remove.</param>
    /// <exception cref="ArgumentNullException"/>
    public void RemoveRange(IEnumerable<T> collection) => this.RemoveItemRange(collection);
    #endregion


    #region Extended protected functionality, for deriving classes
    /// <summary>
    /// Inserts the elements of a collection into the current list at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
    /// <param name="collection">
    /// The collection whose elements should be inserted into the current collection.
    /// The collection itself cannot be null, but it can contain elements that are null, if type
    /// <typeparamref name="T"/> is a reference type.
    /// </param>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
    protected virtual void InsertItemRange(int index, IEnumerable<T> collection)
    {
      if (collection == null)
      {
        throw new ArgumentNullException(nameof(collection));
      }
      if (index < 0)
      {
        throw new ArgumentOutOfRangeException(nameof(index));
      }
      if (index > Count)
      {
        throw new ArgumentOutOfRangeException(nameof(index));
      }

      CheckReentrancy();

      IList list = collection.ToList();

      int curIndex = index;
      foreach (var item in collection)
      {
        Items.Insert(curIndex++, item);
      }

      OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
      OnPropertyChanged(new PropertyChangedEventArgs("Index[]"));
    }


    /// <summary>
    /// Removes the first occurrence of each entry of the given collection. The collection itself must not be null.
    /// If an entry of the given collection is not found for removal, no exception is thrown.
    /// </summary>
    /// <param name="collection">The collection containing all items to remove.</param>
    /// <exception cref="ArgumentNullException"/>
    protected virtual void RemoveItemRange(IEnumerable<T> collection)
    {
      if (collection == null)
      {
        throw new ArgumentNullException(nameof(collection));
      }

      CheckReentrancy();

      foreach (T item in collection)
      {
        var index = IndexOf(item);
        if (index < 0)
        {
          continue;
        }

        // The following will raise CollectionChangedEvents
        this.RemoveAt(index);
      }

      OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
      OnPropertyChanged(new PropertyChangedEventArgs("Index[]"));
    }


    /// <summary>
    /// Removes a range of elements from the current collection.
    /// </summary>
    /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
    /// <param name="count">The number of elements to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"/>
    /// <exception cref="ArgumentException"/>
    protected virtual void RemoveItemRange(int index, int count)
    {
      if (count < 0)
      {
        throw new ArgumentOutOfRangeException(nameof(count));
      }
      if (index < 0 || index + count > Count)
      {
        throw new ArgumentOutOfRangeException(nameof(index));
      }
      if (count == 0)
      {
        return;
      }

      CheckReentrancy();

      IList list = new List<T>();

      for (int i = index + count - 1; i >= index; i--)
      {
        var item = Items[i];
        list.Add(item);

        Items.RemoveAt(i);
      }

      OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
      OnPropertyChanged(new PropertyChangedEventArgs("Index[]"));
      OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, list, index));
    }
    #endregion
  }
}
