﻿using System;
using System.Collections.Generic;
using System.Text;

namespace zenonApi.Core
{
  [AttributeUsage(AttributeTargets.Field)]
  public class zenonSerializableEnumAttribute : Attribute
  {
    public zenonSerializableEnumAttribute(string name)
    {
      Name = name;
    }

    public string Name { get; protected set; }
  }
}
