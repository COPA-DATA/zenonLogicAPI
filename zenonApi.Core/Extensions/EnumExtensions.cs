using System;
using System.Reflection;

namespace zenonApi.Extensions
{
  public static class EnumExtensions
  {
    public static TAttribute[] GetEnumValueAttributes<TEnum, TAttribute>(this TEnum value)
      where TEnum : Enum
      where TAttribute : Attribute
    {
      FieldInfo fi = typeof(TEnum).GetField(value.ToString());
      return (TAttribute[])fi.GetCustomAttributes(typeof(TAttribute), false);
    }
  }
}
