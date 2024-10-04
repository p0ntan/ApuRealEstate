// Created by Pontus Åkerberg 2024-09-10
using System.Runtime.Serialization;

namespace RealEstateBLL.Estates;

/// <summary>
/// Warehouse is one type of a Commercial estate.
/// </summary>
public class Warehouse : Commercial
{
    [DataMember]
    public int StorageArea { get; set; }

    [DataMember]
    public int NumberOfLoadingDocks { get; set; }

    public Warehouse()
    { }

    public override int GetSpecficTypeIndex()
    {
        return (int)CommercialType.Warehouse;
    }

    public override string[] GetSpecificInfo()
    {
        string[] data = [this.StorageArea.ToString(), this.NumberOfLoadingDocks.ToString()];
        return data;
    }

    public override string[] GetSpecificLabels()
    {
        string[] data = ["Storage Area", "No. of Loading Docks"];
        return data;
    }
}
