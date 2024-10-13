// Created by Pontus Åkerberg 2024-10-06
using System.Xml.Serialization;

namespace RealEstateDTO;

// Names are self explainatory.

// Person DTOs
[XmlInclude(typeof(SellerDTO))]
[XmlInclude(typeof(BuyerDTO))]
public record PersonDTO
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public AddressDTO Address { get; init; }
}

public record SellerDTO() : PersonDTO
{ }

public record BuyerDTO() : PersonDTO
{
    public PaymentDTO? Payment { get; init; }
}

// Address DTO
public record AddressDTO()
{
    public string Street { get; init; }
    public string City { get; init; }
    public string ZipCode { get; init; }
    public int Country { get; init; }
}

// Payment DTOs
[XmlInclude(typeof(BankDTO))]
[XmlInclude(typeof(PaypalDTO))]
[XmlInclude(typeof(WesternUnionDTO))]
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
