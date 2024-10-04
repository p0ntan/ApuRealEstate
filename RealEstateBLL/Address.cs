// Created by Pontus Åkerberg 2024-09-09
using System.Runtime.Serialization;

namespace RealEstateBLL;

/// <summary>
/// Address that can be used for both estates or persons owning the estate.
/// </summary>
/// <param name="street"></param>
/// <param name="city"></param>
/// <param name="zipCode"></param>
/// <param name="country"></param>
[DataContract(Name = "Address", Namespace = "")]
public class Address
{
    [DataMember]
    public string Street { get; set; }

    [DataMember]
    public string City { get; set; }

    [DataMember]
    public string ZipCode { get; set; }

    [DataMember]
    public Countries Country { get; set; }

    // Default constructor.
    public Address()
    {
        this.Street = string.Empty;
        this.City = string.Empty;
        this.ZipCode = string.Empty;
        this.Country = Countries.Sweden;
    }

    /// <summary>
    /// An address needs all it's details to be created.
    /// </summary>
    /// <param name="street"></param>
    /// <param name="city"></param>
    /// <param name="zipCode"></param>
    /// <param name="country"></param>
    public Address(string street, string city, string zipCode, Countries country)
    {
        this.Street = street;
        this.City = city;
        this.ZipCode = zipCode;
        this.Country = country;
    }

    /// <summary>
    /// Updates all fields at once using another instanciated address object. 
    /// </summary>
    /// <param name="address">Address object with new info</param>
    public void UpdateAddress(Address address)
    {
        this.Street = address.Street;
        this.City = address.City;
        this.ZipCode = address.ZipCode;
        this.Country = address.Country;
    }

    public override string ToString()
    {
        return $"{this.Street}. {this.ZipCode}, {this.City}. {this.Country}";
    }
}
