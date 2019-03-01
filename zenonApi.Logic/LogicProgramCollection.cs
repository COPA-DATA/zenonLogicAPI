using System.Collections.Generic;
using System.Linq;
using zenonApi.Collections;
using zenonApi.Logic.Internal;

namespace zenonApi.Logic
{
  public class LogicProgramCollection : ContainerAwareObservableCollection<LogicProgram>
  {
    protected override void ClearItems()
    {
      // Besides deleting all items in the user-visible list, we have to also delete them from
      // LogicProject > _Programs.
      ContainerAwareObservableCollection<_Pou> pouCollection = null;

      foreach (LogicProgram program in this.Items)
      {
        if (pouCollection == null && program != null)
        {
          pouCollection = program.Root.Programs.ProgramOrganizationUnits;
        }

        if (pouCollection == null)
        {
          continue;
        }

        // Find the corresponding POU in the internal collection
        var pousToDelete = pouCollection.Where(x => this.Items.Any(pou => x.Name == pou.Name));
        pouCollection.RemoveRange(pousToDelete);
      }

      base.ClearItems();
    }

    protected override void InsertItem(int index, LogicProgram item)
    {
      if (item != null)
      {
        // Find the corresponding pou container, to also add the corresponding pou there
        // TODO
      }

      base.InsertItem(index, item);
    }

    protected override void InsertItemRange(int index, IEnumerable<LogicProgram> collection)
    {
      // TODO
      base.InsertItemRange(index, collection);
    }

    protected override void RemoveItem(int index)
    {
      // TODO
      base.RemoveItem(index);
    }

    protected override void RemoveItemRange(IEnumerable<LogicProgram> collection)
    {
      // TODO
      base.RemoveItemRange(collection);
    }

    protected override void RemoveItemRange(int index, int count)
    {
      // TODO
      base.RemoveItemRange(index, count);
    }

    protected override void SetItem(int index, LogicProgram item)
    {
      // TODO
      base.SetItem(index, item);
    }
  }
}
