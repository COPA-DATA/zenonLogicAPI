using System.Collections.Generic;

namespace zenonApi.MetaDescription.Parser.AdapterAnalysis
{
  public interface IScadaInterface : ITranslatable
  {

    IEnumerable<IScadaMethod> Methods { get; }

    IEnumerable<IScadaEvent> Events { get; }

    IEnumerable<IScadaProperty> Properties { get; }

  }
}
