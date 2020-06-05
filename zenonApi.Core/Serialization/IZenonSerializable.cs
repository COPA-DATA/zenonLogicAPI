using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Linq;

namespace zenonApi.Serialization
{
  public interface IZenonSerializable<out TSelf, out TParent, out TRoot>
    : IZenonSerializable<TSelf>
    where TSelf : class, IZenonSerializable<TSelf>
  {
    TParent Parent { get; }
    TRoot Root { get; }
  }

  // ReSharper disable once UnusedTypeParameter : "Unused" type parameter is required for serialization.
  public interface IZenonSerializable<out TSelf> : IZenonSerializable where TSelf : class { }

  public interface IZenonSerializable : INotifyPropertyChanged
  {
    /// <summary>
    /// The name of the item in its XML representation.
    /// </summary>
    string NodeName { get; }

    /// <summary>
    /// Contains all unknown nodes, which are not covered by this API and were found for the current item.
    /// The key specifies the original tag name from XML, the value contains the entire XElement representing it.
    /// </summary>
    Dictionary<string, List<XElement>> UnknownNodes { get; }

    /// <summary>
    /// Contains all unknown attributes, which are not covered by this API and were found for the current item.
    /// The key specifies the original tag name from XML, the value contains the attribute's value.
    /// </summary>
    Dictionary<string, string> UnknownAttributes { get; }

    /// <summary>
    /// Status about the origin and the current state of the object.
    /// </summary>
    zenonSerializableStatusEnum ObjectStatus { get; set; }

    XElement ExportAsXElement();

    string ExportAsString(string xmlEncoding = "utf-8");

    string ExportAsString(Encoding xmlEncoding);

    void ExportAsFile(string fileName, string xmlEncoding = "utf-8");

    void ExportAsFile(string fileName, Encoding xmlEncoding);

    void OnSerialize();

    void OnSerialized();

    void OnDeserialized(string nameOfDeserializedNode);
  }
}
