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
    /// Id for each estate, should be unique for each estate in manager.
    /// </summary>
    [DataMember(Name = "NextID")]
    private int _nextId = 10000;

    /// <summary>
    /// Adds an new item to the dictionary, making the manager choose the unique id for the new estate.
    /// Using the _nextId to sets the unique ID of the Estate and use that at key.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>True if added, false if not.</returns>
    public bool Add(Estate item)
    {
        item.ID = _nextId;

        bool itemAdded = Add(item.ID, item);  // Using the base Add method.

        if (itemAdded)
            _nextId++;

        return itemAdded;
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
