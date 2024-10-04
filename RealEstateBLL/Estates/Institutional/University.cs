// Created by Pontus Åkerberg 2024-09-10

using System.Runtime.Serialization;

namespace RealEstateBLL.Estates;

/// <summary>
/// University is one type of a Institutional estate.
/// </summary>
public class University : Institutional
{
    [DataMember]
    public int CampusArea { get; set; }

    [DataMember]
    public int StudentCapacity { get; set; }

    public University()
    { }

    public override int GetSpecficTypeIndex()
    {
        return (int)InstitutionalType.University;
    }

    public override string[] GetSpecificInfo()
    {
        string[] data = [this.CampusArea.ToString(), this.StudentCapacity.ToString()];
        return data;
    }

    public override string[] GetSpecificLabels()
    {
        string[] data = ["Campus Area", "Student Capacity"];
        return data;
    }
}
