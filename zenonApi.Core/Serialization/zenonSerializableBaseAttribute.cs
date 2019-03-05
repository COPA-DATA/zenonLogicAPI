using System;
using System.Collections.Generic;
using System.Text;

namespace zenonApi.Serialization
{
  public abstract class zenonSerializableBaseAttribute : Attribute
  {
    internal abstract zenonSerializableAttributeType AttributeType { get; }
    internal abstract byte InternalOrder { get; }
    internal abstract string InternalName { get; }
    internal abstract bool InternalOmitIfNull { get; }
    internal abstract Type InternalConverter { get; }
  }
}
