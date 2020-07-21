using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace zenonApi.Extensions
{
  public static class PropertyInfoExtensions
  {
    public static bool IsEnumerableOf(this PropertyInfo propertyInfo, Type genericTypeParameter)
    {
      if (propertyInfo == null)
      {
        throw new NullReferenceException();
      }

      if (genericTypeParameter == null)
      {
        return false;
      }

      var enumerableType = propertyInfo.PropertyType.GetInterface(typeof(IEnumerable<>).FullName);
      if (enumerableType == null)
      {
        return false;
      }

      var ownGenericType = enumerableType.GenericTypeArguments[0];

      return genericTypeParameter.IsAssignableFrom(ownGenericType);
    }

    public static bool IsEnumerableOf<T>(this PropertyInfo propertyInfo)
    {
      return IsEnumerableOf(propertyInfo, typeof(T));
    }

    public static bool CanBeAssignedTo(this PropertyInfo propertyInfo, Type targetType)
    {
      if (propertyInfo == null)
      {
        throw new NullReferenceException();
      }

      if (targetType == null)
      {
        return false;
      }

      return targetType.IsAssignableFrom(propertyInfo.PropertyType);
    }

    public static bool CanBeAssignedTo<T>(this PropertyInfo propertyInfo)
    {
      return CanBeAssignedTo(propertyInfo, typeof(T));
    }
  }
}
