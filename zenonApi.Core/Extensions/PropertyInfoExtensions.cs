using System;
using System.Collections.Generic;
using System.Reflection;

namespace zenonApi.Extensions
{
  public static class PropertyInfoExtensions
  {
    public static bool IsEnumerableOf(this PropertyInfo propertyInfo, Type definedGenericTypeParameter)
      => IsEnumerableOf(propertyInfo, definedGenericTypeParameter, out _);

    public static bool IsEnumerableOf(this PropertyInfo propertyInfo, Type expectedGenericTypeParameter, out Type definedGenericTypeParameter)
    {
      if (propertyInfo == null)
      {
        throw new NullReferenceException();
      }

      if (expectedGenericTypeParameter == null)
      {
        definedGenericTypeParameter = null;
        return false;
      }

      var enumerableType = propertyInfo.PropertyType.GetInterface(typeof(IEnumerable<>).FullName);
      if (enumerableType == null)
      {
        definedGenericTypeParameter = null;
        return false;
      }

      definedGenericTypeParameter = enumerableType.GenericTypeArguments[0];

      return expectedGenericTypeParameter.IsAssignableFrom(definedGenericTypeParameter);
    }

    public static bool IsEnumerableOf<T>(this PropertyInfo propertyInfo)
    {
      return IsEnumerableOf(propertyInfo, typeof(T));
    }

    public static bool IsEnumerableOf<T>(this PropertyInfo propertyInfo, out Type definedGenericTypeParameter)
    {
      return IsEnumerableOf(propertyInfo, typeof(T), out definedGenericTypeParameter);
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
