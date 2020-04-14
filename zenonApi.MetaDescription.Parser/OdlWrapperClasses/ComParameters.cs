using System.Collections.Generic;
using G = Scada.Common.OdlParser;

namespace zenonApi.MetaDescription.Parser.OdlWrapperClasses
{
  class ComParameters
  {
    public Dictionary<string, string> Parameters { get; }

    public ComParameters(G.ComParameters parameters)
    {
      Parameters = new Dictionary<string, string>();
      foreach (var p in parameters.Parameters)
      {
        Parameters.Add(p.Key, p.Value);
      }
    }
  }
}
