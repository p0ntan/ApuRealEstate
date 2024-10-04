// Created by Pontus Åkerberg 2024-09-11
using System.Runtime.Serialization;

namespace RealEstateBLL.Persons;

/// <summary>
/// For types of Persons. Used in GUI when creating a seller or buyer.
/// </summary>
public enum PersonType
{
    Seller,
    Buyer
}

/// <summary>
/// Abstracs class Person is to be inherited by concrete Seller and Buyer
/// </summary>
[DataContract(Name = "Person", Namespace = "")]
[KnownType(typeof(Seller))]
[KnownType(typeof(Buyer))]
public abstract class Person : IPerson
{
    [DataMember]
    public string FirstName { get; set; }

    [DataMember]
    public string LastName { get; set; }

    [DataMember]
    public Address Address { get; set; }

    public Person()
    {
        this.FirstName = string.Empty;
        this.LastName = string.Empty;
        this.Address = new Address();
    }

    public Person(string firstName, string lastName, Address address)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Address = address;
    }

    /// <summary>
    /// Updates the details of a person with a another Person-object without.
    /// </summary>
    /// <param name="person">Person object witn new data.</param>
    public void UpdateDetails(Person person)
    {
        this.FirstName = person.FirstName;
        this.LastName = person.LastName;
        this.Address.UpdateAddress(person.Address);
    }

    /// <summary>
    /// Get the full name of a Person
    /// </summary>
    /// <returns>Full name (first and last)</returns>

    public string GetFullName()
    {
        return $"{this.FirstName} {this.LastName}";
    }

    /// <summary>
    /// Get details as a list to use for showing in a listbox.
    /// </summary>
    /// <returns>List of strings with details.</returns>
    public virtual List<string> GetDetailsAsList()
    {
        List<string> details = new List<string>();

        details.Add(this.GetFullName());
        details.Add(this.Address.ToString());

        return details;
    }
}
