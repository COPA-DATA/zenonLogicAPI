namespace zenonApi.Logic
{
  public class LogicVariableAttributes
  {
    /// <summary>
    /// Signals that this variable is an INPUT parameter.
    /// For UDFBs only.
    /// </summary>
    public bool In { get; set; }

    /// <summary>
    /// Signals that this variable is an OUTPUT parameter.
    /// For UDFBs only.
    /// </summary>
    public bool Out { get; set; }

    /// <summary>
    /// Signals that this variable is an external variable.
    /// </summary>
    public bool External { get; set; }

    /// <summary>
    /// Signals that this variable is read only.
    /// </summary>
    public bool Constant { get; set; }
  }
}
