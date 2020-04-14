namespace zenonApi.MetaDescription.Parser.AdapterAnalysis
{
  public interface IScadaProperty : ITranslatable
  {
    bool IsMethodInHost { get; }
  }
}
