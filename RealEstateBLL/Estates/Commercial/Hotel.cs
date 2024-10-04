// Created by Pontus Åkerberg 2024-09-10

using System.Runtime.Serialization;

namespace RealEstateBLL.Estates;

/// <summary>
/// Hotel is one type of a Commercial estate.
/// </summary>
public class Hotel : Commercial
{
    [DataMember]
    public int NumberOfBeds { get; set; }

    [DataMember]
    public int NumberOfParkingSpots { get; set; }

    public Hotel()
    { }

    public override int GetSpecficTypeIndex()
    {
        return (int)CommercialType.Hotel;
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
