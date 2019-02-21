using System;
using System.Collections.Generic;
using System.Text;

namespace zenonApi.Serialization
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class zenonSerializableNodeContentAttribute : Attribute
  {
    public zenonSerializableNodeContentAttribute() { }

    public Type Converter { get; set; }
  }
}
