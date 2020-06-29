﻿using PropertyChanged;
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
using zenonApi.Extensions;

// TODO: Exception and other messages in Resource file
// TODO: Refactor this whole file, this POC grew quite a lot

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
    protected zenonSerializable()
    {
      this.NodeName = this.GetType().Name.ReplaceNonUnicodeAlphaNumerics();
    }

    /// <summary>
    /// Standard indentation value for xml format
    /// </summary>
    private const int DefaultXmlIndentation = 3;

    #region Interface implementation
    /// <summary>
    /// The name of the item in its XML representation.
    /// </summary>
    [DoNotNotify]
    public virtual string NodeName { get; }

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

    /// <inheritdoc />
    [DoNotNotify]
    public string UnknownNodeContent { get; set; }

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

    // ReSharper disable once StaticMemberInGenericType : This is intended.
    protected static Dictionary<Type, IZenonSerializableResolver> ResolverCache = new Dictionary<Type, IZenonSerializableResolver>();

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

    protected static IZenonSerializableResolver GetResolver(Type resolverType)
    {
      if (resolverType == null)
      {
        throw new ArgumentNullException(nameof(resolverType));
      }

      IZenonSerializableResolver resolverInstance;
      if (ResolverCache.ContainsKey(resolverType))
      {
        resolverInstance = ResolverCache[resolverType];
      }
      else
      {
        resolverInstance = (IZenonSerializableResolver)Activator.CreateInstance(resolverType, true);
        ResolverCache[resolverType] = resolverInstance;
      }

      return resolverInstance;
    }

    /// <inheritdoc />
    void IZenonSerializable.OnSerialize() => BeforeSerialize();

    protected virtual void BeforeSerialize() { }

    /// <inheritdoc />
    void IZenonSerializable.OnSerialized() => AfterSerialized();

    protected virtual void AfterSerialized() { }

    /// <inheritdoc />
    void IZenonSerializable.OnDeserialized(string nameOfDeserializedNode) => AfterDeserialized(nameOfDeserializedNode);

    protected virtual void AfterDeserialized(string nameOfDeserializedNode) { }
    #endregion

    #region Export methods

    /// <summary>
    /// Exports the current object as an XElement.
    /// </summary>
    public XElement ExportAsXElement()
    {
      BeforeSerialize();
      var result = InternalExportAsXElement();
      AfterSerialized();
      return result;
    }

    /// <summary>
    ///   This method is intended to be called only within other public export methods, since it does not call
    ///   <see cref="AfterSerialized()"/>.
    /// </summary>
    private XElement InternalExportAsXElement()
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
      ExportUnknownContent(current, this);

      this.ObjectStatus = zenonSerializableStatusEnum.Deserialized;

      return current;
    }

    /// <summary>
    /// Exports the current object as an XML formatted string. If the given <paramref name="xmlEncoding"/>
    /// is null, then UTF-8 is used.
    /// </summary>
    /// <param name="xmlEncoding">The XML encoding to use.</param>
    public string ExportAsString(string xmlEncoding = null)
    {
      BeforeSerialize();

      if (string.IsNullOrWhiteSpace(xmlEncoding))
      {
        xmlEncoding = "utf-8";
      }

      XElement self = this.InternalExportAsXElement();
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
          string result = sr.ReadToEnd();
          AfterSerialized();

          return result;
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
    public virtual void ExportAsFile(string fileName, string xmlEncoding = null)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentException(string.Format(Strings.ErrorMessageParameterIsNullOrWhitespace, nameof(fileName)), nameof(fileName));
      }

      BeforeSerialize();

      if (string.IsNullOrWhiteSpace(xmlEncoding))
      {
        xmlEncoding = "utf-8";
      }

      XElement self = this.InternalExportAsXElement();
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

      AfterSerialized();
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

    public virtual void ExportAsStream(Stream targetStream, string xmlEncoding = null)
    {
      if (string.IsNullOrWhiteSpace(xmlEncoding))
      {
        xmlEncoding = "utf-8";
      }

      ExportAsStream(targetStream, Encoding.GetEncoding(xmlEncoding));
    }

    public virtual void ExportAsStream(Stream targetStream, Encoding xmlEncoding = null)
    {
      BeforeSerialize();

      if (xmlEncoding == null)
      {
        xmlEncoding = Encoding.UTF8;
      }

      XElement self = this.InternalExportAsXElement();
      XDocument document = new XDocument
      {
        Declaration = new XDeclaration("1.0", xmlEncoding.BodyName, "yes")
      };

      document.Add(self);

      using (XmlTextWriter writer = new XmlTextWriter(targetStream, xmlEncoding))
      {
        writer.Indentation = DefaultXmlIndentation;
        writer.Formatting = Formatting.Indented;
        document.Save(writer);
        writer.Flush();
      }

      AfterSerialized();
    }

    private static void ExportAttribute(XElement target, IZenonSerializable source, PropertyInfo property, zenonSerializableBaseAttribute attributeAttribute)
    {
      if (attributeAttribute == null)
      {
        return;
      }

      if (property.GetGetMethod(true) == null)
      {
        // No getter, nothing to do
        return;
      }

      object sourceValue = property.GetValue(source);
      if (sourceValue == null)
      {
        // Omit the whole node, since we have nothing to serialize then.
        return;
      }

      // Check if there is an converter for this property
      if (attributeAttribute.InternalConverter != null)
      {
        IZenonSerializationConverter converterInstance = GetConverter(attributeAttribute.InternalConverter);
        target.SetAttributeValue(attributeAttribute.InternalName, converterInstance.Convert(sourceValue));
      }
      else
      {
        var valueType = sourceValue.GetType();
        string stringValue = sourceValue.ToString();

        if (valueType.IsEnum)
        {
          // Try to find a zenonSerializable attribute to write the correct value
          var attribute = valueType.GetField(stringValue).GetCustomAttribute<zenonSerializableEnumAttribute>();
          if (attribute != null)
          {
            // Set the value from the attribute, otherwise use the default string value (after the outer if-clause)
            target.SetAttributeValue(attributeAttribute.InternalName, attribute.Name);
            return;
          }
        }

        target.SetAttributeValue(attributeAttribute.InternalName, stringValue);
      }
    }


    private static void ExportNode(XElement target, IZenonSerializable source, PropertyInfo property, zenonSerializableBaseAttribute nodeAttribute)
    {
      if (property.GetGetMethod(true) == null)
      {
        // No getter, nothing to do
        return;
      }

      if (nodeAttribute == null)
      {
        return;
      }

      object sourceValue = property.GetValue(source);
      if (nodeAttribute.InternalOmitIfNull && sourceValue == null)
      {
        // Omit the whole node
        return;
      }

      bool isEnumerable = property.IsEnumerableOf(typeof(IZenonSerializable));
      bool isZenonSerializable = property.CanBeAssignedTo<IZenonSerializable>();

      if (isZenonSerializable)
      {
        MethodInfo exportMethod = property.PropertyType.GetMethod(nameof(ExportAsXElement), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
        // ReSharper disable once PossibleNullReferenceException : Cannot be null, since provided by the interface.
        XElement child = (XElement)exportMethod.Invoke(sourceValue, null);
        child.Name = TryResolveName(property, nodeAttribute, property.PropertyType, sourceValue, 0, nodeAttribute.InternalName);

        target.Add(child);
      }
      else if (isEnumerable)
      {
        IEnumerable list = (IEnumerable)sourceValue;
        if (list == null)
        {
          return;
        }

        // Currently we only support a list of IZenonSerializable and collections deriving from it
        // ReSharper disable once PossibleNullReferenceException : x will never be null.
        Type baseInterface = property.PropertyType.GetInterfaces().FirstOrDefault(x => x.FullName.StartsWith(typeof(IEnumerable<>).FullName));
        Type genericParameterType = baseInterface?.GenericTypeArguments[0];

        if (genericParameterType == null || !typeof(IZenonSerializable).IsAssignableFrom(baseInterface.GenericTypeArguments[0]))
        {
          throw new NotImplementedException(
            String.Format(Strings.ErrorMessageInvalidSerializationListType, nameof(IZenonSerializable)));
        }

        if (nodeAttribute.InternalEncapsulateChildsIfList)
        {
          if (nodeAttribute.InternalName == null)
          {
            // ReSharper disable once PossibleNullReferenceException : property will not be null at this point.
            throw new Exception(
              $"Property {property.Name} in class {property.DeclaringType.Name} requires a {nameof(zenonSerializableNodeAttribute.NodeName)} "
              + $"if {nameof(zenonSerializableNodeAttribute.EncapsulateChildsIfList)} is set.");
          }
          XElement listWithChilds = new XElement(nodeAttribute.InternalName);

          int index = 0;
          foreach (IZenonSerializable listItem in list)
          {
            if (listItem == null)
            {
              continue;
            }

            // ReSharper disable once PossibleNullReferenceException : "genericParameterType" cannot be null at this point.
            MethodInfo exportMethod = genericParameterType.GetMethod(nameof(ExportAsXElement), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            // ReSharper disable once PossibleNullReferenceException : "exportMethod" cannot be null at this point.
            XElement child = (XElement)exportMethod.Invoke(listItem, null);
            child.Name = TryResolveName(property, nodeAttribute, listItem.GetType(), listItem, index++, child.Name.LocalName);

            listWithChilds.Add(child);
          }

          target.Add(listWithChilds);
        }
        else
        {
          int index = 0;
          foreach (IZenonSerializable listItem in list)
          {
            if (listItem == null)
            {
              continue;
            }

            // ReSharper disable once PossibleNullReferenceException : "genericParameterType" cannot be null at this point.
            MethodInfo exportMethod = genericParameterType.GetMethod(nameof(ExportAsXElement), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            // ReSharper disable once PossibleNullReferenceException : "exportMethod" cannot be null at this point.
            XElement child = (XElement)exportMethod.Invoke(listItem, null);
            child.Name = TryResolveName(property, nodeAttribute, listItem.GetType(), listItem, index++, nodeAttribute.InternalName);

            target.Add(child);
          }
        }
      }
      else
      {
        // Just write the string representation of the property as the value
        var name = TryResolveName(property, nodeAttribute, property.PropertyType, sourceValue, 0, null);
        if (string.IsNullOrWhiteSpace(name))
        {
          name = nodeAttribute.InternalName;
        }

        XElement child = new XElement(name);
        var value = sourceValue?.ToString();
        if (value != null)
        {
          child.Value = value;
        }

        target.Add(child);
      }
    }

    private static string TryResolveName(PropertyInfo property, zenonSerializableBaseAttribute attribute, Type targetType, object value, int index, string defaultValue)
    {
      IZenonSerializableResolver resolver;
      if (attribute.InternalTypeResolver == null || (resolver = GetResolver(attribute.InternalTypeResolver)) == null)
      {
        if (attribute.InternalName == null)
        {
          // ReSharper disable once PossibleNullReferenceException : property and declaring type will never be null at this point.
          throw new Exception(
            $"Neither a name, nor a {nameof(IZenonSerializableResolver)} is given for property {property.Name} in {property.DeclaringType.Name}.");
        }

        return defaultValue;
      }

      string newName = resolver.GetNodeNameForSerialization(property, targetType, value, index)?.ReplaceNonUnicodeAlphaNumerics();
      if (string.IsNullOrWhiteSpace(newName))
      {
        throw new Exception(
          $"The given {nameof(IZenonSerializableResolver)} ({resolver.GetType().Name}) did not return a valid name value for property "
          + $"{property.Name} in {property.DeclaringType.Name} (Requested type was {targetType}).");
      }

      try
      {
        XmlConvert.VerifyName(newName);
      }
      catch (Exception ex)
      {
        throw new Exception(
          $"The given {nameof(IZenonSerializableResolver)} ({resolver.GetType().Name}) did not return a valid name value for property "
          + $"{property.Name} in {property.DeclaringType.Name} (Requested type was {targetType}, returned name was {newName}).", ex);
      }

      return newName;
    }


    private static void ExportNodeContent(XElement target, IZenonSerializable source, PropertyInfo property, zenonSerializableBaseAttribute contentAttribute)
    {
      if (property.GetGetMethod(true) == null)
      {
        // No getter, nothing to do
        return;
      }

      if (contentAttribute == null)
      {
        return;
      }

      object sourceValue = property.GetValue(source);
      if (sourceValue == null)
      {
        return;
      }

      // Check if there is an converter for this property
      if (contentAttribute.InternalConverter != null)
      {
        IZenonSerializationConverter converterInstance = GetConverter(contentAttribute.InternalConverter);
        target.Add(new XText(converterInstance.Convert(sourceValue)));
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
            target.Add(new XText(attribute.Name));
            return;
          }
        }

        string stringValue = sourceValue?.ToString();
        target.Add(new XText(stringValue));
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
      if (source.UnknownAttributes == null || source.UnknownAttributes.Count == 0)
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
      if (source.UnknownNodes == null || source.UnknownNodes.Count == 0)
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

    private static void ExportUnknownContent(XElement target, IZenonSerializable source)
    {
      if (source.UnknownNodeContent == null)
      {
        return;
      }

      target.Add(new XText(source.UnknownNodeContent));
    }
    #endregion

    #region Import methods
    protected static object ImportWithoutClone(Type type, XElement source, object parent = null, object root = null)
    {
      if (!typeof(IZenonSerializable).IsAssignableFrom(type))
      {
        throw new ArgumentException($"The given type does not implement {nameof(IZenonSerializable)}.");
      }

      var result = (IZenonSerializable)Activator.CreateInstance(type, true);

      if (result is IContainerAwareCollectionItem resultWithParent)
      {
        resultWithParent.ItemContainerParent = parent;
        resultWithParent.ItemContainerRoot = root;
      }

      // Find all the attributes and properties of the current type for deserialization
      foreach (var property in type.GetRuntimeProperties())
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
      ImportUnknownContent(result, source);

      result.ObjectStatus = zenonSerializableStatusEnum.Loaded;
      result.OnDeserialized(source.Name.LocalName);

      return result;
    }

    public static TSelf Import(XElement source, object parent = null, object root = null)
    {
      // Create a deep copy: Has quite a performance impact, however
      source = new XElement(source);
      return ImportWithoutClone(typeof(TSelf), source, parent, root) as TSelf;
    }

    public static TSelf Import(string fileName, object parent = null, object root = null)
    {
      XElement source = XElement.Load(fileName);
      return ImportWithoutClone(typeof(TSelf), source, parent, root) as TSelf;
    }

    public static TSelf Import(Stream sourceStream, object parent = null, object root = null)
    {
      XElement source = XElement.Load(sourceStream);
      return ImportWithoutClone(typeof(TSelf), source, parent, root) as TSelf;
    }

    private static Type GetIEnumerableGenericTypeArgument(PropertyInfo targetListProperty)
    {
      Type baseInterface
        = targetListProperty
          .PropertyType
          .GetInterfaces()
          .FirstOrDefault(x => x.FullName.StartsWith(typeof(IEnumerable<>).FullName));

      if (baseInterface == null)
      {
        throw new Exception(
          $"Property {targetListProperty.Name} in class {targetListProperty.DeclaringType.Name} is not a type, which "
          + "implements IEnumerable<> and thus cannot be deserialized.");
      }

      Type genericParameterType = baseInterface.GenericTypeArguments[0];
      if (!typeof(IZenonSerializable).IsAssignableFrom(genericParameterType))
      {
        throw new Exception(
          $"Property {targetListProperty.Name} in class {targetListProperty.DeclaringType.Name} cannot be deserialized, since "
          + $"the used generic type ({genericParameterType.Name}) does not implement {nameof(IZenonSerializable)}.");
      }

      return genericParameterType;
    }

    private static void ImportChilds(
      PropertyInfo targetListProperty,
      IZenonSerializable parentContainer,
      List<(Type Type, XElement Element)> nodes,
      zenonSerializableNodeAttribute attribute)
    {
      if (nodes.Count == 0)
      {
        return;
      }

      Type genericParameterType = GetIEnumerableGenericTypeArgument(targetListProperty);

      IList asList = null;
      Array asArray = null;

      if (targetListProperty.PropertyType.IsArray)
      {
        asArray = Array.CreateInstance(genericParameterType, nodes.Count);
      }
      else if (targetListProperty.PropertyType.IsInterface || targetListProperty.PropertyType.IsAbstract)
      {
        var listType = typeof(List<>).MakeGenericType(genericParameterType);
        if (!targetListProperty.PropertyType.IsAssignableFrom(listType))
        {
          throw new Exception(
            $"Property {targetListProperty.Name} in class {targetListProperty.DeclaringType.Name} cannot be deserialized, since "
            + $"its type is an interface or abstract and is not assignable by an array or List<{genericParameterType.Name}>."
            + $"Use concrete types or known framework interfaces (e.g. IEnumerable<{genericParameterType}>, "
            + $"IList<{genericParameterType}>, or similar) instead.");
        }

        // List<T> is supported, create an instance which we will fill later on.
        asList = (IList)Activator.CreateInstance(listType);
      }
      else
      {
        if (targetListProperty.PropertyType.GetInterfaces().FirstOrDefault(x => x == typeof(IList)) == null)
        {
          // We cannot infer the type automatically, therefore we cannot infer a method to add an item
          string name = genericParameterType.Name;
          throw new Exception(
            $"Property {targetListProperty.Name} in class {targetListProperty.DeclaringType.Name} cannot be deserialized, since "
            + $"its type does not implement IList. You may want to use IEnumerable<{name}>, IList<{name}>, or a type deriving "
            + "from List<{name}> instead.");
        }

        // We have a concrete type, which is not an array, so we can call the default constructor.
        asList = (IList)Activator.CreateInstance(targetListProperty.PropertyType, true);
      }

      if (asList != null)
      {
        ImportChildsAsIList(parentContainer, asList, nodes);
        targetListProperty.SetValue(parentContainer, asList);
      }
      else
      {
        ImportChildsAsArray(parentContainer, asArray, nodes, attribute);
        targetListProperty.SetValue(parentContainer, asArray);
      }
    }

    private static void ImportChildsAsArray(
      IZenonSerializable parentContainer,
      Array targetArray,
      List<(Type Type, XElement Element)> nodes,
      zenonSerializableNodeAttribute attribute)
    {
      IContainerAwareCollectionItem parent = parentContainer as IContainerAwareCollectionItem;
      object root;

      if (parent != null)
      {
        root = parent.ItemContainerRoot;
        if (root == null && parent.ItemContainerParent == null)
        {
          // the current object is most likely the desired root
          root = parentContainer;
        }
      }

      // ReSharper disable once ForCanBeConvertedToForeach : for is faster than foreach.
      for (int i = 0; i < nodes.Count; i++)
      {
        var entry = nodes[i];
        MethodInfo importMethod = GetImportMethod(entry.Type);

        object child;
        int index = 0;

        // The child is not capable of handling parents and roots, therefore pass null
        // ReSharper disable once PossibleNullReferenceException : "importMethod" comes from the interface,
        // will never be null at this point.
        child = importMethod.Invoke(null, new object[] { entry.Type, entry.Element, null, null });
        targetArray.SetValue(child, index);

        entry.Element.Remove();
      }
    }

    private static void ImportChildsAsIList(
      IZenonSerializable parentContainer,
      IList targetList,
      List<(Type Type, XElement Element)> nodes)
    {
      IContainerAwareCollectionItem parent = parentContainer as IContainerAwareCollectionItem;
      object root = null;

      if (parent != null)
      {
        root = parent.ItemContainerRoot;
        if (root == null && parent.ItemContainerParent == null)
        {
          // the current object is most likely the desired root
          root = parentContainer;
        }
      }

      foreach (var entry in nodes)
      {
        MethodInfo importMethod = GetImportMethod(entry.Type);

        object child;
        // The child is not capable of handling parents and roots, therefore pass null
        // ReSharper disable once PossibleNullReferenceException : "importMethod" comes from the interface,
        // will never be null at this point.
        child = importMethod.Invoke(null, new[] { entry.Type, entry.Element, parentContainer, root });
        targetList.Add(child);

        entry.Element.Remove();
      }
    }

    // ReSharper disable once StaticMemberInGenericType : This is intended.
    private static readonly Dictionary<Type, MethodInfo> ImportMethodCache = new Dictionary<Type, MethodInfo>();

    private static MethodInfo GetImportMethod(Type targetType)
    {
      MethodInfo importMethod;
      if (ImportMethodCache.ContainsKey(targetType))
      {
        importMethod = ImportMethodCache[targetType];
      }
      else
      {
        importMethod = targetType
          .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy)
          .First(
            x => x.Name == nameof(ImportWithoutClone)
            && x.ReturnParameter.ParameterType == typeof(object)
            && x.GetParameters().Length == 4);

        ImportMethodCache[targetType] = importMethod;
      }

      return importMethod;
    }

    private static void ImportAttribute(IZenonSerializable target, XElement sourceXml, PropertyInfo property)
    {
      var zenonAttribute = property.GetCustomAttribute<zenonSerializableAttributeAttribute>();
      if (zenonAttribute == null)
      {
        return;
      }

      var xmlAttribute = sourceXml.Attributes(zenonAttribute.AttributeName).FirstOrDefault();
      if (xmlAttribute == null)
      {
        return;
      }

      // Check if there is a converter
      Type type = property.PropertyType;
      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
        // Use the concrete type, so that we are able to convert the instance.
        type = type.GetGenericArguments()[0];
      }

      if (zenonAttribute.Converter != null)
      {
        IZenonSerializationConverter converterInstance = GetConverter(zenonAttribute.Converter);
        property.SetValue(target, converterInstance.Convert(xmlAttribute.Value));
        xmlAttribute.Remove();
        return;
      }

      if (type.IsEnum)
      {
        // If no converter is registered, try to convert it manually
        foreach (var value in Enum.GetValues(type))
        {
          var field = type.GetField(value.ToString());

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
        throw new Exception($"Cannot set value \"{xmlAttribute.Value}\" for {type.Name}, either a "
          + $"{nameof(zenonSerializableEnumAttribute)} must be set for the enum fields or "
          + $"the name must exactly match the XML value.");
      }

      if (typeof(IConvertible).IsAssignableFrom(type))
      {
        var converted = Convert.ChangeType(xmlAttribute.Value, type);
        property.SetValue(target, converted);
        xmlAttribute.Remove();
        return;
      }

      throw new Exception($"Expected property '{property.Name}' in class '{property.DeclaringType.Name}' to be a "
        + $"type, which implements 'IConvertible', inherits from '{typeof(zenonSerializable<>).Name}', or has an "
        + $"'{nameof(IZenonSerializationConverter)}' in its '{nameof(zenonSerializableAttributeAttribute)}' defined.");
    }


    private static void ImportNode(IZenonSerializable target, XElement sourceXml, PropertyInfo property)
    {
      var attribute = property.GetCustomAttribute<zenonSerializableNodeAttribute>();
      if (attribute == null)
      {
        return;
      }

      IEnumerable<XElement> consideredChildren = sourceXml.Elements().OfType<XNode>().Cast<XElement>();
      IEnumerable<XElement> listElementContainers = null;

      bool isEnumerable = property.IsEnumerableOf(typeof(IZenonSerializable));
      bool isZenonSerializable = property.CanBeAssignedTo<IZenonSerializable>();

      if (isEnumerable && attribute.EncapsulateChildsIfList)
      {
        // If we have an encapsulated list, then the containing node must match the given node name and we select
        // all childs for type resolval
        listElementContainers = consideredChildren.Where(x => x.Name.LocalName == attribute.InternalName);
        consideredChildren = listElementContainers.SelectMany(x => x.Elements());
      }

      var nodes = new List<(Type Type, XElement Element)>();

      // If there is a resolver, then we use it to get the mapping type
      if (attribute.InternalTypeResolver != null)
      {
        int index = 0;
        var resolver = GetResolver(attribute.InternalTypeResolver);
        foreach (var element in consideredChildren)
        {
          try
          {
            var type = resolver.GetTypeForDeserialization(element.Name.LocalName, index++);
            if (type == null)
            {
              // Unknown type for this resolver, all fine. We ignore this value for now.
              continue;
            }

            nodes.Add((type, element));
          }
          catch
          {
            // Same as above, just continue.
          }
        }
      }
      else
      {
        // No multiple types are allowed without a resolver, we expect the childs to properly match here.
        if (isEnumerable && !isZenonSerializable)
        {
          // This is a list, array or similar. Let's find the generic type parameter, so that we can store the correct types.
          var genericTypeProperty = GetIEnumerableGenericTypeArgument(property);

          if (listElementContainers != null)
          {
            // There is a parent element, which may have a specific name.
            // For the children, we need to assume that they are all of the correct type, independent of the node name.
            nodes.AddRange(consideredChildren.Select(x => (genericTypeProperty, x)));
          }
          else
          {
            nodes.AddRange(consideredChildren.Where(x => x.Name.LocalName == attribute.InternalName).Select(x => (genericTypeProperty, x)));
          }
        }
        else
        {
          nodes.AddRange(sourceXml.Elements().Where(x => x.Name.LocalName == attribute.NodeName).OfType<XNode>().Cast<XElement>().Select(x => (property.PropertyType, x)));
        }
      }

      if (typeof(IZenonSerializable).IsAssignableFrom(property.PropertyType))
      {
        if (nodes.Count > 1)
        {
          throw new Exception(
            $"Multiple candidates were found in the source XML for property {property.Name} on {property.DeclaringType}. "
            + $"Consider using no aliases via a {nameof(IZenonSerializableResolver)} to avoid conflicts, "
            + $"or change the property to be an IList<{property.PropertyType.Name}>.");
        }

        if (nodes.Count == 0)
        {
          return;
        }

        var node = nodes[0];
        MethodInfo importMethod = GetImportMethod(typeof(TSelf));
        object root = (target as IContainerAwareCollectionItem)?.ItemContainerRoot;

        // ReSharper disable once PossibleNullReferenceException : Method reference can never be null, since coming from the interface.
        var child = (IZenonSerializable)importMethod.Invoke(null, new[] { node.Type, node.Element, target, root ?? target });
        property.SetValue(target, child);

        // Remove the successfully deserialized value from the source node.
        node.Element.Remove();
      }
      else if (isEnumerable && !isZenonSerializable)
      {
        // Create the list which will hold the instances
        ImportChilds(property, target, nodes, attribute);

        if (listElementContainers != null)
        {
          // We need to remove the top level elements for encapsulated lists here too
          listElementContainers.Remove();
        }
      }
      else
      {
        if (nodes.Count > 1)
        {
          throw new Exception(
            $"Multiple candidates were found in the source XML for property {property.Name} in class {property.DeclaringType}. "
            + $"Consider using no aliases via a {nameof(IZenonSerializableResolver)} to avoid conflicts, "
            + $"or change the property to be an IList<{property.PropertyType.Name}>.");
        }

        if (nodes.Count == 0)
        {
          return;
        }

        Type type = property.PropertyType;
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
          // Use the concrete type, so that we are able to convert the instance.
          type = type.GetGenericArguments()[0];
        }

        // Just try to deserialize the value directly
        var node = nodes[0].Element;
        if (type.IsEnum)
        {
          foreach (var value in Enum.GetValues(type))
          {
            var field = type.GetField(value.ToString());

            // Try to match the enum value either by the attribute or the string name
            var enumAttribute = field.GetCustomAttribute<zenonSerializableEnumAttribute>();
            if ((enumAttribute != null && node.Value == enumAttribute.Name) || node.Value == field.Name)
            {
              property.SetValue(target, value);

              // Remove the attribute, so that we can check later on for unhandled ones
              node.Remove();
              return;
            }
          }

          // If neither the attribute nor the string name matches, something went wrong
          throw new Exception($"Cannot set value \"{node.Value}\" for {type.Name}, either a "
                              + $"{nameof(zenonSerializableEnumAttribute)} must be set for the enum fields or "
                              + "the name must exactly match the XML value.");
        }

        if (typeof(IConvertible).IsAssignableFrom(type))
        {
          var value = Convert.ChangeType(node.Value, type);
          property.SetValue(target, value);

          // Remove the successfully deserialized value from the source node.
          node.Remove();
        }
        else
        {
          throw new Exception($"Expected property '{property.Name}' in class '{property.DeclaringType.Name}' to be a "
            + $"type, which implements 'IConvertible', inherits from '{typeof(zenonSerializable<>).Name}', or has an "
            + $"'{nameof(IZenonSerializationConverter)}' in its '{nameof(zenonSerializableNodeAttribute)}' defined.");
        }
      }
    }


    private static void ImportNodeContent(IZenonSerializable target, XElement sourceXml, PropertyInfo property)
    {
      var zenonAttribute = property.GetCustomAttribute<zenonSerializableNodeContentAttribute>();
      if (zenonAttribute == null || string.IsNullOrEmpty(sourceXml.Value))
      {
        return;
      }

      // Check if there is a converter
      if (zenonAttribute.Converter != null)
      {
        IZenonSerializationConverter converterInstance = GetConverter(zenonAttribute.Converter);
        property.SetValue(target, converterInstance.Convert(sourceXml.Value));
        sourceXml.Value = "";
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
            sourceXml.Value = "";
            return;
          }
        }

        // If neither the attribute nor the string name matches, something went wrong
        throw new Exception($"Cannot set value \"{sourceXml.Value}\" for {property.PropertyType.Name}, either a "
          + $"{nameof(zenonSerializableEnumAttribute)} must be set for the enum fields or "
          + "the name must exactly match the XML value.");
      }
      else if (typeof(IConvertible).IsAssignableFrom(property.PropertyType))
      {
        var converted = Convert.ChangeType(sourceXml.Value, property.PropertyType);
        property.SetValue(target, converted);
        sourceXml.Value = "";
      }
      else
      {
        // ReSharper disable once PossibleNullReferenceException : 'property' is never null at this point.
        throw new Exception($"Expected property '{property.Name}' in class '{property.DeclaringType.Name}' to be of type string. "
          + $"Either specify a '{nameof(IZenonSerializationConverter)}' in the '{nameof(zenonSerializableNodeContentAttribute)}', or "
          + "change the property to be a string.");
      }
    }


    private static void ImportRaw(IZenonSerializable target, XElement sourceXml, PropertyInfo property)
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
        var xmlNodes = sourceXml.Elements().Where(x => x.Name.LocalName == prop.NodeName).OfType<XNode>().Cast<XElement>().ToList();
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


    private static void ImportUnknownAttributes(IZenonSerializable target, XElement sourceXml)
    {
      // All attributes which were not yet removed are yet unhandled
      foreach (var attribute in sourceXml.Attributes())
      {
        string name = attribute.Name.ToString();
        target.UnknownAttributes.Add(name, attribute.Value);
        Debug.WriteLine($"zenonSerializable - Import Warning: Unknown attribute \"{name}\" found for XML node \"{target.NodeName}\".");
      }
    }

    private static void ImportUnknownNodes(IZenonSerializable target, XElement sourceXml)
    {
      // All nodes which were not yet removed are yet unhandled
      foreach (var node in sourceXml.Elements())
      {
        List<XElement> list;
        string nodeName = node.Name.LocalName;
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
        list.Remove();
        Debug.WriteLine($"zenonSerializable - Import Warning: Unknown sub-node \"{nodeName}\" found in node \"{target.NodeName}\".");
      }
    }

    private static void ImportUnknownContent(IZenonSerializable target, XElement sourceXml)
    {
      if (string.IsNullOrEmpty(sourceXml.Value))
      {
        return;
      }

      target.UnknownNodeContent = sourceXml.Value;
      Debug.WriteLine($"zenonSerializable - Import Warning: Unknown content \"{target.UnknownNodeContent}\" found in node \"{target.NodeName}\".");
    }
    #endregion
  }
}
