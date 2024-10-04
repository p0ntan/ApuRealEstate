// Created by Pontus Åkerberg 2024-09-10

using System.Runtime.Serialization;

namespace RealEstateBLL.Estates;

/// <summary>
/// Abstract class Apartment is one type of a Residential estate, used as base for Rental and Tenement.
/// </summary>
public abstract class Apartment : Residential
{
    [DataMember]
    public int OnFloor { get; set; }

    [DataMember]
    public int MonthlyCost { get; set; }

    public Apartment()
    { }

    public override string[] GetSpecificInfo()
    {
        string[] data = [this.OnFloor.ToString(), this.MonthlyCost.ToString()];
        return data;
    }

    public override string[] GetSpecificLabels()
    {
        string[] data = ["On Floor", "Monthly Cost"];
        return data;
    }
}
