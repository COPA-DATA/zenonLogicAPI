using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using zenonApi.Collections;
using zenonApi.Logic.Resources;
using zenonApi.Logic.SerializationConverters;
using zenonApi.Serialization;

namespace zenonApi.Logic
{

  [DebuggerDisplay("{" + nameof(Name) + "}")]
  public class LogicFolder : zenonSerializable<LogicFolder, ILogicFileContainer, LogicProject>, ILogicFileContainer
  {
    /// <summary>Private default constructor for serialization.</summary>
    // ReSharper disable once UnusedMember.Local : Required default constructor for serialization.
    public LogicFolder()
    {
      Programs = new LogicProgramCollection(this, null);
      Folders = new ContainerAwareObservableCollection<LogicFolder>(this);
    }

    public LogicFolder(string folderName)
    {
      if (string.IsNullOrEmpty(folderName))
      {
        throw new ArgumentNullException(
          string.Format(Strings.ErrorMessageParameterIsNullOrWhitespace, nameof(LogicFolder), nameof(folderName)));
      }

      Name = folderName;
      Programs = new LogicProgramCollection(this, null);
      Folders = new ContainerAwareObservableCollection<LogicFolder>(this);
    }

    #region zenonSerializable implementation
    public override string NodeName => "Folder";
    #endregion

    public static LogicFolder Create(string folderName, bool isFolderExpanded = false)
    {
      if (string.IsNullOrEmpty(folderName))
      {
        throw new ArgumentNullException(
          string.Format(Strings.GeneralMethodArgumentNullException, nameof(Create), nameof(folderName)));
      }

      LogicFolder logicFolder = new LogicFolder
      {
        Name = folderName,
        Expand = isFolderExpanded
      };

      return logicFolder;
    }
    #region Specific properties
    [zenonSerializableNode("Folder")]
    public ContainerAwareObservableCollection<LogicFolder> Folders { get; protected set; }

    [zenonSerializableNode("Program")]
    public LogicProgramCollection Programs { get; protected set; }

    [zenonSerializableAttribute("Expand", Converter = typeof(YesNoConverter))]
    protected bool Expand { get; set; }

    [zenonSerializableAttribute("Name")]
    public string Name { get; set; } // TODO: What is allowed here as a name?
    #endregion

    public void Add(LogicFolder folder) => Folders.Add(folder);

    public void Add(LogicProgram program) => Programs.Add(program);

    public override LogicProject Root
    {
      get
      {
        if (base.Root != Parent?.Root)
        {
          base.Root = Parent?.Root;
        }

        return base.Root;
      }
      protected set => base.Root = value;
    }
  }

  #region extension methods for folder management

  [Browsable(false)]
  public static class LogicFolderExtensions
  {
    public static LogicFolder GetByName(this IEnumerable<LogicFolder> self, string folderName,
      StringComparison comparison = StringComparison.Ordinal)
    {
      if (string.IsNullOrEmpty(folderName))
      {
        return null;
      }

      return self.FirstOrDefault(logicFolder => logicFolder?.Name.Equals(folderName, comparison) ?? false);
    }

    public static bool Contains(this IEnumerable<LogicFolder> self, string folderName,
      StringComparison comparison = StringComparison.Ordinal)
    {
      return self.GetByName(folderName, comparison) != null;
    }
  }
  #endregion
}
