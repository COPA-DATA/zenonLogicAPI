using System;
using System.Collections.Generic;
using System.Text;
using zenonApi.Logic.SerializationConverters;
using zenonApi.Serialization;

namespace zenonApi.Logic.SequenceFlowChart
{
  /// <summary>
  /// Describes a line segment drawn in a cell of the SFC chart area.
  /// </summary>
  public class Line : zenonSerializable<Line>
  {
    public override string NodeName => "SFClines";

    /// <summary>
    /// X coordinate in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("dx", AttributeOrder = 0)]
    public int X { get; set; }

    /// <summary>
    /// Y coordinate in grid units (mandatory).
    /// </summary>
    [zenonSerializableAttribute("dy", AttributeOrder = 1)]
    public int Y { get; set; }

    /// <summary>
    /// Set this value to true to draw a vertical line to the top.
    /// </summary>
    [zenonSerializableAttribute("vtop", AttributeOrder = 2, Converter = typeof(NumericalBooleanConverter))]
    public bool DrawTopLine { get; set; }

    /// <summary>
    /// Set this value to true to draw a vertical line to the bottom.
    /// </summary>
    [zenonSerializableAttribute("vbottom", AttributeOrder = 3, Converter = typeof(NumericalBooleanConverter))]
    public bool DrawBottomLine { get; set; }

    /// <summary>
    /// Set this value to true to draw a horizontal line to the left.
    /// </summary>
    [zenonSerializableAttribute("hleft", AttributeOrder = 4, Converter = typeof(NumericalBooleanConverter))]
    public bool DrawLeftLine { get; set; }

    /// <summary>
    /// Set this value to true to draw a horizontal line to the right.
    /// </summary>
    [zenonSerializableAttribute("hleft", AttributeOrder = 5, Converter = typeof(NumericalBooleanConverter))]
    public bool DrawRightLine { get; set; }


    /// <summary>
    /// Set this value to true to draw horizontal lines as two lines.
    /// </summary>
    [zenonSerializableAttribute("dbline", AttributeOrder = 6, Converter = typeof(NumericalBooleanConverter))]
    public bool DrawHorizontalLinesDoubled { get; set; }

    /// <summary>
    /// Set this value to false to indicate that horizontal and vertical segments have no logical connection.
    /// </summary>
    [zenonSerializableAttribute("cross", AttributeOrder = 7, Converter = typeof(NumericalBooleanConverter))]
    public bool LogicalConnectionOfHorizontalAndVerticalLines { get; set; }
  }
}
