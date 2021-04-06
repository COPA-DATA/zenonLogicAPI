using System;
using System.Xml;

namespace zenonApi.Serialization
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  // ReSharper disable once InconsistentNaming : "zenon" is always written lowercase.
  public class zenonSerializableRawFormatAttribute : zenonSerializableBaseAttribute
  {
    /// <summary>
    /// Creates a new instance of the <see cref="zenonSerializableRawFormatAttribute"/>.
    /// The <paramref name="nodeName"/> is required to control how the node is named in the XML.
    /// </summary>
    /// <param name="nodeName">The name of the resulting XML node.</param>
    public zenonSerializableRawFormatAttribute(string nodeName)
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
    /// The name of the propertie's representation as a node in XML.
    /// </summary>
    public string NodeName { get; private set; }

    /// <summary>
    /// Controls the serialization order of properties within an <see cref="IZenonSerializable"/>.
    /// </summary>
    public byte NodeOrder { get; set; }

    /// <summary>
    /// Specifies if a node shall be omitted if it is null or contains no items in case of lists (default = true).
    /// </summary>
    public bool OmitIfNull { get; set; } = true;



    #region Internal base class overrides
    internal override zenonSerializableAttributeType AttributeType => zenonSerializableAttributeType.RawNode;

    internal override byte InternalOrder => this.NodeOrder;

    internal override string InternalName => this.NodeName;

    internal override bool InternalOmitIfNull => this.OmitIfNull;

    internal override bool InternalEncapsulateChildsIfList => false;

    internal override Type InternalConverter => null; // Not supported for raw contents

    internal override Type InternalTypeResolver => null; // Not supported for raw contents
    #endregion
  }
}
