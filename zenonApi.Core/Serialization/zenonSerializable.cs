using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using zenonApi.Collections;

// TODO: Nullables to be allowed
// TODO: Exception and other messages in Resource file

namespace zenonApi.Serialization
{
  // ReSharper disable once InconsistentNaming : "zenon" is always written lowercase.
  public abstract class zenonSerializable<TSelf, TParent, TRoot>
    : ContainerAwareCollectionItem<TSelf>, IZenonSerializable<TSelf, TParent, TRoot>
    where TSelf : class, IZenonSerializable<TSelf>
    where TParent : class, IZenonSerializable
    where TRoot : class, IZenonSerializable
  {
    [DoNotNotify]
    public virtual TParent Parent
    {
      // Casts are required here, since we hide the interface properties in the base class
      get => ((IContainerAwareCollectionItem)this).ItemContainerParent as TParent;
      protected set
      {
        ((IContainerAwareCollectionItem)this).ItemContainerParent = value;
      }
    }

    [DoNotNotify]
    public virtual TRoot Root
    {
      // Casts are required here, since we hide the interface properties in the base class
      get => ((IContainerAwareCollectionItem)this).ItemContainerRoot as TRoot;
      protected set
      {
        ((IContainerAwareCollectionItem)this).ItemContainerRoot = value;
      }
    }
  }

