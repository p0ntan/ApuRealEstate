// Created by Pontus Åkerberg 2024-09-10

using System.Runtime.Serialization;

namespace RealEstateBLL.Estates;

/// <summary>
/// Hospital is one type of a Institutional estate.
/// </summary>
public class Hospital : Institutional
{
    [DataMember]
    public int NumberOfBeds { get; set; }

    [DataMember]
    public int NumberOfParkingSpots { get; set; }

    public Hospital( )
    { }

    public override int GetSpecficTypeIndex()
    {
        return (int)InstitutionalType.Hospital;
    }

    public override string[] GetSpecificInfo()
    {
        string[] data = [this.NumberOfBeds.ToString(), this.NumberOfParkingSpots.ToString()];
        return data;
    }

    public override string[] GetSpecificLabels()
    {
        string[] data = ["No. of Beds", "No. of Parkings Spots"];
        return data;
    }
}
