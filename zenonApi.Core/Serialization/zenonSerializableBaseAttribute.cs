﻿using System;

namespace zenonApi.Serialization
{
  // ReSharper disable once InconsistentNaming : "zenon" is always written lowercase.
  public abstract class zenonSerializableBaseAttribute : Attribute
  {
    internal abstract zenonSerializableAttributeType AttributeType { get; }
    internal abstract byte InternalOrder { get; }
    internal abstract string InternalName { get; }
    internal abstract bool InternalEncapsulateChildsIfList { get; }
    internal abstract bool InternalOmitIfNull { get; }
    internal abstract Type InternalConverter { get; }
    internal abstract Type InternalTypeResolver { get; }
  }
}
