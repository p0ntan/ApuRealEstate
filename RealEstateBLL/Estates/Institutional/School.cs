// Created by Pontus Åkerberg 2024-09-10

using System.Runtime.Serialization;

namespace RealEstateBLL.Estates;

/// <summary>
/// School is one type of a Institutional estate.
/// </summary>
public class School : Institutional
{
    [DataMember]
    public int NumberOfTeachers { get; set; }

    [DataMember]
    public int StudentCapacity { get; set; }

    public School()
    { }

    public override int GetSpecficTypeIndex()
    {
        return (int)InstitutionalType.School;
    }

    public override string[] GetSpecificInfo()
    {
        string[] data = [this.NumberOfTeachers.ToString(), this.StudentCapacity.ToString()];
        return data;
    }

    public override string[] GetSpecificLabels()
    {
        string[] data = ["No. of Teachers", "Student Capacity"];
        return data;
    }
}
