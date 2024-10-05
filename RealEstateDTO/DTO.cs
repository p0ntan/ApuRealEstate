namespace RealEstateDTO;

public abstract record EstateDTO
{
    public int ID { get; init; }
    public string LegalForm { get; init; }
    public AddressDTO Address { get; init; }
    public PersonDTO Seller { get; init; }
    public PersonDTO Buyer { get; init; }
}
public abstract record ResidentialDTO : EstateDTO
{
    public int Area { get; init; }
    public int Bedrooms { get; init; }
}

public record VillaDTO : ResidentialDTO
{
    public int Floors { get; init; }
    public int PlotArea { get; init; }
}

public abstract record CommercialDTO : EstateDTO
{
    public int YearBuilt { get; init; }
    public decimal YearlyRevenue { get; init; }
}

public record HotelDTO : CommercialDTO
{
    public int NumberOfBeds { get; init; }
    public int NumberOfParkingSpots { get; init; }
}


public record EstateCreateDTO(
    string EstateType,
    int SpecificTypeIndex,
    string LegalForm,
    AddressDTO Address,
    PersonDTO Seller,
    PersonDTO Buyer,
    int TypeDataOne,
    int TypeDataTwo,
    int SpecificDataOne,
    int SpecificDataTwo
    );

public record PersonDTO(
    string firstName,
    string lastName,
    AddressDTO address,
    PaymentDTO? payment
    );

public record AddressDTO(string Street, string City, string ZipCode, string Country);

public record PaymentDTO(double Amount);