using System;
using System.Collections.Generic;
using System.Text;

namespace zenonApi.Core
{
  [AttributeUsage(AttributeTargets.Property)]
  public class zenonSerializableNodeAttribute : Attribute
  {
    public zenonSerializableNodeAttribute(string attributeName)
    {
      this.PropertyName = attributeName;
    }

    public string PropertyName { get; private set; }
  }
}
