using System;
using System.IO;
using System.Xml.Linq;
using zenonApi.Logic.Ini;
using zenonApi.Zenon.Helper;
using zenonApi.Zenon.K5Prp;

// ReSharper disable once CheckNamespace : Intended, to fit into the object hierarchy of the logic API.
namespace zenonApi.Logic
{
  public class LazyLogicProject : Lazy<LogicProject>
  {
    private string _projectNameBeforeLoad;

    private string _pathBeforeLoad;

    private K5DbxsIniFile _k5DbxsIniFile;


    /// <summary>
    ///   Specifies the Name of the zenon Logic project.
    /// </summary>
    public string ProjectName
    {
      get {
        if (this.IsValueCreated)
        {
          return this.Value.ProjectName;
        }

        return _projectNameBeforeLoad;
      }
      private set
      {
        if (this.IsValueCreated)
        {
          this.Value.ProjectName = value;
          return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
          value = "Unnamed";
        }

        if (value.Length > 15)
        {
          value = value.Substring(0, 15);
        }

        string currentProjectPath = this.Path?.TrimEnd(System.IO.Path.DirectorySeparatorChar) ?? string.Empty;
        string[] splitResult = currentProjectPath.Split(System.IO.Path.DirectorySeparatorChar);

        splitResult[splitResult.Length - 1] = value;
        this.Path = string.Join(System.IO.Path.DirectorySeparatorChar.ToString(), splitResult);

        _projectNameBeforeLoad = value;
      }
    }

    /// <summary>
    ///   The pathname of the K5 project's folder.
    /// </summary>
    public string Path
    {
      get
      {
        if (IsValueCreated)
        {
          return Value.Path;
        }

        return _pathBeforeLoad;
      }
      // The setter is only invoked on creation of the LazyLogicProject and until .Value was called.
      private set => _pathBeforeLoad = value;
    }

    /// <summary>
    ///   MainPort which is used by the current zenon Logic project for communication with zenon.
    ///   The MainPort configuration has to be unique for each zenon Logic project within a
    ///   zenon project.
    /// </summary>
    public uint MainPort => K5DbxsIniFile?.MainPort ?? 0;

    /// <summary>
    ///   Object for read and write access to the K5dbxs.ini file of the current zenon Logic project.
    /// </summary>
    private K5DbxsIniFile K5DbxsIniFile
    {
      get
      {
        if (IsValueCreated)
        {
          return Value.K5DbxsIniFile;
        }

        if (_k5DbxsIniFile != null)
        {
          return _k5DbxsIniFile;
        }

        if (File.Exists(K5DbxsIniFilePath))
        {
          _k5DbxsIniFile = new K5DbxsIniFile(K5DbxsIniFilePath, base.Value.MainPort);
        }

        return _k5DbxsIniFile;
      }
    }

    /// <summary>
    ///   Directory of ini file which stores the integration settings between zenon and zenon Logic.
    /// </summary>
    /// <remarks>
    ///   This property is not in use for Straton only usage.
    /// </remarks>
    public string K5DbxsIniFilePath
    {
      get
      {
        if (IsValueCreated)
        {
          return Value.K5DbxsIniFilePath;
        }

        if (!string.IsNullOrWhiteSpace(Path))
        {
          return System.IO.Path.Combine(Path, LogicProject.K5DbxsIniFileName);
        }

        return null;
      }
    }

    /// <summary>
    ///   Creates a new instance of a lazy zenon Logic project, using an already existing project.
    /// </summary>
    /// <param name="project">The existing zenon Logic project.</param>
    public LazyLogicProject(LogicProject project) : base(() => project)
    {
      if (project == null)
      {
        throw new ArgumentNullException(nameof(project));
      }

      ProjectName = project.ProjectName;
      if (!string.IsNullOrWhiteSpace(project.Path))
      {
        Path = System.IO.Path.GetDirectoryName(project.Path);
      }
    }

    /// <summary>
    ///   The root of a K5 project structure, but lazy loaded.
    ///   Use the Value property to retrieve the <see cref="LogicProject"/> instance or load it.
    /// </summary>
    public LazyLogicProject(string name, string logicPath) : base(() =>
    {
      K5ToolSet k5ToolSet = new K5ToolSet(logicPath);
      string randomXmlFilePath = TemporaryFileCreator.GetRandomTemporaryFilePathWithExtension("xml");
      k5ToolSet.ExportZenonLogicProjectAsXml(randomXmlFilePath);

      XElement logicProjectXmlExport = XElement.Load(randomXmlFilePath);

      // Initialization of the zenon logic project data model
      LogicProject logicProject = LogicProject.Import(logicProjectXmlExport);

      File.Delete(randomXmlFilePath);

      return logicProject;
    })
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new ArgumentNullException(nameof(name));
      }

      if (string.IsNullOrWhiteSpace(logicPath))
      {
        throw new ArgumentNullException(nameof(logicPath));
      }

      ProjectName = name;
      Path = logicPath;
    }

    public static implicit operator LazyLogicProject(LogicProject project) => new LazyLogicProject(project);
  }
}
