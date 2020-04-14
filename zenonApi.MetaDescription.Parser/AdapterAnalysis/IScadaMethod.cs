using System.Collections.Generic;

namespace zenonApi.MetaDescription.Parser.AdapterAnalysis
{
  public interface IScadaMethod : ITranslatable
  {
    List<IScadaParameter> Parameters { get; }
  }
}
