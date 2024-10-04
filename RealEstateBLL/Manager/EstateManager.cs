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
    /// Adds an item to the dictionary with the given key. Sets the unique ID of the Estate to the nextID in manager when added.
    /// Checks that the key isn't already in dictionary and that the item is not null.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="item"></param>
    /// <returns>True if added, false if not.</returns>
    public override bool Add(int key, Estate item)
    {
        item.ID = _nextId;

        bool itemAdded = base.Add(key, item);  // Using the base Add method.

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
