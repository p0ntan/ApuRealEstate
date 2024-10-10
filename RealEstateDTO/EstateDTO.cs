// Created by Pontus Åkerbergh 2024-10-05
using System.Xml.Serialization;

namespace RealEstateDTO;

// Names are self explainatory
// Estate DTOs
[XmlInclude(typeof(VillaDTO))]
[XmlInclude(typeof(RowhouseDTO))]
[XmlInclude(typeof(RentalDTO))]
[XmlInclude(typeof(TenementDTO))]
[XmlInclude(typeof(FactoryDTO))]
[XmlInclude(typeof(HotelDTO))]
[XmlInclude(typeof(ShopDTO))]
[XmlInclude(typeof(WarehouseDTO))]
[XmlInclude(typeof(HospitalDTO))]
[XmlInclude(typeof(SchoolDTO))]
[XmlInclude(typeof(UniversityDTO))]
public record EstateDTO
{    
    public int ID { get; set; }
    public int LegalForm { get; set; }
    public AddressDTO Address { get; set; }
    public SellerDTO Seller { get; set; }
    public BuyerDTO Buyer { get; set; }
}

// Estate types
public record ResidentialDTO : EstateDTO
{
    public int Area { get; set; }
    public int Bedrooms { get; set; }
}

public record CommercialDTO : EstateDTO
{
    public int YearBuilt { get; set; }
    public int YearlyRevenue { get; set; }
}

public record InstitutionalDTO : EstateDTO
{
    public int EstablishedYear { get; set; }
    public int NumberOfBuildings { get; set; }
}

// Specific estates
// Residentials
public record VillaDTO : ResidentialDTO
{
    public int Floors { get; set; }
    public int PlotArea { get; set; }
}

public record RowhouseDTO : VillaDTO { }

public record ApartmentDTO : ResidentialDTO
{
    public int OnFloor { get; set; }
    public int MonthlyCost { get; set; }
}

public record RentalDTO : ApartmentDTO { }
public record TenementDTO : ApartmentDTO { }

// Commercials
public record FactoryDTO : CommercialDTO
{
    public int ProductionCapacity { get; set; }
    public int NumberOfEmployees { get; set; }
}

public record HotelDTO : CommercialDTO
{
    public int NumberOfBeds { get; set; }
    public int NumberOfParkingSpots { get; set; }
}

public record ShopDTO : CommercialDTO
{
    public int CustomerCapacity { get; set; }
    public int StorageArea { get; set; }
}

public record WarehouseDTO : CommercialDTO
{
    public int StorageArea { get; set; }
    public int NumberOfLoadingDocks { get; set; }
}

// Institutionals
public record HospitalDTO : InstitutionalDTO
{
    public int NumberOfBeds { get; set; }
    public int NumberOfParkingSpots { get; set; }
}

public record SchoolDTO : InstitutionalDTO
{
    public int NumberOfTeachers { get; set; }
    public int StudentCapacity { get; set; }
}

public record UniversityDTO : InstitutionalDTO
{
    public int CampusArea { get; set; }
    public int StudentCapacity { get; set; }
}