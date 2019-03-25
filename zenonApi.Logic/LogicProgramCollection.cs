using System;
using System.Collections.Generic;
using System.Linq;
using zenonApi.Collections;
using zenonApi.Logic.Internal;

namespace zenonApi.Logic
{
  public class LogicProgramCollection : ContainerAwareObservableCollection<LogicProgram>
  {
    /// <summary>
    /// Internal default constructor to prevent users to use it directly.
    /// This way we can ensure, that the contained programs do always contain a root, which we require for several
    /// methods to find the connected <see cref="_Pou"/>.
    /// </summary>
    internal LogicProgramCollection() : base() { }

    protected override void ClearItems()
    {
      if (this.Items.Count == 0)
      {
        return;
      }

      // Besides deleting all items in the user-visible list, we have to also delete them from
      // LogicProject > _Programs.
      this.Items.FirstOrDefault()?.Root.Programs.ProgramOrganizationUnits.Clear();
      base.ClearItems();
    }

    protected override void InsertItem(int index, LogicProgram item)
    {
      if (item == null)
      {
        throw new ArgumentNullException(nameof(item));
      }

      // Add the logic program to the ContainerAwareObservableCollection.
      // This will throw an exception if the item is already possessed by another ContainerAwareObservableCollection
      // (which, of course, is intended).
      base.InsertItem(index, item);

      // Also find the corresponding _Pou container, to also add the corresponding _Pou there
      var connectedPou = item.GetOrCreateConnectedPou();
      connectedPou.Remove();
      item.Root.Programs.ProgramOrganizationUnits.Add(connectedPou);
    }

    protected override void InsertItemRange(int index, IEnumerable<LogicProgram> collection)
    {
      // This will throw an exception if the item is already possessed by another ContainerAwareObservableCollection
      // (which, of course, is intended).
      base.InsertItemRange(index, collection);

      // It doesn't matter for us where in the _Pou collection the connected pous are stored, we simply insert those anywhere.
      foreach (var item in collection)
      {
        var connectedPou = item.GetOrCreateConnectedPou();
        connectedPou.Remove();
        item.Root.Programs.ProgramOrganizationUnits.Add(connectedPou);
      }
    }

    protected override void RemoveItem(int index)
    {
      var item = this.Items[index];
      item.GetOrCreateConnectedPou().Remove();
      base.RemoveItem(index);
    }

    protected override void RemoveItemRange(IEnumerable<LogicProgram> collection)
    {
      base.RemoveItemRange(collection);
      foreach (var item in collection)
      {
        var connectedPou = item.GetOrCreateConnectedPou();
        connectedPou.Remove();
      }
    }

    protected override void RemoveItemRange(int index, int count)
    {
      var items = this.Items.Skip(index).Take(count);
      foreach (var item in items)
      {
        item.GetOrCreateConnectedPou().Remove();
      }

      base.RemoveItemRange(index, count);
    }

    protected override void SetItem(int index, LogicProgram item)
    {
      var oldItem = this.Items[index];
      var oldConnectedPou = oldItem.GetOrCreateConnectedPou();

      oldItem.Remove();
      oldConnectedPou.Remove();

      base.SetItem(index, item);
      item.Root.Programs.ProgramOrganizationUnits.Add(oldConnectedPou);
    }
  }
}
