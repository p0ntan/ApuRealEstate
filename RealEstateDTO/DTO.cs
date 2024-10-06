namespace RealEstateDTO;

public record EstateCreateDTO(
    int? estateId,
    int EstateType,
    int SpecificTypeIndex,
    int LegalForm,
    AddressDTO Address,
    PersonDTO Seller,
    PersonDTO Buyer,
    int TypeDataOne,
    int TypeDataTwo,
    int SpecificDataOne,
    int SpecificDataTwo
    );

public abstract record EstateDTO
{
    public int ID { get; init; }
    public int LegalForm { get; init; }
    public AddressDTO Address { get; init; }
    public PersonDTO Seller { get; init; }
    public PersonDTO Buyer { get; init; }
}

// Estate types

public abstract record ResidentialDTO : EstateDTO
{
    public int Area { get; init; }
    public int Bedrooms { get; init; }
}

public abstract record CommercialDTO : EstateDTO
{
    public int YearBuilt { get; init; }
    public int YearlyRevenue { get; init; }
}

public abstract record InstitutionalDTO : EstateDTO
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
public record ApartmentDTO : ResidentialDTO
{
    public int OnFloor { get; init; }
    public int MonthlyCost { get; init; }
}

public record RowHouseDTO : VillaDTO {}
public record RentalDTO : ApartmentDTO { }
public record TenementDTO : ApartmentDTO { }

// Commercials
public record HotelDTO : CommercialDTO
{
    public int NumberOfBeds { get; init; }
    public int NumberOfParkingSpots { get; init; }
}

// Person DTOs
public record PersonDTO(
    string FirstName,
    string LastName,
    AddressDTO Address,
    PaymentDTO? Payment
    );

// Address DTO

public record AddressDTO(string Street, string City, string ZipCode, int Country);

// Payment DTOs
public abstract record PaymentDTO
{
    public double Amount { get; init; }
}
public record BankDTO : PaymentDTO
{
    public required string Name { get; init; }
    public required string AccountNumber { get; init; }
}

public record PaypalDTO : PaymentDTO
{
    public required string Email { get; init; }
}

public record WesternUnionDTO : PaymentDTO
{
    public required string Name { get; init; }
    public required string Email { get; init; }
}
