using System;
using System.Collections;
using System.Collections.Generic;
using zenonApi.Serialization;

namespace zenonApi.Xml.Variable
{
  public class VariableModule : zenonSerializable<VariableModule>, IEnumerable<Variable>
  {
    public override string NodeName => "Subject";

    [zenonSerializableAttribute("ShortName")]
    private string _shortName => "zenOn(R) exported project";

    [zenonSerializableAttribute("MainVersion")]
    public string MainVersion { get; set; }

    [zenonSerializableNode("Apartment", NodeOrder = 1)]
    private VariableApartment _variableApartment { get; set; }

    [zenonSerializableNode("Apartment", NodeOrder = 2)]
    private DriverApartment _driverApartment { get; set; }

    [zenonSerializableNode("Apartment", NodeOrder = 3)]
    private DataTypeApartment _dataTypeApartment { get; set; }


    public VariableModule(string zenonMainVersion)
    {
      MainVersion = zenonMainVersion;
      _variableApartment = new VariableApartment
      {
        Version = zenonMainVersion
      };
      _driverApartment = new DriverApartment
      {
        Version = zenonMainVersion
      };
      _dataTypeApartment = new DataTypeApartment
      {
        Version = zenonMainVersion
      };
    }


    public void Add(Variable variable)
    {
      _variableApartment.Add(variable);
      _driverApartment.Add(variable.Driver);
      _dataTypeApartment.Add(variable.DataType);
    }

    public IEnumerator<Variable> GetEnumerator()
    {
      foreach (Variable variable in _variableApartment)
      {
        yield return variable;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }

  class VariableApartment : zenonSerializable<VariableApartment>, IEnumerable<Variable>
  {
    public override string NodeName => "Apartment";

    [zenonSerializableNode("NODE_NAME_HIDDEN")]
    private List<Variable> _variables { get; set; } = new List<Variable>();

    [zenonSerializableAttribute("ShortName")]
    private string ShortName => "zenOn(R) process variables list";

    [zenonSerializableAttribute("Version")]
    internal string Version { get; set; }

    internal void Add(Variable variable)
    {
      _variables.Add(variable);
    }

    public IEnumerator<Variable> GetEnumerator()
    {
      foreach (Variable variable in _variables)
      {
        yield return variable;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }

  class DriverApartment : zenonSerializable<DriverApartment>
  {
    public override string NodeName => "Apartment";

    [zenonSerializableNode("NODE_NAME_HIDDEN")]
    private List<Driver> _driver { get; set; } = new List<Driver>();

    [zenonSerializableAttribute("ShortName")]
    private string ShortName => "zenOn(R) process driver list";

    [zenonSerializableAttribute("Version")]
    internal string Version { get; set; }

    internal void Add(Driver driver)
    {
      if (!_driver.Contains(driver))
      {
        _driver.Add(driver);
      }
    }
  }

  class DataTypeApartment : zenonSerializable<DataTypeApartment>
  {
    public override string NodeName => "Apartment";

    [zenonSerializableNode("NODE_NAME_HIDDEN")]
    private List<DataType> _dataType { get; set; } = new List<DataType>();

    [zenonSerializableAttribute("ShortName")]
    private string ShortName => "zenOn(R) process driver list";

    [zenonSerializableAttribute("Version")]
    internal string Version { get; set; }

    internal void Add(DataType dataType)
    {
      if (!_dataType.Contains(dataType))
      {
        _dataType.Add(dataType);
      }
    }
  }
}


