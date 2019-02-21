using System;
using System.Collections.Generic;
using System.Text;

namespace zenonApi.Serialization
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class zenonSerializableNodeAttribute : Attribute
  {
    public zenonSerializableNodeAttribute(string attributeName)
    {
      this.NodeName = attributeName;
    }

    public string NodeName { get; private set; }
    public byte NodeOrder { get; set; }
    public bool OmitIfNull { get; set; } = true;
  }
}
