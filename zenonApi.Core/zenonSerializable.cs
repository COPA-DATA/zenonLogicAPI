using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using System.Collections;

namespace zenonApi.Core
{
  public abstract class zenonSerializable<TSelf, TParent> : IZenonSerializable<TParent>
    where TSelf : zenonSerializable<TSelf, TParent>
    where TParent : class
  {
    protected abstract string NodeName { get; }
    public abstract TParent Parent { get; protected set; }

    static Dictionary<Type, IZenonSerializationConverter> converterCache = new Dictionary<Type, IZenonSerializationConverter>();

    public virtual XElement Export()
    {
      // Create a node for the current element, check for all properties with a zenonSerializableAttribute-
      // or zenonSerializableNode-Attribute and append them
      XElement current = new XElement(this.NodeName);

      TSelf self = (TSelf)this;
      foreach (var property in this.GetType().GetRuntimeProperties())
      {
        exportAttribute(current, self, property);
        exportNode(current, self, property);
      }

      return current;
    }

    private static void exportAttribute(XElement target, TSelf source, PropertyInfo property)
    {
      var xmlAttribute = property.GetCustomAttribute<zenonSerializableAttributeAttribute>();
      if (xmlAttribute != null)
      {
        // Check if there is an converter for this property
        if (xmlAttribute.Converter != null)
        {
          IZenonSerializationConverter converterInstance = getConverter(xmlAttribute.Converter);
          // Ensure to call the correct method overload of the converter by using the (object)-cast
          target.SetAttributeValue(xmlAttribute.AttributeName, converterInstance.Convert((object)property.GetValue(source)));
        }
        else
        {
          string stringValue = property.GetValue(source)?.ToString();
          if (stringValue != null)
          {
            // If the value is null, we omit the attribute
            target.SetAttributeValue(xmlAttribute.AttributeName, stringValue);
          }
        }
      }
    }

    private static void exportNode(XElement target, TSelf source, PropertyInfo property)
    {
      var xmlNode = property.GetCustomAttribute<zenonSerializableNodeAttribute>();
      if (xmlNode != null)
      {
        if (typeof(IZenonSerializable).IsAssignableFrom(property.PropertyType))
        {
          MethodInfo exportMethod = property.PropertyType.GetMethod(nameof(Export), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
          XElement child = (XElement)exportMethod.Invoke(property.GetValue(source), null);

          target.Add(child);
        }
        else if (typeof(IList).IsAssignableFrom(property.PropertyType))
        {
          IList list = (IList)property.GetValue(source);
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
          // TODO, non zenonSerializableTypes
        }
      }
    }


    public static TSelf Import(XElement source, TParent parent = null)
    {
      // Create an instance of the current type
      var result = (TSelf)Activator.CreateInstance(typeof(TSelf), true);
      result.Parent = parent;
      Type t = typeof(TSelf);

      // The source element must match the node name
      if (source.Name != result.NodeName)
      {
        throw new Exception($"Expected {result.NodeName}, but got {source.Name}");
      }

      // Find all the attributes and properties of the current type for deserialization
      foreach (var property in t.GetRuntimeProperties())
      {
        importAttribute(result, source, property);
        importNode(result, source, property);
      }

      return result;
    }

    private static IZenonSerializationConverter getConverter(Type converterType)
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
        else
        {
          // If no converter is registered, try to convert it manually
          // TODO: Special types like guids would be nice to consider here
          if (typeof(IConvertible).IsAssignableFrom(property.PropertyType))
          {
            var converted = Convert.ChangeType(xmlAttribute.Value, property.PropertyType);
            property.SetValue(target, converted);
          }
          else
          {
            throw new Exception($"Cannot convert types without a converter, which do not implement {nameof(IConvertible)}.");
          }
        }
      }
    }


    private static void importNode(TSelf target, XElement sourceXml, PropertyInfo property)
    {
      var prop = property.GetCustomAttribute<zenonSerializableNodeAttribute>();
      if (prop != null)
      {
        var xmlNodes = sourceXml.Elements(prop.PropertyName).OfType<XNode>().ToList();

        // Currently we only support deserializing to a concrete List types, not IEnumerable or similar, maybe in the future
        if (typeof(IList).IsAssignableFrom(property.PropertyType))
        {
          // Create the list which will hold the instances
          IList list = (IList)Activator.CreateInstance(property.PropertyType, true);

          // Get the generic type, so that we can instantiate entries for the list (this should be a zenonSerializable)
          Type genericParameter = property.PropertyType.GetGenericArguments().First();
          if (typeof(IZenonSerializable).IsAssignableFrom(genericParameter))
          {
            foreach (var xmlNode in xmlNodes)
            {
              MethodInfo importMethod = genericParameter.GetMethod(nameof(Import), BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
              var child = importMethod.Invoke(null, new object[] { xmlNode, target });

              list.Add(child);
            }
          }

          property.SetValue(target, list);
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
            var child = importMethod.Invoke(null, new object[] { xmlNodes.First(), target });

            property.SetValue(target, child);
          }
        }
      }
    }
  }
}
