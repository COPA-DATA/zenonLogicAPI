using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace zenonApi.Core.Extensions
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
