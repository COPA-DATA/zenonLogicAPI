using System.Collections.Generic;

namespace zenonApi.MetaDescription.Parser.AdapterAnalysis
{
  public interface IScadaEnum : ITranslatable
  {
    List<IScadaEnumValue> Values { get; }
  }
}
