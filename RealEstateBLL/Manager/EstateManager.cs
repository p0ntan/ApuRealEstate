// Created by Pontus Åkerberg 2024-09-10
using RealEstateBLL.Estates;
using RealEstateDAL;
using RealEstateDTO;
using System.Runtime.Serialization;

namespace RealEstateBLL.Manager;

/// <summary>
/// Manages all estates by implementing DictionaryManager with integers and Estate as types. As key the ID for an estate should be used.
/// </summary>
[DataContract]
public class EstateManager : DictionaryManager<int, Estate>
{
    /// <summary>
    /// Adds an new item to the manager, making the manager choose the unique id for the new estate.
    /// Checks the highest current ID in dictionary and uses the number after as new id.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>True if added, false if not.</returns>
    public bool Add(Estate item)
    {
        // Get the highest id in dictionary and add one to get new unique id.
        // If no items in dictonary use 10000 as first id.
        int newId = Dictionary.Keys.Any() ? this.Dictionary.Keys.Max() + 1: 10000;

        item.ID = newId;

        bool itemAdded = Add(item.ID, item);  // Using the base Add method.

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

    /// <summary>
    /// Method to save the state of the estatemanager to a file.
    /// </summary>
    /// <param name="filePath">Filepath to file.</param>
    /// <returns>True if saved, false if not.</returns>
    public bool SaveToFile(string filePath)
    {
        List<EstateDTO> estateDTOList = [];
        EstateMapper mapper = new();

        foreach (Estate item in Dictionary.Values)
        {
            EstateDTO? estateDTO = mapper.MapEstateToDTO(item);

            if (estateDTO != null)
                estateDTOList.Add(estateDTO);
        }

        bool success = FileHandler.SaveToFile<List<EstateDTO>>(filePath, estateDTOList);

        return success;
    }

    /// <summary>
    /// Method to load estates into the estatemanager from a file.
    /// </summary>
    /// <param name="filePath">Filepath to file</param>
    /// <returns>True if loaded, false if not.</returns>
    public bool LoadFromFile(string filePath)
    {
        List<EstateDTO>? dtoList = FileHandler.OpenFile<List<EstateDTO>>(filePath);

        if (dtoList == null)
            return false;

        DeleteAll();  // Reset current manager by removing all items.

        EstateMapper mapper = new();

        foreach (EstateDTO dto in dtoList)
        {
            Estate? estate = mapper.MapDTOToEstate(dto);

            if (estate != null)
                Add(estate.ID, estate);
        }

        return true;
    }
}
