// Created by Pontus Åkerberg 2024-09-10

using System.Runtime.Serialization;

namespace RealEstateBLL.Estates;

/// <summary>
/// Abstract class Residential to be used as base for each concrete Residential class.
/// </summary>
[DataContract]
public abstract class Residential : Estate
{
    [DataMember]
    public int Area { get; set; }

    [DataMember]
    public int Bedrooms { get; set; }

    public Residential()
    { }

    public override EstateType GetEstateType()
    {
        return EstateType.Residential;
    }

    public override List<string> GetDetailsAsList()
    {
        List<string> details = base.GetDetailsAsList();

        // Use of concrete methods to get specifc data
        string[] specificData = this.GetSpecificInfo();
        string[] specificLabels = this.GetSpecificLabels();

        details.Add($"Area: {this.Area} m^2");
        details.Add($"Bedrooms: {this.Bedrooms}");

        try  // In a try/catch if index is out of range
        {
            details.Add($"{specificLabels[0]}: {specificData[0]}");
            details.Add($"{specificLabels[1]}: {specificData[1]}");
        }
        catch
        { }

        return details;
    }
}
