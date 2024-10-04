// Created by Pontus Åkerberg 2024-09-10

using System.Runtime.Serialization;

namespace RealEstateBLL.Estates;

/// <summary>
/// Abstract class Institutional to be used as base for each concrete Institutional class.
/// </summary>
public abstract class Institutional : Estate
{
    [DataMember]
    public int EstablishedYear { get; set; }

    [DataMember]
    public int NumberOfBuildings { get; set; }

    public Institutional() : base()
    { }

    public override EstateType GetEstateType()
    {
        return EstateType.Institutional;
    }

    public override List<string> GetDetailsAsList()
    {
        List<string> details = base.GetDetailsAsList();

        // Use of concrete methods to get specifc data
        string[] specificData = this.GetSpecificInfo();
        string[] specificLabels = this.GetSpecificLabels();

        details.Add($"Established Year: {this.EstablishedYear}");
        details.Add($"No. of Buildings: {this.NumberOfBuildings}");

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
