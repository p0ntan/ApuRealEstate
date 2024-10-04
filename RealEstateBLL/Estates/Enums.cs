// Created by Pontus Åkerberg 2024-09-09
namespace RealEstateBLL.Estates;

/// <summary>
/// Represents type of estate.
/// </summary>
public enum EstateType
{
    Residential,
    Commercial,
    Institutional,
}

/// <summary>
/// Represents type of Residential.
/// </summary>
public enum ResidentialType
{
    Villa,
    Rowhouse,
    Rental,
    Tenement,
}

/// <summary>
/// Represents type of Commercial.
/// </summary>
public enum CommercialType
{
    Factory,
    Hotel,
    Shop,
    Warehouse,
}

/// <summary>
/// Represents type of Institutional.
/// </summary>
public enum InstitutionalType
{
    Hospital,
    School,
    University,
}

/// <summary>
/// Type of legal form.
/// </summary>
public enum LegalForm
{
    Ownership,
    Tenement, 
    Rental
}
