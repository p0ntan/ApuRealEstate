// Created by Pontus Åkerberg 2024-09-10

namespace RealEstateBLL.Estates;

/// <summary>
/// Tenement is a type of Apartment, that in is a type of Residential.
/// </summary>
public class Tenement : Apartment
{
    public Tenement()
    {
        // Setting the default value to Tenement but can by with setter.
        this.LegalForm = LegalForm.Tenement;
    }

    public override int GetSpecficTypeIndex()
    {
        return (int)ResidentialType.Tenement;
    }
}
