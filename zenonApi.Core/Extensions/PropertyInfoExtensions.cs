using System;
using System.Collections.Generic;
using System.Linq;
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

      if (propertyInfo.PropertyType.IsGenericType)
      {
        var genericType = propertyInfo.PropertyType.GetGenericTypeDefinition();
        if (genericType == typeof(IEnumerable<>))
        {
          // This is already an IEnumerable
          definedGenericTypeParameter = propertyInfo.PropertyType.GenericTypeArguments[0];
          if (expectedGenericTypeParameter.IsAssignableFrom(definedGenericTypeParameter))
          {
            return true;
          }

          return false;
        }
      }

      var enumerableType
        = propertyInfo.PropertyType
          .GetInterfaces()
          .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));

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

    public static bool IsEnumOrNullableEnum(this PropertyInfo propertyInfo)
    {
      return IsEnumOrNullableEnum(propertyInfo, out _);
    }

    public static bool IsEnumOrNullableEnum(this PropertyInfo propertyInfo, out bool isNullable)
    {
      if (propertyInfo == null)
      {
        throw new ArgumentNullException();
      }

      isNullable = false;

      var type = propertyInfo.PropertyType;
      if (type.IsEnum)
      {
        return true;
      }

      if (!type.IsGenericType)
      {
        // Neither an enum, nor a generic, cannot be a nullable enum therefore.
        return false;
      }

      var genericType = type.GetGenericTypeDefinition();
      if (genericType != typeof(Nullable<>))
      {
        return false;
      }

      isNullable = true;
      return type.GenericTypeArguments[0].IsEnum;
    }
  }
}
