using zenonApi.Collections;
using zenonApi.Serialization;

namespace zenonApi.Logic.FunctionBlockDiagrams
{
  /// <summary>
  /// Describes a Function Block Diagram.
  /// </summary>
  public class FunctionBlockDiagramDefinition : zenonSerializable<FunctionBlockDiagramDefinition, ILogicProgram, LogicProject>
  {
    protected override string NodeName => "sourceFBD";

    #region Serializable Attributes
    /// <summary>
    /// The width of a grid cell in accurate units, which is mandatory.
    /// The FBD diagram is drawn in a logical "grid", as shown in the FBD editor.
    /// All coordinates of graphic objects are expressed in grid units.
    /// </summary>
    [zenonSerializableAttribute("sxcell", AttributeOrder = 0)]
    public uint GridCellWidth { get; set; }

    /// <summary>
    /// The height of a grid cell in accurate units, which is mandatory.
    /// The FBD diagram is drawn in a logical "grid", as shown in the FBD editor.
    /// All coordinates of graphic objects are expressed in grid units.
    /// </summary>
    [zenonSerializableAttribute("sycell", AttributeOrder = 1)]
    public uint GridCellHeight { get; set; }

    /// <summary>
    /// The zero based index of the output pin in the source object,
    /// which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("pinfrom", AttributeOrder = 2)]
    public uint PinFrom { get; set; }

    /// <summary>
    /// The zero based index of the input pin in the destination object,
    /// which is mandatory.
    /// </summary>
    [zenonSerializableAttribute("pinto", AttributeOrder = 3)]
    public uint PinTo { get; set; }
    #endregion

    #region Serializable Nodes
    /// <summary>
    /// Contains the "variable" boxes in a FBD diagram.
    /// </summary>
    [zenonSerializableNode("FBDvarbox", NodeOrder = 0)]
    public ContainerAwareObservableCollection<FunctionBlockDiagramVariableBox> VariableBoxes { get; set; }

    /// <summary>
    /// Contains the "comment" boxes in a FBD diagram.
    /// </summary>
    [zenonSerializableNode("FBDcomment", NodeOrder = 1)]
    public ContainerAwareObservableCollection<FunctionBlockDiagramComment> CommentBoxes { get; set; }

    /// <summary>
    /// Contains the "network break" separation lines in a FBD diagram.
    /// </summary>
    [zenonSerializableNode("FBDbreak", NodeOrder = 2)]
    public ContainerAwareObservableCollection<FunctionBlockDiagramNetworkBreak> NetworkBreaks { get; set; }

    /// <summary>
    /// Contains functions or FB boxes in a FBD diagram.
    /// </summary>
    [zenonSerializableNode("FBDbox", NodeOrder = 3)]
    public ContainerAwareObservableCollection<FunctionBlockDiagramBox> Boxes { get; set; }

    /// <summary>
    /// Contains coils in a function block diagram.
    /// </summary>
    [zenonSerializableNode("FBDcontact", NodeOrder = 4)]
    public ContainerAwareObservableCollection<FunctionBlockDiagramContact> Contacts { get; set; }

    /// <summary>
    /// Contains coils in a function block diagram.
    /// </summary>
    [zenonSerializableNode("FBDcoil", NodeOrder = 5)]
    public ContainerAwareObservableCollection<FunctionBlockDiagramCoil> Coils { get; set; }

    /// <summary>
    /// Contains vertical bars (rails) in a FBD diagram.
    /// </summary>
    [zenonSerializableNode("FBDvrail", NodeOrder = 6)]
    public ContainerAwareObservableCollection<FunctionBlockDiagramVerticalRail> VerticalRails { get; set; }

    /// <summary>
    /// Contains labels of the FBD diagram.
    /// </summary>
    [zenonSerializableNode("FBDlabel", NodeOrder = 7)]
    public ContainerAwareObservableCollection<FunctionBlockDiagramLabel> Labels { get; set; }
    
    /// <summary>
    /// Contains jump or "&lt;RETURN&gt;" items of the FBD diagram.
    /// </summary>
    [zenonSerializableNode("FBDjump", NodeOrder = 8)]
    public ContainerAwareObservableCollection<FunctionBlockDiagramJump> Jumps { get; set; }

    /// <summary>
    /// Contains the user defined cornerns in a FBD diagram.
    /// </summary>
    [zenonSerializableNode("FBDcorner", NodeOrder = 8)]
    public ContainerAwareObservableCollection<FunctionBlockDiagramCorner> Corners { get; set; }
    
    /// <summary>
    /// Contains the connection lines in a FBD diagram.
    /// </summary>
    [zenonSerializableNode("FBDcorner", NodeOrder = 8)]
    public ContainerAwareObservableCollection<FunctionBlockDiagramLine> Lines { get; set; }
    #endregion
  }
}
