using System.Collections.Generic;

namespace zenonApi.MetaDescription.Parser.AdapterAnalysis
{
  public interface IScadaEvent : ITranslatable
  {
    IEnumerable<IScadaParameter> Parameters { get; }
  }
}
