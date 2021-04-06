﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;
using zenonApi.Collections;
using zenonApi.Logic.Ini;
using zenonApi.Logic.Internal;
using zenonApi.Logic.Network;
using zenonApi.Logic.Resources;
using zenonApi.Serialization;

namespace zenonApi.Logic
{
  /// <summary>
  /// The root of a K5 project structure.
  /// </summary>
  [DebuggerDisplay("{" + nameof(ProjectName) + "}")]
  public class LogicProject : zenonSerializable<LogicProject, IZenonSerializable, LogicProject>
  {
    public const string K5DbxsIniFileName = "K5DBXS.INI";
    private const string K5ProjectRootNodeName = "K5project";

    private uint? _mainPort;

    private LogicProject()
    {
      // Initialize members which require the current object in their ctor-parameters:
      this.Settings = new LogicProjectSettings(this);
      this.DataTypes = new ExtendedObservableCollection<LogicDataType>();

      PopulateBasicLogicDataTypes();

      this.LogicDefinitions = new LogicDefinitions(this);
      this.GlobalVariables = new LogicGlobalVariables(this);
      this.Networks = new LogicNetwork(this);
      this.Programs = new _LogicProgramsCollection(this);
      this.ApplicationTree = new ApplicationTree(this);
    }

    public LogicProject(string name) : this()
    {
      this.ProjectName = name;
    }

    /// <summary>
    /// Returns instance of <see cref="LogicProject"/> with the loaded information of the k5XmlExport and the
    /// specified zenon Logic project name.
    /// </summary>
    /// <param name="k5XmlExport">K5 XML export of a zenon Logic project</param>
    /// <param name="zenonLogicProjectName">Name which should be set for the zenon Logic project</param>
    /// <param name="stratonDirectoryOfZenonProject">Directory path of the straton folder of a zenon project.
    /// Example path: "C:\ProgramData\COPA-DATA\SQL2012\"zenon project GUID"\FILES\straton"</param>
    /// <returns></returns>
    public static LogicProject Import(XDocument k5XmlExport, string stratonDirectoryOfZenonProject = null, string zenonLogicProjectName = null)
    {
      var zenonLogicProject = LogicProject.Import(k5XmlExport.Element(K5ProjectRootNodeName));
      if (zenonLogicProjectName != null)
      {
        zenonLogicProject.ProjectName = zenonLogicProjectName;
      }

      if (stratonDirectoryOfZenonProject != null)
      {
        zenonLogicProject.ModifyStratonDirectoryPartOfPath(stratonDirectoryOfZenonProject);
      }

      return zenonLogicProject;
    }

    public override void ExportAsFile(string fileName, string xmlEncoding = null)
    {
      if (xmlEncoding == null)
      {
        xmlEncoding = "iso-8859-1";
      }

      base.ExportAsFile(fileName, xmlEncoding);
    }

    public override void ExportAsFile(string fileName, Encoding xmlEncoding)
    {
      if (xmlEncoding == null)
      {
        xmlEncoding = Encoding.GetEncoding("iso-8859-1");
      }

      base.ExportAsFile(fileName, xmlEncoding);
    }

    public override string ExportAsString(Encoding xmlEncoding)
    {
      if (xmlEncoding == null)
      {
        xmlEncoding = Encoding.GetEncoding("iso-8859-1");
      }

      return base.ExportAsString(xmlEncoding);
    }

    public override void ExportAsStream(Stream targetStream, string xmlEncoding = null)
    {
      if (string.IsNullOrWhiteSpace(xmlEncoding))
      {
        xmlEncoding = "iso-8859-1";
      }

      base.ExportAsStream(targetStream, xmlEncoding);
    }

    public override void ExportAsStream(Stream targetStream, Encoding xmlEncoding = null)
    {
      if (xmlEncoding == null)
      {
        xmlEncoding = Encoding.GetEncoding("iso-8859-1");
      }

      base.ExportAsStream(targetStream, xmlEncoding);
    }

    #region zenonSerializable Implementation

    public override string NodeName => "K5project";

    #endregion

    #region zenon Logic specific properties

    /// <summary>
    /// zenon Logic project name
    /// Note that this property gets the value from the last part of the <see cref="Path"/> property.
    /// Changes made to this property will affect the according part of the <see cref="Path"/> property.
    /// </summary>
    public string ProjectName
    {
      // gets the name of the zenon Logic project from the last part of the k5Project path xml attribute
      get => string.IsNullOrEmpty(this.Path)
        ? "Unknown"
        : System.IO.Path.GetFileName(this.Path.TrimEnd(System.IO.Path.DirectorySeparatorChar));
      // writes the new name on the according position of the k5project path xml attribute
      set
      {
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
      }
    }

    /// <summary>
    /// Sets the part of the directory stored in the <see cref="Path"/> property which belongs to the
    /// zenon project´s straton folder.
    /// </summary>
    /// <param name="stratonDirectoryOfZenonProject">Example value:
    /// "C:\ProgramData\COPA-DATA\SQL2012\"zenon project GUID"\FILES\straton"
    /// </param>
    public void ModifyStratonDirectoryPartOfPath(string stratonDirectoryOfZenonProject)
    {
      if (string.IsNullOrWhiteSpace(stratonDirectoryOfZenonProject))
      {
        throw new ArgumentNullException(String.Format(Strings.ErrorMessageParameterIsNullOrWhitespace,
          nameof(ModifyStratonDirectoryPartOfPath), nameof(stratonDirectoryOfZenonProject)));
      }

      this.Path = System.IO.Path.Combine(stratonDirectoryOfZenonProject, this.ProjectName);
    }

