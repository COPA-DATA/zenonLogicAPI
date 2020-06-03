using System;
using System.Linq;
using System.Text;

namespace zenonApi.Extensions
{
  public static class StringExtensions
  {
    /// <summary>
    /// A list of invalid characters for names in zenon.
    /// </summary>
    private static readonly char[] IllegalCharacters = { '/', '\\', ':', '*', '?', /*'<', '>', */ '|', '\"', '#', '%', '@', '{', '}' };

    /// <summary>
    /// An array of possible C language keywords, which we forbid for logic names.
    /// </summary>
    private static readonly string[] CKeywords =
    {
      "auto",
      "break",
      "case",
      "char",
      "const",
      "continue",
      "default",
      "do",
      "double",
      "else",
      "enum",
      "extern",
      "float",
      "for",
      "goto",
      "if",
      "inline", //(since C99)
      "int",
      "long",
      "register",
      "restrict", // (since C99)
      "return",
      "short",
      "signed",
      "sizeof",
      "static",
      "struct",
      "switch",
      "typedef",
      "union",
      "unsigned",
      "void",
      "volatile",
      "while"
    };

    /// <summary>
    /// Returns true if the given <paramref name="pattern"/> is contained, using the given
    /// <paramref name="comparison"/> type.
    /// </summary>
    /// <param name="self">The current string.</param>
    /// <param name="pattern">The string to search within the current string.</param>
    /// <param name="comparison">The comparison type to use.</param>
    public static bool Contains(this string self, string pattern, StringComparison comparison)
    {
      return self.LastIndexOf(pattern, comparison) != -1;
    }

    /// <summary>
    /// Returns true, if the current string is a valid zenon variable name, otherwise false.
    /// </summary>
    /// <param name="self">The current string.</param>
    /// <param name="allowSpaces">Set this to false to not permit spaces (default = true).</param>
    public static bool IsValidZenonName(this string self, bool allowSpaces = true)
    {
      if (String.IsNullOrWhiteSpace(self))
      {
        return false;
      }
      if (!allowSpaces && self.Contains(" "))
      {
        return false;
      }
      if (IllegalCharacters.Any((c) => self.Contains(c)))
      {
        return false;
      }

      return true;
    }

    /// <summary>
    /// Returns true, if the current string is a valid zenon logic variable name, otherwise false.
    /// If this returns true, the current string is also a valid zenon name.
    /// </summary>
    /// <param name="self">The current string.</param>
    /// <param name="allowSpaces">Set this to true to permit spaces (default = false).</param>
    public static bool IsValidZenonLogicName(this string self, bool allowSpaces = false)
    {
      if (!self.IsValidZenonName(allowSpaces)
        || (!allowSpaces && self.Contains(" "))
        || self.Contains("__")
        || !self.IsValidCVariableName(allowSpaces))
      {
        // * Two followed underlines are not allowed in zenon Logic
        // * Taken standard C keywords are not allowed in zenon Logic
        return false;
      }

      string asciiOnly = self.RemoveNonAsciiChars();
      if (asciiOnly.Length != self.Length)
      {
        // zenon Logic variables must not contain Unicode chars
        return false;
      }

      return true;
    }


    /// <summary>
    /// Returns true, if the current string does not contain C keywords and does comply to the overall naming rules.
    /// </summary>
    /// <param name="self">The current string.</param>
    /// <param name="allowSpaces">
    /// Set this to true to permit spaces (default = false).
    /// If it is set to true, each "word" will be checked according to the overall rules.
    /// </param>
    /// <param name="mustStartCLike">
    /// If this is set to true, then the current string must start either with an underscore or a letter (a-z, A-Z)
    /// (default = true).
    /// </param>
    /// <param name="canContainSpecialChars">
    /// If this is set to true, the variables may contain other chars than letters (a-z, A-Z), numbers and underscores
    /// (default = false).
    /// </param>
    public static bool IsValidCVariableName(this string self, bool allowSpaces = false, bool mustStartCLike = true, bool canContainSpecialChars = false)
    {
      if (string.IsNullOrWhiteSpace(self))
      {
        return false;
      }

      var values = self.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      if (!allowSpaces && values.Length > 1)
      {
        // No spaces allowed, but spaces found
        return false;
      }

      // Check each splitted value for validity
      foreach (var value in values)
      {
        if (mustStartCLike
          && self[0] != '_'
          && !(self[0] >= 'a' && self[0] <= 'z')
          && !(self[0] >= 'A' && self[0] <= 'Z'))
        {
          // zenon Logic variables must start with either an underscore or a letter
          return false;
        }

        if (CKeywords.Any((keyword) => value == keyword))
        {
          return false;
        }

        if (!canContainSpecialChars)
        {
          for (int i = 0; i < value.Length; i++)
          {
            if (self[i] != '_'
                && !(self[i] >= 'a' && self[i] <= 'z')
                && !(self[i] >= 'A' && self[i] <= 'Z')
                && !(self[i] >= '0' && self[i] <= '9')
                )
            {
              return false;
            }
          }
        }
      }

      // All checks passed
      return true;
    }

    /// <summary>
    /// Removes all non-ASCII chars from the current string and returns the result.
    /// </summary>
    /// <param name="self">The current string.</param>
    public static string RemoveNonAsciiChars(this string self)
    {
      if (self == null)
      {
        return null;
      }

      // We can use the ASCII encoder to get rid of non-ASCII characters
      // (The standard conversion would replace unicode characters with '?')
      Encoding encoding = Encoding.GetEncoding(Encoding.ASCII.CodePage,
        new EncoderReplacementFallback(String.Empty),
        new DecoderReplacementFallback(String.Empty));

      return encoding.GetString(encoding.GetBytes(self));
    }

    public static string ReplaceNonUnicodeAlphaNumerics(this string self, char replacement = '_')
    {
      if (self == null)
      {
        return null;
      }

      unsafe
      {
        fixed (char* first = self)
        {
          char* current = first;
          while (*current != 0)
          {
            if (!char.IsLetterOrDigit(*current))
            {
              *current = replacement;
            }

            current++;
          }

          current++;
        }
      }

      return self;
    }
  }
}
