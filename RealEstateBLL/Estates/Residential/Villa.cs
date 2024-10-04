// Created by Pontus Åkerberg 2024-09-10
using System.Runtime.Serialization;

namespace RealEstateBLL.Estates;

/// <summary>
/// Villa is one type of a Residential estate.
/// </summary>
[DataContract]
public class Villa : Residential
{
    [DataMember]
    public int Floors { get; set; }

    [DataMember]
    public int PlotArea { get; set; }

    public Villa()
    { }

    public override int GetSpecficTypeIndex()
    {
        return (int)ResidentialType.Villa;
    }

    public override string[] GetSpecificInfo()
    {
        string[] data = [this.Floors.ToString(), this.PlotArea.ToString()];
        return data;
    }

    public override string[] GetSpecificLabels()
    {
        string[] data = ["Floors", "Plot Area"];
        return data;
    }
}
