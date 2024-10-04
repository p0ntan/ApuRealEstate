// Created by Pontus Åkerberg 2024-09-10

using System.Runtime.Serialization;

namespace RealEstateBLL.Estates;

/// <summary>
/// Factory is one type of a Commercial estate.
/// </summary>
public class Factory : Commercial
{
    [DataMember]
    public int ProductionCapacity { get; set; }

    [DataMember]
    public int NumberOfEmployees { get; set; }

    public Factory()
    { }

    public override int GetSpecficTypeIndex()
    {
        return (int)CommercialType.Factory;
    }

    public override string[] GetSpecificInfo()
    {
        string[] data = [this.ProductionCapacity.ToString(), this.NumberOfEmployees.ToString()];
        return data;
    }

    public override string[] GetSpecificLabels()
    {
        string[] data = ["Production Capacity", "No. of Employees"];
        return data;
    }
}
