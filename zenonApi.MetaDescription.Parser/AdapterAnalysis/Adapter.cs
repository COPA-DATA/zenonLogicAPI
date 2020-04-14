using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using zenonApi.MetaDescription.Parser.AdapterAnalysis.ApiContent;
using zenonApi.MetaDescription.Parser.AdapterAnalysis.Structure;

namespace zenonApi.MetaDescription.Parser.AdapterAnalysis
{
  public static class Adapter
  {
    public static IOrderedEnumerable<IScadaNamespace> Adapt()
    {
      List<IScadaNamespace> ScadaNamespaces = new List<IScadaNamespace>();

      Assembly assembly = Assembly.LoadFrom(Globals.FilePaths.AdapterAnalysisDllFilePath);
      Type type = assembly.GetType(Globals.AdapterAnalysis.ScadaInterfaceAnalyzer);
      MethodInfo Analyze = type.GetMethod(Globals.AdapterAnalysis.Analyze);
      PropertyInfo Namespaces = type.GetProperty(Globals.AdapterAnalysis.Namespaces);

      var interfaceAnalyzer = Activator.CreateInstance(type);

      object[] argstopass = new object[] { (string)Globals.FilePaths.TranslationXmlPath, (string)Globals.FilePaths.InteropDllFilePath };

      Analyze.Invoke(interfaceAnalyzer, argstopass);
      var unorderedNamespaces = Namespaces.GetValue(interfaceAnalyzer) as IEnumerable;

      foreach (var nspace in unorderedNamespaces)
      {
        ScadaNamespaces.Add(new ScadaNamespace(nspace));
      }

      return ScadaNamespaces.OrderBy(x => x.Name);
    }

    public static SortedDictionary<string, NamespaceStruct> OrderNamespacesIntoDict(IEnumerable<IScadaNamespace> scadaNamespaces, string assemblyName)
    {
      Assembly assembly = Assembly.LoadFile($@"{Directory.GetCurrentDirectory()}\{assemblyName}");
      int countAllElements = 0;
      Type[] types;
      try
      {
        types = assembly.GetTypes();
      }
      catch (ReflectionTypeLoadException e)
      {
        types = e.Types;
      }
      SortedDictionary<string, NamespaceStruct> _namespaceStruct = new SortedDictionary<string, NamespaceStruct>();
      foreach (var type in types)
      {
        if (type != null)
        {
          if (!_namespaceStruct.Keys.Contains(type.Namespace))
          {
            _namespaceStruct.Add(type.Namespace, new NamespaceStruct(type.Namespace, types));
          }
        }
      }
      foreach (var ns in _namespaceStruct)
      {
        countAllElements++;
        countAllElements += ns.Value.TypeDict.Count;

        ns.Value.ScadaNamespace = GetNamespaceFromOrderedNamespaces(assembly, ns.Key, scadaNamespaces);
      }

      return _namespaceStruct;
    }


    private static IScadaNamespace GetNamespaceFromOrderedNamespaces(Assembly assembly, string fullName, IEnumerable<IScadaNamespace> scadaNamespaces)
    {
      string namespaceFromAssembly = assembly.GetName().Name;
      string namespaceToLookUp = string.Empty;

      if (fullName != namespaceFromAssembly)
      {
        namespaceToLookUp = fullName.Replace($"{namespaceFromAssembly}.", "");
      }

      if (scadaNamespaces.Any(x => x.Name == namespaceToLookUp))
      {
        return scadaNamespaces.First(x => x.Name == namespaceToLookUp);
      }

      return null;
    }
  }
}