  // ReSharper disable once InconsistentNaming : "zenon" is always written lowercase.
  public abstract class zenonSerializable<TSelf> : IZenonSerializable<TSelf>
    where TSelf : class, IZenonSerializable<TSelf>
  { 
    /// <summary>
    /// Standard indentation value for xml format
    /// </summary>
    private const int DefaultXmlIndentation = 3;

    #region Interface implementation
    /// <summary>
    /// The name of the item in its XML representation.
    /// </summary>
    [DoNotNotify]
    public abstract string NodeName { get; }

    /// <summary>
    /// Contains all unknown nodes, which are not covered by this API and were found for the current item.
    /// The key specifies the original tag name from XML, the value contains all the entire XElements representing it.
    /// </summary>
    [DoNotNotify]
    public Dictionary<string, List<XElement>> UnknownNodes { get; } = new Dictionary<string, List<XElement>>();

    /// <summary>
    /// Contains all unknown attributes, which are not covered by this API and were found for the current item.
    /// The key specifies the original tag name from XML, the value contains the attribute's value.
    /// </summary>
    [DoNotNotify]
    public Dictionary<string, string> UnknownAttributes { get; } = new Dictionary<string, string>();

    /// <summary>
    /// Status about the origin and the current state of the object.
    /// </summary>
    [DoNotNotify]
    public zenonSerializableStatusEnum ObjectStatus { get; set; } = zenonSerializableStatusEnum.New;

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      // update ObjectStatus to signal that the object was modified during its live time
      this.ObjectStatus = zenonSerializableStatusEnum.Modified;
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    #region Private/Protected methods
    /// <summary>
    /// Protected member, containing all converters which were previously initialized during Serialization/Deserialization.
    /// </summary>
    // ReSharper disable once StaticMemberInGenericType : This is intended.
    protected static Dictionary<Type, IZenonSerializationConverter> ConverterCache = new Dictionary<Type, IZenonSerializationConverter>();

    protected static IZenonSerializationConverter GetConverter(Type converterType)
    {
      if (converterType == null)
      {
        throw new ArgumentNullException(nameof(converterType));
      }

      IZenonSerializationConverter converterInstance;
      if (ConverterCache.ContainsKey(converterType))
      {
        converterInstance = ConverterCache[converterType];
      }
      else
      {
        converterInstance = (IZenonSerializationConverter)Activator.CreateInstance(converterType, true);
        ConverterCache[converterType] = converterInstance;
      }

      return converterInstance;
    }
    #endregion

    #region Export methods

    /// <summary>
    /// Exports the current object as an XElement.
    /// </summary>
    public virtual XElement ExportAsXElement()
    {
      // Create a node for the current element, check for all properties with a zenonSerializableAttribute-
      // or zenonSerializableNode-Attribute and append them
      XElement current = new XElement(this.NodeName);

      // Get all the properties together with their attributes in tuples
      var properties = this.GetType().GetRuntimeProperties().Select(x => (property: x, attributes: x.GetCustomAttributes()));

      // Group the tuples by the required attribute types and order them if required by their specified serialization order
      var attributeMappings = properties
        .Select(x => (x.property, attribute: x.attributes.OfType<zenonSerializableBaseAttribute>().FirstOrDefault()))
        .Where(x => x.attribute != null)
        .OrderBy(x => x.attribute.InternalOrder);

      foreach (var attributeMapping in attributeMappings)
      {
        switch (attributeMapping.attribute.AttributeType)
        {
          case zenonSerializableAttributeType.Attribute:
            ExportAttribute(current, this, attributeMapping.property, attributeMapping.attribute);
            break;
          case zenonSerializableAttributeType.Node:
            ExportNode(current, this, attributeMapping.property, attributeMapping.attribute);
            break;
          case zenonSerializableAttributeType.NodeContent:
            ExportNodeContent(current, this, attributeMapping.property, attributeMapping.attribute);
            break;
          case zenonSerializableAttributeType.RawNode:
            ExportRaw(current, this, attributeMapping.property, attributeMapping.attribute);
            break;
          default:
            // e.g. enum attribute
            throw new NotSupportedException(
              String.Format(
                Strings.ErrorMessageAttributeNotSupported,
                attributeMapping.attribute.AttributeType,
                attributeMapping.property.Name));
        }
      }

      ExportUnknownAttributes(current, this);
      ExportUnknownNodes(current, this);

      this.ObjectStatus = zenonSerializableStatusEnum.Deserialized;

      return current;
    }

    /// <summary>
    /// Exports the current object as an XML formatted string. If the given <paramref name="xmlEncoding"/>
    /// is null, then UTF-8 is used.
    /// </summary>
    /// <param name="xmlEncoding">The XML encoding to use.</param>
    public virtual string ExportAsString(string xmlEncoding = "utf-8")
    {
      if (string.IsNullOrWhiteSpace(xmlEncoding))
      {
        xmlEncoding = "utf-8";
      }
      
      XElement self = this.ExportAsXElement();
      XDocument document = new XDocument
      {
        Declaration = new XDeclaration("1.0", xmlEncoding, "yes")
      };

      document.Add(self);

      using (MemoryStream memoryStream = new MemoryStream())
      using (XmlTextWriter writer = new XmlTextWriter(memoryStream, Encoding.GetEncoding(xmlEncoding)))
      {
        writer.Indentation = DefaultXmlIndentation;
        writer.Formatting = Formatting.Indented;
        document.Save(writer);
        writer.Flush();

        memoryStream.Position = 0;
        using (StreamReader sr = new StreamReader(memoryStream))
        {
          return sr.ReadToEnd();
        }
      }
    }

    /// <summary>
    /// Exports the current object as an XML formatted string. If the given <paramref name="xmlEncoding"/>
    /// is null, then UTF-8 is used.
    /// </summary>
    /// <param name="xmlEncoding">The XML encoding to use.</param>
    public virtual string ExportAsString(Encoding xmlEncoding)
    {
      if (xmlEncoding == null)
      {
        xmlEncoding = Encoding.UTF8;
      }

      return ExportAsString(xmlEncoding.BodyName);
    }

    /// <summary>
    /// Exports the current object as XML into the given file.
    /// If the file does already exist, it will be overwritten.
    /// If the given <paramref name="xmlEncoding"/> is null, then UTF-8 is used.
    /// </summary>
    /// <param name="fileName">The path to the target file.</param>
    /// <param name="xmlEncoding">The XML encoding to use.</param>
    public virtual void ExportAsFile(string fileName, string xmlEncoding = "utf-8")
    {
      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentException(string.Format(Strings.ErrorMessageParameterIsNullOrWhitespace, nameof(fileName)), nameof(fileName));
      }

      if (string.IsNullOrWhiteSpace(xmlEncoding))
      {
        xmlEncoding = "utf-8";
      }

      XElement self = this.ExportAsXElement();
      XDocument document = new XDocument
      {
        Declaration = new XDeclaration("1.0", xmlEncoding, "yes")
      };

      document.Add(self);

      using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.GetEncoding(xmlEncoding)))
      {
        writer.Indentation = DefaultXmlIndentation;
        writer.Formatting = Formatting.Indented;
        document.Save(writer);
      }
    }

    /// <summary>
    /// Exports the current object as XML into the given file.
    /// If the file does already exist, it will be overwritten.
    /// If the given <paramref name="xmlEncoding"/> is null, then UTF-8 is used.
    /// </summary>
    /// <param name="fileName">The path to the target file.</param>
    /// <param name="xmlEncoding">The XML encoding to use.</param>
    public virtual void ExportAsFile(string fileName, Encoding xmlEncoding)
    {
      if (xmlEncoding == null)
      {
        xmlEncoding = Encoding.UTF8;
      }

      ExportAsFile(fileName, xmlEncoding.BodyName);
    }

    private static void ExportAttribute(XElement target, IZenonSerializable source, PropertyInfo property, zenonSerializableBaseAttribute attributeAttribute)
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
          IZenonSerializationConverter converterInstance = GetConverter(attributeAttribute.InternalConverter);
          target.SetAttributeValue(attributeAttribute.InternalName, converterInstance.Convert(property.GetValue(source)));
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


    private static void ExportNode(XElement target, IZenonSerializable source, PropertyInfo property, zenonSerializableBaseAttribute nodeAttribute)
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
          MethodInfo exportMethod = property.PropertyType.GetMethod(nameof(ExportAsXElement), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
          // ReSharper disable once PossibleNullReferenceException : Cannot be null, since provided by the interface.
          XElement child = (XElement)exportMethod.Invoke(sourceValue, null);

          target.Add(child);
        }
        else if (typeof(IList).IsAssignableFrom(property.PropertyType))
        {
          IList list = (IList)sourceValue;
          if (list == null)
          {
            return;
          }

          // Currently we only support a list of IZenonSerializable and collections deriving from it
          Type genericParameterType = property.PropertyType.GetGenericArguments().FirstOrDefault();
          if (genericParameterType == null && typeof(IList).IsAssignableFrom(property.PropertyType))
          {
            Type baseType = property.PropertyType.BaseType;
            while (baseType != null && genericParameterType == null)
            {
              genericParameterType = baseType.GetGenericArguments().FirstOrDefault();
              baseType = baseType.BaseType;
            }
          }
          if (!typeof(IZenonSerializable).IsAssignableFrom(genericParameterType))
          {
            throw new NotImplementedException(
              String.Format(Strings.ErrorMessageInvalidSerializationListType, nameof(IZenonSerializable)));
          }

          if (nodeAttribute.InternalEncapsulateChildsIfList)
          {
            XElement listWithChilds = new XElement(nodeAttribute.InternalName);

            foreach (IZenonSerializable listItem in list)
            {
              // ReSharper disable once PossibleNullReferenceException : "genericParameterType" cannot be null at this point.
              MethodInfo exportMethod = genericParameterType.GetMethod(nameof(ExportAsXElement), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
              // ReSharper disable once PossibleNullReferenceException : "exportMethod" cannot be null at this point.
              XElement child = (XElement)exportMethod.Invoke(listItem, null);

              listWithChilds.Add(child);
            }

            target.Add(listWithChilds);
          }
          else
          {
            foreach (IZenonSerializable listItem in list)
            {
              // ReSharper disable once PossibleNullReferenceException : "genericParameterType" cannot be null at this point.
              MethodInfo exportMethod = genericParameterType.GetMethod(nameof(ExportAsXElement), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
              // ReSharper disable once PossibleNullReferenceException : "exportMethod" cannot be null at this point.
              XElement child = (XElement)exportMethod.Invoke(listItem, null);

              target.Add(child);
            }
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


    private static void ExportNodeContent(XElement target, IZenonSerializable source, PropertyInfo property, zenonSerializableBaseAttribute contentAttribute)
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
          IZenonSerializationConverter converterInstance = GetConverter(contentAttribute.InternalConverter);
          target.Value = converterInstance.Convert(sourceValue);
        }
        else
        {
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

          string stringValue = sourceValue.ToString();
          target.SetValue(stringValue);
        }
      }
    }

    private static void ExportRaw(XElement target, IZenonSerializable source, PropertyInfo property, zenonSerializableBaseAttribute rawAttribute)
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
          Debug.WriteLine($"zenonSerializable - Export Warning: Property \"{property.Name}\" is implemented with the {nameof(zenonSerializableRawFormatAttribute)} "
            + "attribute. If the usage of this property is not an exception, ask the API creators to implement an API supported version of it.");

          target.Add(sourceValue);
        }
        else
        {
          target.Add(new XElement(rawAttribute.InternalName));
        }
      }
    }


    private static void ExportUnknownAttributes(XElement target, IZenonSerializable source)
    {
      if (source?.UnknownAttributes == null || source.UnknownAttributes.Count == 0)
      {
        return;
      }

      foreach (var attributeMapping in source.UnknownAttributes)
      {
        target.SetAttributeValue(attributeMapping.Key, attributeMapping.Value);
        Debug.WriteLine($"zenonSerializable - Export Warning: Unknown attribute \"{attributeMapping.Key}\" found for XML node \"{source.NodeName}\".");
      }
    }

    private static void ExportUnknownNodes(XElement target, IZenonSerializable source)
    {
      if (source?.UnknownNodes == null || source.UnknownNodes.Count == 0)
      {
        return;
      }

      foreach (var nodeMapping in source.UnknownNodes)
      {
        var list = nodeMapping.Value;
        if (list == null)
        {
          continue;
        }

        foreach (var singleNode in list)
        {
          if (singleNode == null)
          {
            continue;
          }

          target.Add(singleNode);
          Debug.WriteLine($"zenonSerializable - Export Warning: Unknown sub-node \"{nodeMapping.Key}\" found in node \"{source.NodeName}\".");
        }
      }
    }
    #endregion

    #region Import methods
    public static TSelf Import(XElement source, object parent = null, object root = null)
    {
      // TODO: After importing, the original XDocument is changed, therefore we MUST copy the XElement first in our final "Import" method
      // The current method should be kept internal anyway
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
        resultWithParent.ItemContainerParent = parent;
        resultWithParent.ItemContainerRoot = root;
      }

      // Find all the attributes and properties of the current type for deserialization
      foreach (var property in t.GetRuntimeProperties())
      {
        if (property.GetSetMethod(true) == null)
        {
          // No setter, nothing to do at all
          continue;
        }

        // Each of the following methods alters the source XML, by removing everything which was found from it
        ImportAttribute(result, source, property);
        ImportNode(result, source, property);
        ImportNodeContent(result, source, property);
        ImportRaw(result, source, property);
      }

      // Since everything was removed what was found, all of the remaining are unknown elements, which we can handle explicitly
      ImportUnknownAttributes(result, source);
      ImportUnknownNodes(result, source);

      result.ObjectStatus = zenonSerializableStatusEnum.Loaded;

      return result;
    }

    private static void ImportChilds(PropertyInfo targetListProperty, TSelf parentContainer, List<XElement> xmlNodes, zenonSerializableNodeAttribute attribute)
    {
      IList list = (IList)Activator.CreateInstance(targetListProperty.PropertyType, true);

      // Get the generic type, so that we can instantiate entries for the list (this should be a zenonSerializable)
      Type genericParameter = targetListProperty.PropertyType.GetGenericArguments().FirstOrDefault();
      if (genericParameter == null && typeof(IList).IsAssignableFrom(targetListProperty.PropertyType))
      {
        Type baseType = targetListProperty.PropertyType.BaseType;
        while (baseType != null && genericParameter == null)
        {
          genericParameter = baseType.GetGenericArguments().FirstOrDefault();
          baseType = baseType.BaseType;
        }
      }
      if (typeof(IZenonSerializable).IsAssignableFrom(genericParameter))
      {
        // ReSharper disable once PossibleNullReferenceException : "genericParameter" will never be null at this position.
        MethodInfo importMethod = genericParameter.GetMethod(nameof(Import), BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        IContainerAwareCollectionItem caItem = parentContainer as IContainerAwareCollectionItem;
        object root = null;

        if (caItem != null)
        {
          root = caItem.ItemContainerRoot;
          if (root == null && caItem.ItemContainerParent == null)
          {
            // the current object is most likely the desired root
            root = parentContainer;
          }
        }

        foreach (var xmlNode in xmlNodes)
        {
          object child;

          if (attribute.EncapsulateChildsIfList)
          {
            foreach (var subNode in xmlNode.Elements())
            {
              // ReSharper disable once PossibleNullReferenceException : "importMethod" comes from the interface,
              // will never be null at this point.
              child = importMethod.Invoke(null, new[] { subNode, parentContainer, root });
              list.Add(child);
            }
          }
          else
          {
            // The child is not capable of handling parents and roots, therefore pass null
            // ReSharper disable once PossibleNullReferenceException : "importMethod" comes from the interface,
            // will never be null at this point.
            child = importMethod.Invoke(null, new[] { xmlNode, parentContainer, root });
            list.Add(child);
          }
        }
      }
      else
      {
        throw new Exception($"A list with a {nameof(zenonSerializableNodeAttribute)} must use "
          + $"{nameof(IZenonSerializable)} or derived classes as the generic parameter. Invalid property's name: {nameof(targetListProperty.Name)}");
      }

      targetListProperty.SetValue(parentContainer, list);
    }


    private static void ImportAttribute(TSelf target, XElement sourceXml, PropertyInfo property)
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
          IZenonSerializationConverter converterInstance = GetConverter(zenonAttribute.Converter);
          property.SetValue(target, converterInstance.Convert(xmlAttribute.Value));
        }
        else if (property.PropertyType.IsEnum)
        {
          // If no converter is registered, try to convert it manually
          foreach (var value in Enum.GetValues(property.PropertyType))
          {
            var field = property.PropertyType.GetField(value.ToString());

            // Try to match the enum value either by the attribute or the string name
            var attribute = field.GetCustomAttribute<zenonSerializableEnumAttribute>();
            if ((attribute != null && xmlAttribute.Value == attribute.Name)
              || xmlAttribute.Value == field.Name)
            {
              property.SetValue(target, value);

              // Remove the attribute, so that we can check later on for unhandled ones
              xmlAttribute.Remove();
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

        // Remove the attribute, so that we can check later on for unhandled ones
        xmlAttribute.Remove();
      }
    }


    private static void ImportNode(TSelf target, XElement sourceXml, PropertyInfo property)
    {
      var attribute = property.GetCustomAttribute<zenonSerializableNodeAttribute>();
      if (attribute != null)
      {
        // ReSharper disable once RedundantEnumerableCastCall : Not redundant, required to retrieve the correct format for later.
        var xmlNodes = sourceXml.Elements(attribute.NodeName).OfType<XNode>().Cast<XElement>().ToList();

        // Currently we only support deserializing to a concrete List types, not IEnumerable or similar, maybe in the future
        if (typeof(IList).IsAssignableFrom(property.PropertyType))
        {
          // Create the list which will hold the instances
          ImportChilds(property, target, xmlNodes, attribute);
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
            object root = (target as IContainerAwareCollectionItem)?.ItemContainerRoot;
            // ReSharper disable once PossibleNullReferenceException : Method reference can never be null, since coming from the interface.
            var child = importMethod.Invoke(null, new[] { xmlNodes.First(), target, root ?? target });

            property.SetValue(target, child);
          }
        }
        else if (xmlNodes.Count == 1)
        {
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

        xmlNodes.Remove();
      }
    }


    private static void ImportNodeContent(TSelf target, XElement sourceXml, PropertyInfo property)
    {
      var zenonAttribute = property.GetCustomAttribute<zenonSerializableNodeContentAttribute>();
      if (zenonAttribute != null)
      {
        // Check if there is a converter
        if (zenonAttribute.Converter != null)
        {
          IZenonSerializationConverter converterInstance = GetConverter(zenonAttribute.Converter);
          property.SetValue(target, converterInstance.Convert(sourceXml.Value));
        }
        else if (property.PropertyType.IsEnum)
        {
          // If no converter is registered, try to convert it manually
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


    private static void ImportRaw(TSelf target, XElement sourceXml, PropertyInfo property)
    {
      var prop = property.GetCustomAttribute<zenonSerializableRawFormatAttribute>();
      if (prop != null)
      {
        if (!typeof(XElement).IsAssignableFrom(property.PropertyType))
        {
          throw new Exception($"Usage of {nameof(zenonSerializableRawFormatAttribute)} is only permitted if the property "
            + $"type is or is derived from XElement (applied on property: {property.Name}).");
        }

        // ReSharper disable once RedundantEnumerableCastCall : Not redundant.
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
        Debug.WriteLine($"zenonSerializable - Import Warning: Property \"{property.Name}\" is implemented with the {nameof(zenonSerializableRawFormatAttribute)} "
          + "attribute. If the usage of this property is not an exception, ask the API creators to implement an API supported version of it.");

        property.SetValue(target, xmlNodes.FirstOrDefault());
        xmlNodes.Remove();
      }
    }


    private static void ImportUnknownAttributes(TSelf target, XElement sourceXml)
    {
      // All attributes which were not yet removed are yet unhandled
      foreach (var attribute in sourceXml.Attributes())
      {
        string name = attribute.Name.ToString();
        target.UnknownAttributes.Add(name, attribute.Value);
        Debug.WriteLine($"zenonSerializable - Import Warning: Unknown attribute \"{name}\" found for XML node \"{target.NodeName}\".");
      }
    }

    private static void ImportUnknownNodes(TSelf target, XElement sourceXml)
    {
      // All nodes which were not yet removed are yet unhandled
      foreach (var node in sourceXml.Elements())
      {
        List<XElement> list;
        string nodeName = node.Name.ToString();
        if (target.UnknownNodes.ContainsKey(nodeName))
        {
          list = target.UnknownNodes[nodeName];
        }
        else
        {
          list = new List<XElement>();
          target.UnknownNodes[nodeName] = list;
        }

        list.Add(node);
        Debug.WriteLine($"zenonSerializable - Import Warning: Unknown sub-node \"{nodeName}\" found in node \"{target.NodeName}\".");
      }
    }
    #endregion
  }
}
