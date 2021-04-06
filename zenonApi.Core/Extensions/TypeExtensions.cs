using System;
using System.Collections.Generic;

namespace zenonApi.Extensions
{
  public static class TypeExtensions
  {
    public static IEnumerable<Type> InterfacesByName(this Type t, string filter, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
      if (t == null)
      {
        yield break;
      }

      foreach (Type interfaceItem in t.GetInterfaces())
      {
        if (interfaceItem.Name.Contains(filter, comparison))
        {
          yield return interfaceItem;
        }
      }
    }

    public static IEnumerable<Type> BaseTypesByName(this Type t, string filter, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
      if (t == null)
      {
        yield break;
      }

      if (t.BaseType != null)
      {
        if (t.BaseType.Name.Contains(filter, comparison))
        {
          yield return t.BaseType;
        }

        foreach (Type subType in t.BaseType.BaseTypesByName(filter))
        {
          yield return subType;
        }
      }
    }

    public static IEnumerable<Type> BaseTypesAndInterfacesByName(this Type t, string filter, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
      if (t == null)
      {
        yield break;
      }

      foreach (Type subType in t.BaseTypesByName(filter, comparison))
      {
        yield return subType;
      }

      foreach (Type interfaceItem in t.InterfacesByName(filter, comparison))
      {
        yield return interfaceItem;
      }
    }
  }
}
