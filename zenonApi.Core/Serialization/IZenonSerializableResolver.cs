using System;
using System.Collections.Generic;
using System.Text;

namespace zenonApi.Serialization
{
  public interface IZenonSerializableResolver
  {
    string GetNodeNameForTargetType(Type entityType);

    Type GetTypeForNodeName(string nodeName);
  }
}
