using System;

namespace zenonApi.Extensions
{
  public static class StringExtensions
  {
    public static bool Contains(this string self, string pattern, StringComparison comparison)
    {
      return self.LastIndexOf(pattern, comparison) != -1;
    }
  }
}
