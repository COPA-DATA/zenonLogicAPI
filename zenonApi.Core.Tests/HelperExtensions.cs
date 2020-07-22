using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace zenonApi.Core.Tests
{
  public static class HelperExtensions
  {
    private const BindingFlags InstanceFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private const BindingFlags PropertyFlags = InstanceFlags | BindingFlags.GetProperty;
    private const BindingFlags FieldFlags = InstanceFlags | BindingFlags.GetField;


    public static bool DeepEquals(this object self, object other, params string[] ignoredPropertiesOrFields)
    {
      if (ReferenceEquals(self, other))
      {
        // No question, those are the same.
        return true;
      }

      if (self == null || other == null)
      {
        // One of the entries is null, cannot be the same.
        return false;
      }

      var ownType = self.GetType();
      if (ownType.IsValueType || ownType == typeof(string))
      {
        // Value types are already compared by their field values with Equals():
        return self.Equals(other);
      }


      // Check for a custom Equals override, then we can call it on our own:
      var otherType = other.GetType();

      var ownTypeInfo = ownType.GetTypeInfo();
      var otherTypeInfo = otherType.GetTypeInfo();

      if (ownType != typeof(object) && ownTypeInfo.GetMethods().FirstOrDefault(x => x.Name == "Equals" && x.DeclaringType != typeof(object)) != null)
      {
        // The Equals() method is overridden, use it:
        return self.Equals(other);
      }

      if (ownType != otherType)
      {
        // If there is no custom equals, then the comparison will always fail if the types are different.
        return false;
      }

      // All Arrays, Lists, Dictionaries, IEnumerable<>, (even strings) implement IEnumerable:
      var selfEnumerable = self as IEnumerable;
      var otherEnumerable = other as IEnumerable;

      if (selfEnumerable != null || otherEnumerable != null)
      {
        if (selfEnumerable == null || otherEnumerable == null)
        {
          // If only one of them is enumerable, then they should not be comparable at all.
          return false;
        }

        var otherEnumerator = otherEnumerable.GetEnumerator();
        otherEnumerator.Reset();

        foreach (var selfItem in selfEnumerable)
        {
          if (!otherEnumerator.MoveNext())
          {
            // Different amount of items
            return false;
          }

          // Compare the current item with this same method
          if (!DeepEquals(selfItem, otherEnumerator.Current, ignoredPropertiesOrFields))
          {
            return false;
          }
        }

        // if otherEnumerator can still proceed, than the amount of items is unequal again
        if (otherEnumerator.MoveNext())
        {
          return false;
        }
      }
      else
      {
        // Compare each and every property
        foreach (var ownPropertyInfo in ownTypeInfo.GetProperties(PropertyFlags))
        {
          if (ignoredPropertiesOrFields.Any(x => x == ownPropertyInfo.Name))
          {
            continue;
          }

          var otherPropertyInfo = otherTypeInfo.GetProperty(ownPropertyInfo.Name, PropertyFlags);
          if (otherPropertyInfo == null)
          {
            // The other object does not have the same property, cannot be the same.
            return false;
          }

          if (ownPropertyInfo.GetMethod == null && otherPropertyInfo.GetMethod == null)
          {
            // Both have no getter, may be the same at this point.
            continue;
          }

          if (ownPropertyInfo.GetMethod == null || otherPropertyInfo.GetMethod == null)
          {
            // Just one of the properties has no getter, cannot be the same.
            return false;
          }

          if (!DeepEquals(ownPropertyInfo.GetValue(self, null), otherPropertyInfo.GetValue(other, null), ignoredPropertiesOrFields))
          {
            return false;
          }
        }

        // Same for fields
        foreach (var ownFieldInfo in ownTypeInfo.GetFields(FieldFlags))
        {
          // Ignore compiler generated backing fields:
          if (ownFieldInfo.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
          {
            continue;
          }

          if (ignoredPropertiesOrFields.Any(x => x == ownFieldInfo.Name))
          {
            continue;
          }

          var otherFieldInfo = otherTypeInfo.GetField(ownFieldInfo.Name, FieldFlags);
          if (otherFieldInfo == null)
          {
            return false;
          }

          if (!DeepEquals(ownFieldInfo.GetValue(self), otherFieldInfo.GetValue(other), ignoredPropertiesOrFields))
          {
            return false;
          }
        }
      }

      return true;
    }
  }
}