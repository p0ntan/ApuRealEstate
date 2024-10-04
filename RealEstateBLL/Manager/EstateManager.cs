// Created by Pontus Åkerberg 2024-09-10
using RealEstateBLL.Estates;
using System.Runtime.Serialization;

namespace RealEstateBLL.Manager;

/// <summary>
/// Manages all estates by implementing DictionaryManager with integers and Estate as types. As key the ID for an estate should be used.
/// </summary>
[DataContract]
public class EstateManager : DictionaryManager<int, Estate>
{
    /// <summary>
    /// EstateCreator is in charge of creating estates.
    /// </summary>
    [DataMember(Name = "EstateCreator")]
    private EstateCreator _estateHandler = new EstateCreator();

    /// <summary>
    /// Method to create a new estate by using the class EstateCreator.
    /// </summary>
    /// <param name="estateType">Type of estate</param>
    /// <param name="specificTypeIndex">Index of the specific Estate type as in enums for that estate type.</param>
    /// <returns></returns>
    public Estate? CreateEstate(EstateType estateType, int specificTypeIndex)
    {
        return _estateHandler.CreateEstate(estateType, specificTypeIndex);
    }

    /// <summary>
    /// Method to filter estates by country and returns a dictionary with the key of a country as in enum Countries and a list of estates.
    /// </summary>
    /// <param name="country">Country to filter</param>
    /// <returns></returns>
    public Dictionary<Countries, List<Estate>> FindByCountry(Countries country)
    {
        List<Estate> listOfEstates = new();
        Dictionary<Countries, List<Estate>> dictionary = new();

        foreach (Estate estate in this.Dictionary.Values)
        {
            if (estate.Address?.Country == country)
            {
                listOfEstates.Add(estate);
            }
        }

        dictionary.Add(country, listOfEstates);

        return dictionary;
    }
}
