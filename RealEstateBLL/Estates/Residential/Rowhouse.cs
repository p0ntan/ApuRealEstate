// Created by Pontus Åkerberg 2024-09-10

namespace RealEstateBLL.Estates;

/// <summary>
/// Rowhouse is one type of a Residential estate. Inherits from Villa.
/// </summary>
public class Rowhouse : Villa
{
    public Rowhouse()
    { }

    public override int GetSpecficTypeIndex()
    {
        return (int)ResidentialType.Rowhouse;
    }
}
