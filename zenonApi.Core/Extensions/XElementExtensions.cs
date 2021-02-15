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
  }
}
