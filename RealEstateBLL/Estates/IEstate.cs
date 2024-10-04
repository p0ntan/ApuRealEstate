// Created by Pontus Åkerberg 2024-09-09

namespace RealEstateBLL.Estates;

/// <summary>
/// The interface for all kinds of estates. All estates needs an ID, address and legalform.
/// All estates also needs method for returning the right estate type (restidential etc.).
/// </summary>
public interface IEstate
{
    int ID { get; set; }
    Address? Address { get; set; }
    LegalForm LegalForm { get; set; }

    /// <summary>
    /// Method to get the type of estate as in enum EstateType (Residential etc.).
    /// </summary>
    /// <returns>Type of estate.</returns>
    EstateType GetEstateType();
}
