// Created by Pontus Åkerberg 2024-09-10
namespace RealEstateBLL.Persons;

/// <summary>
/// Personmananger manages persons (buyers and sellers).
/// </summary>
public class PersonCreator
{
    /// <summary>
    /// Method that creates a person.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="address"></param>
    /// <param name="personType">Typeo of person to create.</param>
    /// <returns>The created Person</returns>
    /// <exception cref="Exception"></exception>
    static public Person? CreatePerson(string firstName, string lastName, Address address, PersonType personType)
    {
        Person? client = null;

        switch (personType)
        {
            case PersonType.Seller:
                client = new Seller(firstName, lastName, address);
                break;
            case PersonType.Buyer:
                client = new Buyer(firstName, lastName, address);
                break;
        }

        return client;
    }
}
