using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace zenonApi.Serialization
{
  /// <summary>
  ///   Allows to resolve abstract/base class/interface types to concrete instances during serialization/deserialization.
  ///   Use <see cref="GetNodeNameForSerialization(PropertyInfo, Type, int)"/> to return a name based on the current
  ///   target property and type to retrieve a node name.
  ///   Use <see cref="GetTypeForDeserialization(string, int)"/> to get the concrete target type based on a node name.
  /// </summary>
  public interface IZenonSerializableResolver
  {
    /// <summary>
    ///   Implementing classes are supposed to return a node name based on the given target property, target type, and
    ///   optionally an index.
    /// </summary>
    /// <param name="targetProperty">
    ///   The property, of which the value is about to be serialized.
    ///   If the <paramref name="targetProperty"/> is inheriting from IEnumerable&lt;<see cref="IZenonSerializable"/>&gt;,
    ///   then the <paramref name="targetProperty"/> may refer to an array or list, while the <paramref name="targetType"/>
    ///   is the type of a concrete list item.
    ///   If the <paramref name="targetProperty"/> is not a supported collection type, then the <paramref name="targetType"/>
    ///   is usually the same as the type of the <paramref name="targetProperty"/>.
    /// </param>
    /// <param name="targetType">
    ///   The target type, which is either the type of a collection item from the given <paramref name="targetProperty"/>
    ///   (if the <paramref name="targetProperty"/> is a supported collection type), or the type of the
    ///   <paramref name="targetProperty"/> itself.
    /// </param>
    /// <param name="value">
    ///   The value of the node to transform.
    /// </param>
    /// <param name="index">
    ///   An optional index may be passed, if the <paramref name="targetProperty"/> is a collection type.
    /// </param>
    string GetNodeNameForSerialization(PropertyInfo targetProperty, Type targetType, object value, int index);

    /// <summary>
    ///   Implementing classes are supposed to return a concrete type for deserialization, based on the given node name and optionally
    ///   an index.
    ///   If the given node name is unknown, then null should be returned.
    /// </summary>
    /// <param name="targetProperty">The target property, for which a possible candidate is searched.</param>
    /// <param name="nodeName">The name of a found node.</param>
    /// <param name="node">The node, which is currently processed.</param>
    /// <param name="index">
    ///   An optional index within the source file.
    ///   For single properties, this index will always be null.
    ///   For lists, this index will be the node index.
    /// </param>
    Type GetTypeForDeserialization(PropertyInfo targetProperty, string nodeName, XElement node, int index);
  }
}
