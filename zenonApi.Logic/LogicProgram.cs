using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using zenonApi.Serialization;
using zenonApi.Logic.Internal;

namespace zenonApi.Logic
{
  public class LogicProgram : zenonSerializable<LogicProgram, ILogicFileContainer, LogicProject>
  {
    private LogicProgram() { }

    internal LogicProgram(LogicFolder parent)
    {
      this.Parent = parent;
      this.Root = parent.Root;
    }

    protected override string NodeName => "Program";

    [zenonSerializableAttribute("Name")]
    public string Name { get; set; }

    #region Properties, which are serialized/deserialized in external class _Pou
    public LogicProgramType Kind
    {
      get => (LogicProgramType)getPouProperty();
      set => setPouProperty(value);
    }

    public LogicProgramLanguage Language
    {
      get => (LogicProgramLanguage)getPouProperty();
      set => setPouProperty(value);
    }

    public uint Period
    {
      get => (uint)getPouProperty();
      set => setPouProperty(value);
    }

    public string SourceCode
    {
      get => (string)getPouProperty();
      set => setPouProperty(value);
    }

    private object getPouProperty([CallerMemberName]string propertyName = null)
    {
      var pou = this.Root.Programs.ProgramOrganizationUnits.Where(x => x.Name == this.Name).FirstOrDefault();
      if (pou == null)
      {
        throw new Exception($"Cannot find the {nameof(_Pou)} named \"{this.Name}\".");
      }

      var property = typeof(_Pou).GetRuntimeProperties().Where(x => x.Name == propertyName).FirstOrDefault();
      if (property == null)
      {
        throw new Exception($"{nameof(_Pou)} does not have any properties named \"{propertyName}\"");
      }

      return property.GetValue(pou);
    }

    private void setPouProperty(object value, [CallerMemberName]string propertyName = null)
    {
      var pou = this.Root.Programs.ProgramOrganizationUnits.Where(x => x.Name == this.Name).FirstOrDefault();
      if (pou == null)
      {
        throw new Exception($"Cannot find the {nameof(_Pou)} named \"{this.Name}\".");
      }

      var property = typeof(_Pou).GetRuntimeProperties().Where(x => x.Name == propertyName).FirstOrDefault();
      if (property == null)
      {
        throw new Exception($"{nameof(_Pou)} does not have any properties named \"{propertyName}\"");
      }

      property.SetValue(pou, value);
    }
    #endregion
  }
}
