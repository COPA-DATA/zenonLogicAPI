using System.Collections.Generic;

namespace zenonApi.MetaDescription.Parser.AdapterAnalysis
{
  public interface IScadaNamespace
  {
    string Name { get; }
    IEnumerable<IScadaInterface> Interfaces { get; }
    IEnumerable<IScadaEnum> Enumerations { get; }
  }
}
