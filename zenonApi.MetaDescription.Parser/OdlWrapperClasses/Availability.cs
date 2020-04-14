using G = Scada.Common.OdlParser;

namespace zenonApi.MetaDescription.Parser.OdlWrapperClasses
{
  class Availability
  {
    public G.AvailabilityType EditorAvailablityType { get; }
    public G.AvailabilityType RuntimeAvailablityType { get; }
    public G.AvailabilityType VbaAvailablityType { get; }
    public G.AvailabilityType VstaAvailablityType { get; }

    public Availability()
    {
      EditorAvailablityType = G.AvailabilityType.Undefined;
      RuntimeAvailablityType = G.AvailabilityType.Undefined;
      VbaAvailablityType = G.AvailabilityType.Undefined;
      VstaAvailablityType = G.AvailabilityType.Undefined;
    }

    public Availability(G.Availability OdlParserAvb)
    {
      EditorAvailablityType = OdlParserAvb.EditorAvailablityType;
      RuntimeAvailablityType = OdlParserAvb.RuntimeAvailablityType;
      VbaAvailablityType = OdlParserAvb.VbaAvailablityType;
      VstaAvailablityType = OdlParserAvb.VstaAvailablityType;
    }
  }
}
