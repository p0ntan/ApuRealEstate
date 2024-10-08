// Created by Pontus Åkerbergh 2024-10-05
// Names are self explainatory

namespace RealEstateDTO;

// Estate DTOs
public record EstateDTO
{
    public int ID { get; init; }
    public int LegalForm { get; init; }
    public AddressDTO Address { get; init; }
    public SellerDTO Seller { get; set; }
    public BuyerDTO Buyer { get; init; }
}

// Estate types
public record ResidentialDTO : EstateDTO
{
    public int Area { get; init; }
    public int Bedrooms { get; init; }
}

public record CommercialDTO : EstateDTO
{
    public int YearBuilt { get; init; }
    public int YearlyRevenue { get; init; }
}

public record InstitutionalDTO : EstateDTO
{
    public int EstablishedYear { get; init; }
    public int NumberOfBuildings { get; init; }
}

// Specific estates
// Residentials
public record VillaDTO : ResidentialDTO
{
    public int Floors { get; init; }
    public int PlotArea { get; init; }
}

public record RowhouseDTO : VillaDTO { }

public record ApartmentDTO : ResidentialDTO
{
    public int OnFloor { get; init; }
    public int MonthlyCost { get; init; }
}

public record RentalDTO : ApartmentDTO { }
public record TenementDTO : ApartmentDTO { }

// Commercials
public record FactoryDTO : CommercialDTO
{
    public int ProductionCapacity { get; init; }
    public int NumberOfEmployees { get; init; }
}

public record HotelDTO : CommercialDTO
{
    public int NumberOfBeds { get; init; }
    public int NumberOfParkingSpots { get; init; }
}

public record ShopDTO : CommercialDTO
{
    public int CustomerCapacity { get; init; }
    public int StorageArea { get; init; }
}

public record WarehouseDTO : CommercialDTO
{
    public int StorageArea { get; init; }
    public int NumberOfLoadingDocks { get; init; }
}

// Institutionals
public record HospitalDTO : InstitutionalDTO
{
    public int NumberOfBeds { get; init; }
    public int NumberOfParkingSpots { get; init; }
}

public record SchoolDTO : InstitutionalDTO
{
    public int NumberOfTeachers { get; init; }
    public int StudentCapacity { get; init; }
}

public record UniversityDTO : InstitutionalDTO
{
    public int CampusArea { get; init; }
    public int StudentCapacity { get; init; }
}