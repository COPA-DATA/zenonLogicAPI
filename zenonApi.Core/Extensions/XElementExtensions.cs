using System;
using System.Text;
using System.Xml.Linq;

namespace zenonApi.Extensions
{
  public static class XElementExtensions
  {
    public static string GetInnerXml(this XElement element)
    {
      if (element == null)
      {
        return null;
      }

      using (var reader = element.CreateReader())
      {
        reader.MoveToContent();
        return reader.ReadInnerXml();
      }
    }

		public static void SetInnerXml(this XElement element, string innerXml)
    {
      if (element == null)
      {
        return;
      }

      element.Nodes().Remove();
      if (innerXml == null)
      {
        return;
      }

      if (string.IsNullOrEmpty(innerXml))
      {
        element.Value = string.Empty;
        return;
      }

      AddInnerXml(element, innerXml);
    }

    public static void AddInnerXml(this XElement element, string innerXml)
    {
      if (element == null)
      {
        return;
      }

      if (string.IsNullOrEmpty(innerXml))
      {
        return;
      }

      StringBuilder sb = new StringBuilder();
      string name = Guid.NewGuid().ToString("N");
      sb.Append('<').Append('T').Append(name).Append('>')
        .Append(innerXml)
        .Append("</").Append('T').Append(name).Append('>');

      var stringContent = sb.ToString();
      try
      {
        var content = XElement.Parse(stringContent);
        // Valid xml at this point
        foreach (var item in content.Nodes())
        {
          element.Add(item);
        }
      }
      catch
      {
        using (var writer = element.CreateWriter())
        {
          // Note: WriteRaw DOES escape invalid characters, based on the XmlWriterConfiguration of the writer,
          // which can not be modified when working with XElements (therefore the workaround above)
          writer.WriteRaw(innerXml);
        }
      }
    }
	}
}
