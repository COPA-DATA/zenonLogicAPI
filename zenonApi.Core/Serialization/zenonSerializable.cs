using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using System.Collections;
using zenonApi.Collections;
using System.Diagnostics;

// TODO: Add converters for zenonSerializableNode
// TODO: Nullables to be allowed

namespace zenonApi.Serialization
{
  public abstract class zenonSerializable<TSelf, TParent, TRoot>
    : ContainerAwareCollectionItem<TSelf>, IZenonSerializable<TSelf, TParent, TRoot>
    where TSelf : class, IZenonSerializable<TSelf>
    where TParent : class, IZenonSerializable
    where TRoot : class, IZenonSerializable
  {
    public virtual TParent Parent
    {
      // Casts are required here, since we hide the interface properties in the base class
      get => ((IContainerAwareCollectionItem)this).ContainerItemParent as TParent;
      protected set
      {
        ((IContainerAwareCollectionItem)this).ContainerItemParent = value;
      }
    }

    public virtual TRoot Root
    {
      // Casts are required here, since we hide the interface properties in the base class
      get => ((IContainerAwareCollectionItem)this).ContainerItemRoot as TRoot;
      protected set
      {
        ((IContainerAwareCollectionItem)this).ContainerItemRoot = value;
      }
    }
  }

  public abstract class zenonSerializable<TSelf> : IZenonSerializable<TSelf>
    where TSelf : class, IZenonSerializable<TSelf>
  {
    #region Interface implementation
    /// <summary>
    /// The name of the item in its XML representation.
    /// </summary>
    public abstract string NodeName { get; }

    /// <summary>
    /// Contains all unknown nodes, which are not covered by this API and were found for the current item.
    /// The key specifies the original tag name from XML, the value contains the entire XElement representing it.
    /// </summary>
    public Dictionary<string, XElement> UnknownNodes => unknownNodes;

    /// <summary>
    /// Contains all unknown attributes, which are not covered by this API and were found for the current item.
    /// The key specifies the original tag name from XML, the value contains the attribute's value.
    /// </summary>
    public Dictionary<string, string> UnknownAttributes => unknownAttributes;
    #endregion


    #region Private/Protected methods
    // TODO: Clear the converter cache after serialization/deserialization?
    /// <summary>
    /// Protected member, containing all converters which were previously initialized during Serialization/Deserialization.
    /// </summary>
    protected static Dictionary<Type, IZenonSerializationConverter> converterCache = new Dictionary<Type, IZenonSerializationConverter>();

    protected static IZenonSerializationConverter getConverter(Type converterType)
    {
      if (converterType == null)
      {
        throw new ArgumentNullException(nameof(converterType));
      }

      IZenonSerializationConverter converterInstance = null;
      if (converterCache.ContainsKey(converterType))
      {
        converterInstance = converterCache[converterType];
      }
      else
      {
        converterInstance = (IZenonSerializationConverter)Activator.CreateInstance(converterType, true);
        converterCache[converterType] = converterInstance;
      }

      return converterInstance;
    }

    private Dictionary<string, XElement> unknownNodes = new Dictionary<string, XElement>();
    private Dictionary<string, string> unknownAttributes = new Dictionary<string, string>();
    #endregion


    #region Export to XElement
    public virtual XElement Export()
    {
      // Create a node for the current element, check for all properties with a zenonSerializableAttribute-
      // or zenonSerializableNode-Attribute and append them
      XElement current = new XElement(this.NodeName);

      // Get all the properties together with their attributes in tuples
      var properties = this.GetType().GetRuntimeProperties().Select(x => (property: x, attributes: x.GetCustomAttributes()));

      // Group the tuples by the required attribute types and order them if required by their specified serialization order
      var attributeMappings = properties
        .Select(x => (property: x.property, attribute: x.attributes.OfType<zenonSerializableBaseAttribute>().FirstOrDefault()))
        .Where(x => x.attribute != null)
        .OrderBy(x => x.attribute.InternalOrder);

      foreach (var attributeMapping in attributeMappings)
      {
        switch (attributeMapping.attribute.AttributeType)
        {
          case zenonSerializableAttributeType.Attribute:
            exportAttribute(current, this, attributeMapping.property, attributeMapping.attribute);
            break;
          case zenonSerializableAttributeType.Node:
            exportNode(current, this, attributeMapping.property, attributeMapping.attribute);
            break;
          case zenonSerializableAttributeType.NodeContent:
            exportNodeContent(current, this, attributeMapping.property, attributeMapping.attribute);
            break;
          case zenonSerializableAttributeType.RawNode:
            exportRaw(current, this, attributeMapping.property, attributeMapping.attribute);
            break;
          default:
            // e.g. enum attribute
            throw new NotSupportedException($"Attribute {attributeMapping.attribute.AttributeType} is not supported for usage on {attributeMapping.property.Name}.");
        }
      }

      return current;
    }


    private static void exportAttribute(XElement target, IZenonSerializable source, PropertyInfo property, zenonSerializableBaseAttribute attributeAttribute)
    {
      if (property.GetGetMethod(true) == null)
      {
        // No getter, nothing to do
        return;
      }

      if (attributeAttribute != null)
      {
        object sourceValue = property.GetValue(source);
        if (attributeAttribute.InternalOmitIfNull && sourceValue == null)
        {
          // Omit the whole node
          return;
        }

        // Check if there is an converter for this property
        if (attributeAttribute.InternalConverter != null)
        {
          IZenonSerializationConverter converterInstance = getConverter(attributeAttribute.InternalConverter);
          // Ensure to call the correct method overload of the converter by using the (object)-cast
          target.SetAttributeValue(attributeAttribute.InternalName, converterInstance.Convert((object)property.GetValue(source)));
        }
        else
        {
          var value = property.GetValue(source);
          if (value == null)
          {
            // If the value is null, we omit the attribute
            return;
          }

          var valueType = value.GetType();
          if (valueType.IsEnum)
          {
            // Try to find a zenonSerializable attribute to write the correct value
            var attribute = valueType.GetField(value.ToString()).GetCustomAttribute<zenonSerializableEnumAttribute>();
            if (attribute != null)
            {
              // Set the value from the attribute, otherwise use the default string value (after the outer if-clause)
              target.SetAttributeValue(attributeAttribute.InternalName, attribute.Name);
              return;
            }
          }

          string stringValue = property.GetValue(source)?.ToString();
          target.SetAttributeValue(attributeAttribute.InternalName, stringValue);
        }
      }
    }


    private static void exportNode(XElement target, IZenonSerializable source, PropertyInfo property, zenonSerializableBaseAttribute nodeAttribute)
    {
      if (property.GetGetMethod(true) == null)
      {
        // No getter, nothing to do
        return;
      }

      if (nodeAttribute != null)
      {
        object sourceValue = property.GetValue(source);
        if (nodeAttribute.InternalOmitIfNull && sourceValue == null)
        {
          // Omit the whole node
          return;
        }

        if (typeof(IZenonSerializable).IsAssignableFrom(property.PropertyType))
        {
          MethodInfo exportMethod = property.PropertyType.GetMethod(nameof(Export), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
          XElement child = (XElement)exportMethod.Invoke(sourceValue, null);

          target.Add(child);
        }
        else if (property.PropertyType.IsEnum)
        {
          // TODO: zenonSerializableEnum? Required at all?
        }
        else if (typeof(IList).IsAssignableFrom(property.PropertyType))
        {
          IList list = (IList)sourceValue;
          if (list == null)
          {
            return;
          }

          // Currently we only support a list of IZenonSerializable
          var genericParameterType = property.PropertyType.GenericTypeArguments.First();
          if (!typeof(IZenonSerializable).IsAssignableFrom(genericParameterType))
          {
            throw new NotImplementedException($"Currently, only lists of types derived by {nameof(IZenonSerializable)} are supported.");
          }

          foreach (IZenonSerializable listItem in list)
          {
            MethodInfo exportMethod = genericParameterType.GetMethod(nameof(Export), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            XElement child = (XElement)exportMethod.Invoke(listItem, null);

            target.Add(child);
          }
        }
        else
        {
          // Just write the string representation of the property as the value
          XElement child = new XElement(nodeAttribute.InternalName);
          var value = sourceValue?.ToString();
          if (value != null)
          {
            child.Value = value;
          }

          target.Add(child);
        }
      }
    }


    private static void exportNodeContent(XElement target, IZenonSerializable source, PropertyInfo property, zenonSerializableBaseAttribute contentAttribute)
    {
      if (property.GetGetMethod(true) == null)
      {
        // No getter, nothing to do
        return;
      }

      if (contentAttribute != null)
      {
        object sourceValue = property.GetValue(source);
        if (sourceValue == null)
        {
          return;
        }

        // Check if there is an converter for this property
        if (contentAttribute.InternalConverter != null)
        {
          IZenonSerializationConverter converterInstance = getConverter(contentAttribute.InternalConverter);
          target.Value = converterInstance.Convert(sourceValue);
        }
        else
        {
          if (sourceValue == null)
          {
            // If the value is null, we set the content to null too.
            target.Value = null;
            return;
          }

          var valueType = sourceValue.GetType();
          if (valueType.IsEnum)
          {
            // Try to find a zenonSerializable attribute to write the correct value
            var attribute = valueType.GetField(sourceValue.ToString()).GetCustomAttribute<zenonSerializableEnumAttribute>();
            if (attribute != null)
            {
              // Set the value from the attribute, otherwise use the default string value (after the outer if-clause)
              target.SetValue(attribute.Name);
              return;
            }
          }

          string stringValue = sourceValue?.ToString();
          target.SetValue(stringValue);
        }
      }
    }


    private static void exportRaw(XElement target, IZenonSerializable source, PropertyInfo property, zenonSerializableBaseAttribute rawAttribute)
    {
      if (property.GetGetMethod(true) == null)
      {
        // No getter, nothing to do
        return;
      }

      if (rawAttribute != null)
      {
        if (!typeof(XElement).IsAssignableFrom(property.PropertyType))
        {
          throw new Exception($"Usage of {nameof(zenonSerializableRawFormatAttribute)} is only permitted if the property "
            + $"type is or is derived from XElement (applied on property: {property.Name}).");
        }

        XElement sourceValue = (XElement)property.GetValue(source);
        if (rawAttribute.InternalOmitIfNull && sourceValue == null)
        {
          // Omit the whole node
          return;
        }

        if (sourceValue != null)
        {
          // We use the raw attribute only for items, which are not required by our team, therefore we print warnings if they are used
          Debug.WriteLine($"Warning: Property {property.Name} is implemented with the {nameof(zenonSerializableRawFormatAttribute)} "
            + "attribute. If the usage of this property is not an exception, ask the API creators to implement an API supported version of it.");

          target.Add(sourceValue);
        }
        else
        {
          target.Add(new XElement(rawAttribute.InternalName));
        }
      }
    }
    #endregion


    #region Import from XElement
    public static TSelf Import(XElement source, object parent = null, object root = null)
    {
      // Create an instance of the current type
      Type t = typeof(TSelf);
      var result = (TSelf)Activator.CreateInstance(t, true);

      // The source element must match the node name
      if (source.Name != result.NodeName)
      {
        throw new Exception($"Expected {result.NodeName}, but got {source.Name}");
      }

      if (result is IContainerAwareCollectionItem resultWithParent)
      {
        resultWithParent.ContainerItemParent = parent;
        resultWithParent.ContainerItemRoot = root;
      }

      // Find all the attributes and properties of the current type for deserialization
      foreach (var property in t.GetRuntimeProperties())
      {
        if (property.GetSetMethod(true) == null)
        {
          // No setter, nothing to do at all
          continue;
        }

        importAttribute(result, source, property);
        importNode(result, source, property);
        importNodeContent(result, source, property);
        importRaw(result, source, property);
      }

      return result;
    }

    private static void importChilds(PropertyInfo targetListProperty, TSelf parentContainer, List<XElement> xmlNodes)
    {
      IList list = (IList)Activator.CreateInstance(targetListProperty.PropertyType, true);

      // Get the generic type, so that we can instantiate entries for the list (this should be a zenonSerializable)
      Type genericParameter = targetListProperty.PropertyType.GetGenericArguments().First();
      if (typeof(IZenonSerializable).IsAssignableFrom(genericParameter))
      {
        foreach (var xmlNode in xmlNodes)
        {
          MethodInfo importMethod = genericParameter.GetMethod(nameof(Import), BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
          object child = null;

          if (parentContainer is IContainerAwareCollectionItem caItem)
          {
            object root = caItem.ContainerItemRoot;
            if (root == null && caItem.ContainerItemParent == null)
            {
              // the current object is most likely the desired root
              root = parentContainer;
            }

            child = importMethod.Invoke(null, new object[] { xmlNode, parentContainer, root });
          }
          else
          {
            // The child is not capable of handling parents and roots, therefore pass null
            child = importMethod.Invoke(null, new object[] { xmlNode, parentContainer, null });
          }

          list.Add(child);
        }
      }
      else
      {
        throw new Exception($"A list with a {nameof(zenonSerializableNodeAttribute)} must use "
          + $"{nameof(IZenonSerializable)} or derived classes as the generic parameter. Invalid property's name: {nameof(targetListProperty.Name)}");
      }

      targetListProperty.SetValue(parentContainer, list);
    }


    private static void importAttribute(TSelf target, XElement sourceXml, PropertyInfo property)
    {
      var zenonAttribute = property.GetCustomAttribute<zenonSerializableAttributeAttribute>();
      if (zenonAttribute != null)
      {
        var xmlAttribute = sourceXml.Attributes(zenonAttribute.AttributeName).FirstOrDefault();
        if (xmlAttribute == null)
        {
          return;
        }

        // Check if there is a converter
        if (zenonAttribute.Converter != null)
        {
          IZenonSerializationConverter converterInstance = getConverter(zenonAttribute.Converter);
          property.SetValue(target, converterInstance.Convert(xmlAttribute.Value));
        }
        else if (property.PropertyType.IsEnum)
        {
          // If no converter is registered, try to convert it manually
          // TODO: Special types like guids would be nice to consider here

          foreach (var value in Enum.GetValues(property.PropertyType))
          {
            var field = property.PropertyType.GetField(value.ToString());

            // Try to match the enum value either by the attribute or the string name
            var attribute = field.GetCustomAttribute<zenonSerializableEnumAttribute>();
            if ((attribute != null && xmlAttribute.Value == attribute.Name)
              || xmlAttribute.Value == field.Name)
            {
              property.SetValue(target, value);
              return;
            }
          }

          // If neither the attribute nor the string name matches, something went wrong
          throw new Exception($"Cannot set value \"{xmlAttribute.Value}\" for {property.PropertyType.Name}, either a "
            + $"{nameof(zenonSerializableEnumAttribute)} must be set for the enum fields or "
            + $"the name must exactly match the XML value.");
        }
        else if (typeof(IConvertible).IsAssignableFrom(property.PropertyType))
        {
          var converted = Convert.ChangeType(xmlAttribute.Value, property.PropertyType);
          property.SetValue(target, converted);
        }
        else
        {
          throw new Exception($"Cannot convert types without an {nameof(IZenonSerializationConverter)}, which do not implement {nameof(IConvertible)}.");
        }
      }
    }


    private static void importNode(TSelf target, XElement sourceXml, PropertyInfo property)
    {
      var prop = property.GetCustomAttribute<zenonSerializableNodeAttribute>();
      if (prop != null)
      {
        var xmlNodes = sourceXml.Elements(prop.NodeName).OfType<XNode>().Cast<XElement>().ToList();

        // Currently we only support deserializing to a concrete List types, not IEnumerable or similar, maybe in the future
        if (typeof(IList).IsAssignableFrom(property.PropertyType))
        {
          // Create the list which will hold the instances
          importChilds(property, target, xmlNodes);
        }
        else if (typeof(IZenonSerializable).IsAssignableFrom(property.PropertyType))
        {
          if (xmlNodes.Count > 1)
          {
            throw new Exception($"Expected a IList<{nameof(IZenonSerializable)}>, but got {nameof(IZenonSerializable)}.");
          }

          if (xmlNodes.Count == 1)
          {
            MethodInfo importMethod = property.PropertyType.GetMethod(nameof(Import), BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            object root = (target as IContainerAwareCollectionItem)?.ContainerItemRoot;
            var child = importMethod.Invoke(null, new object[] { xmlNodes.First(), target, root ?? target });

            property.SetValue(target, child);
          }
        }
        else if (xmlNodes.Count == 1)
        {
          // TODO: There are no converters yet for this case (no zenonSerializable)
          // Just try to deserialize the value directly
          if (typeof(IConvertible).IsAssignableFrom(property.PropertyType))
          {
            var value = Convert.ChangeType(xmlNodes.First().Value, property.PropertyType);
            property.SetValue(target, value);
          }
          else
          {
            throw new Exception($"Cannot convert types without an {nameof(IZenonSerializationConverter)}, which do not implement {nameof(IConvertible)}.");
          }
        }
      }
    }


    private static void importNodeContent(TSelf target, XElement sourceXml, PropertyInfo property)
    {
      var zenonAttribute = property.GetCustomAttribute<zenonSerializableNodeContentAttribute>();
      if (zenonAttribute != null)
      {
        if (sourceXml.Value == null)
        {
          return;
        }

        // Check if there is a converter
        if (zenonAttribute.Converter != null)
        {
          IZenonSerializationConverter converterInstance = getConverter(zenonAttribute.Converter);
          property.SetValue(target, converterInstance.Convert(sourceXml.Value));
        }
        else if (property.PropertyType.IsEnum)
        {
          // If no converter is registered, try to convert it manually
          // TODO: Special types like guids would be nice to consider here

          foreach (var value in Enum.GetValues(property.PropertyType))
          {
            var field = property.PropertyType.GetField(value.ToString());

            // Try to match the enum value either by the attribute or the string name
            var attribute = field.GetCustomAttribute<zenonSerializableEnumAttribute>();
            if ((attribute != null && sourceXml.Value == attribute.Name)
              || sourceXml.Value == field.Name)
            {
              property.SetValue(target, value);
              return;
            }
          }

          // If neither the attribute nor the string name matches, something went wrong
          throw new Exception($"Cannot set value \"{sourceXml.Value}\" for {property.PropertyType.Name}, either a "
            + $"{nameof(zenonSerializableEnumAttribute)} must be set for the enum fields or "
            + $"the name must exactly match the XML value.");
        }
        else if (typeof(IConvertible).IsAssignableFrom(property.PropertyType))
        {
          var converted = Convert.ChangeType(sourceXml.Value, property.PropertyType);
          property.SetValue(target, converted);
        }
        else
        {
          throw new Exception($"Cannot convert types without an {nameof(IZenonSerializationConverter)}, which do not implement {nameof(IConvertible)}.");
        }
      }
    }


    private static void importRaw(TSelf target, XElement sourceXml, PropertyInfo property)
    {
      var prop = property.GetCustomAttribute<zenonSerializableRawFormatAttribute>();
      if (prop != null)
      {
        if (!typeof(XElement).IsAssignableFrom(property.PropertyType))
        {
          throw new Exception($"Usage of {nameof(zenonSerializableRawFormatAttribute)} is only permitted if the property "
            + $"type is or is derived from XElement (applied on property: {property.Name}).");
        }

        var xmlNodes = sourceXml.Elements(prop.NodeName).OfType<XNode>().Cast<XElement>().ToList();
        if (xmlNodes.Count == 0)
        {
          return;
        }
        else if (xmlNodes.Count > 1)
        {
          throw new Exception($"{nameof(zenonSerializableRawFormatAttribute)} can currently not be used if more than one entry "
            + $"exists. Failing for node {prop.NodeName} on property {property.Name}.");
        }

        // We use the raw attribute only for items, which are not required by our team, therefore we print warnings if they are used
        Debug.WriteLine($"Warning: Property {property.Name} is implemented with the {nameof(zenonSerializableRawFormatAttribute)} "
          + "attribute. If the usage of this property is not an exception, ask the API creators to implement an API supported version of it.");

        property.SetValue(target, xmlNodes.FirstOrDefault());
      }
    }
    #endregion
  }
}
