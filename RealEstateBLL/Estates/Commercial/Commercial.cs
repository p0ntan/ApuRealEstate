// Created by Pontus Åkerberg 2024-09-10

using System.Runtime.Serialization;

namespace RealEstateBLL.Estates;

/// <summary>
/// Abstract class Commercial to be used as base for each concrete Commercial class.
/// </summary>
public abstract class Commercial : Estate
{
    [DataMember]
    public int YearBuilt { get; set; }

    [DataMember]
    public int YearlyRevenue { get; set; }

    public Commercial()
    { }

    public override EstateType GetEstateType()
    {
        return EstateType.Commercial;
    }

    public override List<string> GetDetailsAsList()
    {
        List<string> details = base.GetDetailsAsList();

        // Use of concrete methods to get specifc data
        string[] specificData = this.GetSpecificInfo();
        string[] specificLabels = this.GetSpecificLabels();

        details.Add($"Year Built: {this.YearBuilt}");
        details.Add($"Yearly Revenue: {this.YearlyRevenue},-");

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
