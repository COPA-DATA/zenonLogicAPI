using System.Collections.Generic;
using System.Linq;
using zenonApi.MetaDescription.Parser.AdapterAnalysis;
using zenonApi.MetaDescription.Parser.AdapterAnalysis.Structure;

namespace zenonApi.MetaDescription.Parser.XML
{
  public static class WrappedNamespace
  {
    internal static void Parse(Definitions definitions, KeyValuePair<string, NamespaceStruct> namespaceKeyValuePair, OdlWrapperClasses.ZenonCom _zenonCom)
    {
      foreach (IScadaInterface scadaInterface in namespaceKeyValuePair.Value.ScadaNamespace.Interfaces)
      {
        if (scadaInterface.IsIgnored != null && (bool) scadaInterface.IsIgnored)
        {
          continue;
        }

        OdlWrapperClasses.ComClass comClass = null;
        if (_zenonCom.ClassDictionary.Any(x => x.Key == scadaInterface.HostName))
        {
          comClass = _zenonCom.ClassDictionary[scadaInterface.HostName];
        }

        Class classFormInterface = WrappedClass.Parse(scadaInterface, namespaceKeyValuePair.Key);
        definitions.Classes.Add(classFormInterface);

        if (comClass != null)
        {
          foreach (var dynPropertyMap in comClass.DicComDynProperties)
          {
            Property dynProperty = new Property(dynPropertyMap.Key,dynPropertyMap.Key);
            dynProperty.IsDynProperty = true;
            dynProperty.DefinitionType = dynProperty.ViewName;

            classFormInterface.HasDynProperties = true;
            classFormInterface.Properties.Add(dynProperty);
            definitions.Classes.Add(WrappedClass.Parse(dynPropertyMap, classFormInterface.ViewName));
          }
        }
      }

      foreach (IScadaEnum scadaEnum in namespaceKeyValuePair.Value.ScadaNamespace.Enumerations)
      {
        if (scadaEnum.IsIgnored != null && (bool)scadaEnum.IsIgnored)
        {
          continue;
        }
        definitions.Enums.Add(WrappedEnum.Parse(scadaEnum));
      }
    }
  }
}
