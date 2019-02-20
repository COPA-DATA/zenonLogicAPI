using System;
using System.Collections.Generic;
using System.Text;

namespace zenonApi.Core
{
  [AttributeUsage(AttributeTargets.Property)]
  public class zenonSerializableNodeContentAttribute : Attribute
  {
    public zenonSerializableNodeContentAttribute() { }
  }
}
