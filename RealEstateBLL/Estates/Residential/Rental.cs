// Created by Pontus Åkerberg 2024-09-10

namespace RealEstateBLL.Estates;

/// <summary>
/// Rental is a type of Apartment.
/// </summary>
public class Rental : Apartment
{
    public Rental()
    {
        // Setting the default value to rental but can be changed when creating estate.
        this.LegalForm = LegalForm.Rental;
    }

    public override int GetSpecficTypeIndex()
    {
        return (int)ResidentialType.Rental;
    }
}