    /// <summary>
    ///   Directory of ini file which stores the integration settings between zenon and zenon Logic.
    /// </summary>
    /// <remarks>
    ///   This property is not in use for Straton only usage.
    /// </remarks>
    public string K5DbxsIniFilePath => System.IO.Path.Combine(Path, K5DbxsIniFileName);

    private K5DbxsIniFile _k5DbxsIniFile;

    /// <summary>
    /// Object for read and write access to the K5dbxs.ini file of the current zenon Logic project.
    /// </summary>
    public K5DbxsIniFile K5DbxsIniFile
    {
      get
      {
        if (_k5DbxsIniFile != null)
        {
          return _k5DbxsIniFile;
        }

        if (File.Exists(K5DbxsIniFilePath))
        {
          _k5DbxsIniFile = new K5DbxsIniFile(K5DbxsIniFilePath, this._mainPort);
        }

        return _k5DbxsIniFile;
      }
    }

    /// <summary>
    ///   MainPort which is used by the current zenon Logic project for communication with zenon.
    ///   The MainPort configuration has to be unique for each zenon Logic project within a
    ///   zenon project.
    /// </summary>
    public uint MainPort
    {
      get
      {
        if (this.K5DbxsIniFile == null)
        {
          return this._mainPort ?? uint.MinValue;
        }
        return K5DbxsIniFile.MainPort;
      }
      set
      {
        if (this.K5DbxsIniFile != null)
        {
          K5DbxsIniFile.MainPort = value; 
        }
        this._mainPort = value;
      }
    }

    #endregion

    #region Specific properties
    [zenonSerializableNode("prjdesc", NodeOrder = 0)]
    public string ProjectDescription { get; set; }

    /// <summary>
    /// The mandatory version of the K5 project.
    /// </summary>
    [zenonSerializableAttribute("version", AttributeOrder = 0)]
    public string Version { get; protected set; } = "1.1";

    /// <summary>
    /// The pathname of the K5 project's folder.
    /// </summary>
    [zenonSerializableAttribute("path", AttributeOrder = 1)]
    public string Path { get; protected set; }

    //TODO: discuss about default constructor calls/init for this property and subproperties
    /// <summary>
    /// This tag groups all the settings of the project.
    /// </summary>
    [zenonSerializableNode("settings", NodeOrder = 2)]
    public LogicProjectSettings Settings { get; protected set; }

    //TODO: Ask StefanH about this property (not in docu)
    [zenonSerializableRawFormat("libraries", NodeOrder = 3)]
    public XElement Libraries { get; set; }

    /// <summary>
    /// The tag groups all the defined data types.
    /// </summary>
    [zenonSerializableNode("types", NodeOrder = 4, EncapsulateChildsIfList = true)]
    public ExtendedObservableCollection<LogicDataType> DataTypes { get; protected set; }

    /// <summary>
    /// This tag groups the COMMON and GOLBAL definitions.
    /// </summary>
    [zenonSerializableNode("definitions", NodeOrder = 5)]
    public LogicDefinitions LogicDefinitions { get; protected set; }

    //TODO: Ask StefanH about this property (not in docu)
    [zenonSerializableRawFormat("IOs", NodeOrder = 6)]
    public XElement Ios { get; set; }

    /// <summary>
    /// This tag groups all the global scope variable groups of the project.
    /// </summary>
    [zenonSerializableNode("variables", NodeOrder = 7)]
    public LogicGlobalVariables GlobalVariables { get; protected set; }

    /// <summary>
    /// This tag describes all network configuration.
    /// </summary>
    [zenonSerializableNode("networks", NodeOrder = 8)]
    public LogicNetwork Networks { get; protected set; }

    [zenonSerializableRawFormat("fieldbus", NodeOrder = 9)]
    public XElement FieldBus { get; set; }

    /// <summary>
    /// Lists all programs, sub-programs and UDFBs of the project.
    /// It is not intended to be manipulated by users of this API directly.
    /// </summary>
    [Browsable(false)]
    [zenonSerializableNode("programs", NodeOrder = 10)]
    internal _LogicProgramsCollection Programs { get; private set; }

    [zenonSerializableRawFormat("spylists", NodeOrder = 11)]
    public XElement SpyLists { get; set; }

    [zenonSerializableRawFormat("recipes", NodeOrder = 12)]
    public XElement Recipes { get; set; }

    [zenonSerializableRawFormat("graphics", NodeOrder = 13)]
    public XElement Graphics { get; set; }

    [zenonSerializableRawFormat("gridresources", NodeOrder = 14)]
    public XElement GridResources { get; set; }

    [zenonSerializableRawFormat("K5HMI", NodeOrder = 15)]
    public XElement HumanMachineInterface { get; set; }

    [zenonSerializableRawFormat("files", NodeOrder = 16)]
    public XElement Files { get; set; }

    /// <summary>
    /// Contains the logical folder structure of the programs and UDFBs.
    /// </summary>
    [zenonSerializableNode("Appli", NodeOrder = 17)]
    public ApplicationTree ApplicationTree { get; protected set; }

    #endregion

    private void PopulateBasicLogicDataTypes()
    {
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "BOOL" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "BYTE" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "DINT" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "DWORD" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "INT" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "LINT" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "LREAL" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "LWORD" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "REAL" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "SINT" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "STRING" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "TIME" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "UDINT" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "UINT" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "ULINT" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "USINT" });
      this.DataTypes.Add(new LogicDataType() { Kind = LogicDataTypeKind.Basic, Name = "WORD" });
    }
  }
}
