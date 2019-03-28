using System;
using System.Xml;

namespace zenonApi.Serialization
{
  /// <summary>
  /// Set this attribute to properties of an <see cref="IZenonSerializable"/> to create a node for it in the
  /// resulting XML.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class zenonSerializableNodeAttribute : zenonSerializableBaseAttribute
  {
    /// <summary>
    /// Creates a new instance of the <see cref="zenonSerializableNodeAttribute"/>.
    /// The <paramref name="nodeName"/> is required to control how the node is named in the XML.
    /// </summary>
    /// <param name="nodeName">The name of the resulting XML node.</param>
    public zenonSerializableNodeAttribute(string nodeName)
    {
      if (string.IsNullOrWhiteSpace(nodeName))
      {
        throw new Exception("No node name is given.");
      }

      try
      {
        XmlConvert.VerifyName(nodeName);
      }
      catch (Exception ex)
      {
        throw new Exception($"Invalid node name: \"{nodeName}\"", ex);
      }

      this.NodeName = nodeName;
    }

    /// <summary>
    /// The name of the properties' representation as a node in XML.
    /// </summary>
    public string NodeName { get; private set; }

    /// <summary>
    /// If the <see cref="zenonSerializableNodeAttribute"/> is applied to an IList, then this property defines
    /// if a node for every item is generated (false, default), or if an extra node layer with the <see cref="NodeName"/>
    /// is created and filled with the child nodes.
    /// </summary>
    public bool EncapsulateChildsIfList { get; set; }

    /// <summary>
    /// Controls the serialization order of properties within an <see cref="IZenonSerializable"/>.
    /// </summary>
    public byte NodeOrder { get; set; }

    /// <summary>
    /// Specifies if a node shall be omitted if it is null or contains no items in case of lists (default = true).
    /// </summary>
    public bool OmitIfNull { get; set; } = true;


    #region Internal base class overrides
    internal override zenonSerializableAttributeType AttributeType => zenonSerializableAttributeType.Node;

    internal override byte InternalOrder => this.NodeOrder;

    internal override string InternalName => this.NodeName;

    internal override bool InternalEncapsulateChildsIfList => this.EncapsulateChildsIfList;

    internal override bool InternalOmitIfNull => this.OmitIfNull;

    internal override Type InternalConverter => null; // No converters are allowed for nodes
    #endregion
  }
}
