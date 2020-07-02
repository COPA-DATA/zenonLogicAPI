using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using zenonApi.Serialization;

namespace zenonApi.Converters
{
  /// <summary>
  ///   A <see cref="IZenonSerializationConverter"/>, which supports multiple string values given in form of a
  ///   comma separated list. Commas within braces "{}" , brackets "[]", angle brackets "&lt;&gt;", and parentheses "()"
  ///   are ignored if an opening and closing tag exists. Nesting is allowed.
  ///   Furthermore, commas within single and double quotes are also ignored.
  /// </summary>
  public class CommaSeparatedConverter : IZenonSerializationConverter
  {
    // TODO Later: Since this is a quite useful converter: Move it to the core API
    private static readonly Regex ValuesRegex = new Regex(@"
      (?:
        # Match recursive braces = {}
        (?:
          # Match anything else than the beginning of our separators
          [^,\{\<\(\[\""\']*
          (?:
            (?:
      	      # On each matching '{', push our named 'Count' group to the Regex stack
              (?'Count'{)
      		    # Match everything else than braces
              [^{}]*
        	  )+
      	    # Pop our 'Count' group from the Stack, clear it, assign the substring between 'Count' and 'Content' to 'Content'
            (?'Content-Count'})+
          )*
      	  # Match recursive angle brackets = <>
          # (basically the same as above, but we need to use the same 'Count' group)
          (?:
            (?:
              (?'Count'<)
              [^<>]*
        	  )+
            (?'Content-Count'>)+
          )*
      	  # Match recursive parantheses = ()
          (?:
            (?:
              (?'Count'\()
              [^\(\)]*
        	  )+
            (?'Content-Count'\))+
          )*
      	  # Match recursive brackets = []
          (?:
            (?:
              (?'Count'\[)
              [^\[\]]*
        	  )+
            (?'Content-Count'\])+
          )*
          # Match all within double quotes (ignores commas in it)
          (?:\""[^\""]*\"")*
          # Match all within single quotes (ignores commas in it)
          (?:\'[^\']*\')*
          # Match anything else than commas
          [^,]*
        )
        # Conditional expression; If our Count is NOT empty (or undefined), fail with a zero-width negative lookahead
        # (occurs, if the limiters were unbalanced)
        (?(Count)(?!))
      )",
      RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

    /// <inheritdoc />
    public string Convert(object source)
    {
      if (source == null)
      {
        return null;
      }

      var values = source as IEnumerable<string>;
      if (values == null)
      {
        throw new Exception($"Invalid datatype. A {nameof(CommaSeparatedConverter)} requires an IEnumerable<string> as input.");
      }

      return string.Join(", ", values);
    }

    /// <inheritdoc />
    public object Convert(string source)
    {
      List<string> result = new List<string>();
      if (string.IsNullOrWhiteSpace(source))
      {
        return result;
      }

      Match match = ValuesRegex.Match(source);
      while (match.Success)
      {
        string item = match.Value.Trim();
        if (string.IsNullOrWhiteSpace(item))
        {
          continue;
        }

        result.Add(item);
        match = match.NextMatch();
      }

      return result;
    }
  }
}
