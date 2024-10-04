// Created by Pontus Åkerberg 2024-09-10

using System.Runtime.Serialization;

namespace RealEstateBLL.Estates;

/// <summary>
/// Shop is one type of a Commercial estate.
/// </summary>
public class Shop : Commercial
{
    [DataMember]
    public int CustomerCapacity { get; set; }

    [DataMember]
    public int StorageArea { get; set; }

    public Shop()
    { }

    public override int GetSpecficTypeIndex()
    {
        return (int)CommercialType.Shop;
    }

    public override string[] GetSpecificInfo()
    {
        string[] data = [this.CustomerCapacity.ToString(), this.StorageArea.ToString()];
        return data;
    }

    public override string[] GetSpecificLabels()
    {
        string[] data = ["Customer Capacity", "Storage Area"];
        return data;
    }
}
