// Created by Pontus Åkerberg 2024-09-11
using System.Runtime.Serialization;

namespace RealEstateBLL.Persons;

/// <summary>
/// Seller is one type of a Person that is selling an estate.
/// </summary>
[DataContract(Name = "Seller", Namespace = "")]
public class Seller : Person
{
    public Seller()
    { }

    public Seller(
        string firstName,
        string lastName,
        Address address
    ) : base(firstName, lastName, address)
    { }
}