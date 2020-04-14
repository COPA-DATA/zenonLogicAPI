using System.Collections.Generic;
using G = Scada.Common.OdlParser;
namespace zenonApi.MetaDescription.Parser.OdlWrapperClasses
{
  class ComMember
  {
    public Availability Availability { get; set; }
    private string _helpContext;
    private string _helpId;
    private Dictionary<string, string> _parameters;
    private string _datatype;

    public ComParameters Parameters { get; }
    public string DataType { get { return _datatype; } }

    public ComMember(G.ComMember member)
    {
      Availability = new Availability(member.Availability);
      _helpContext = member.HelpContext;
      _helpId = member.HelpId;
      _datatype = member.DataType;
      _parameters = new Dictionary<string, string>();
      if (member.Parameters != null)
        Parameters = new ComParameters(member.Parameters);
    }
  }
}
