// Created by Pontus Åkerberg 2024-10-05
using RealEstateDTO;
using RealEstateBLL;
using RealEstateBLL.Estates;
using RealEstateBLL.Manager;

namespace RealEstateService;

/// <summary>
/// EstateService is the API to use for PL when accessing estate related data and actions.
/// </summary>
public class EstateService
{
    private EstateManager _estateManager;
    private static EstateService? _estateService;  // Using singleton for keeping one instance of EstateService

    /// <summary>
    /// Private construtor to use the Singleton pattern.
    /// </summary>
    private EstateService()
    {
        _estateManager = new();
    }

    /// <summary>
    /// Returns the instance of the class by using Singleton pattern.
    /// </summary>
    /// <returns></returns>
    public static EstateService GetInstance()
    {
        if (_estateService == null)
            _estateService = new EstateService();

        return _estateService;
    }

    /// <summary>
    /// Returns a list with strings for a single estate. If no estae is round, the list is empty.
    /// </summary>
    /// <param name="estateId"></param>
    /// <returns>List of strings with details about an estate.</returns>
    public List<string> GetEstateAsListOfStrings(int estateId)
    {
        Estate? estate = _estateManager.Get(estateId);
        List<string> strings = [];

        if (estate == null)
            return strings;

        strings = estate.GetDetailsAsList();

        return strings;
    }

    /// <summary>
    /// Create an estate and add it to system/manager. Method is to be seen as the Create in a CRUD API.
    /// </summary>
    /// <param name="estate">EstateDTO to add to system.</param>
    /// <returns>Boolean if success and the new id of the estate to show for user.</returns>
    public (bool, int) CreateEstate(EstateDTO estateDTO)
    {
        EstateMapper mapper = new();
        Estate? estate = mapper.MapDTOToEstate(estateDTO);

        int newID = -1;  // Set a default id to use when estate not created/added

        if (estate == null)
            return (false, newID);

        bool estateAdded = _estateManager.Add(estate);

        if (estateAdded)
            newID = estate.ID;  // ID is updated since estate is reached by reference

        return (estateAdded, newID);
    }

    /// <summary>
    /// Deletes an estate from the system/manager. Method is to be seen as the Delete in a CRUD API.
    /// </summary>
    /// <param name="estateID">Id of estate to delete.</param>
    /// <returns>True if deleted, False if not.</returns>
    public bool DeleteEstate(int estateID)
    {
        bool isDeleted = _estateManager.Delete(estateID);

        return isDeleted;
    }

    /// <summary>
    /// Updates the the estate in manager with a given DTO. Method is to be seen as the U in a CRUD API.
    /// </summary>
    /// <param name="estateDTO"></param>
    /// <returns>True if updates, false if not</returns>
    public bool UpdateEstate(EstateDTO estateDTO)
    {
        EstateMapper mapper = new();
        Estate? estate = mapper.MapDTOToEstate(estateDTO);

        if (estate == null)
            return false;

        bool estateUpdated = _estateManager.Change(estate.ID, estate);

        return estateUpdated;
    }

    /// <summary>
    /// Reads an estate from the system/manager. Method is to be seen as the Read in a CRUD API.
    /// </summary>
    /// <param name="estateID"></param>
    /// <returns>The found estate as an DTO.</returns>
    public EstateDTO? GetEstate(int estateID)
    {
        Estate? estate = _estateManager.Get(estateID);

        if (estate == null)
            return null;

        EstateMapper mapper = new();
        EstateDTO? estateDTO = mapper.MapEstateToDTO(estate);

        return estateDTO;
    }

    /// <summary>
    /// Gets the estates from a country based on a given string.
    /// </summary>
    /// <param name="country">Country to filter by</param>
    /// <returns>Array of strings representing the found countries.</returns>
    public string[] GetEstateByCountry(string country)
    {
        if (!Enum.TryParse(country, out Countries foundCountry))
            return [];

        Dictionary<Countries, List<Estate>> countryDict = _estateManager.FindByCountry(foundCountry);
        List<string> countryList = [];

        foreach (Estate estate in countryDict[foundCountry])
        {
            countryList.Add(estate.ToString());
        }

        return countryList.ToArray();
    }

    /// <summary>
    /// Returns a list of the current estates in estatemanager as strings.
    /// </summary>
    /// <returns>Array of string.</returns>
    public string[] GetEstatesAsArrayOfStrings()
    {
        string[] estateList = _estateManager.ToStringArray();

        return estateList;
    }

    /// <summary>
    /// Resets the estatemanager to it's original state.
    /// </summary>
    public void ResetManager()
    {
        _estateManager = new EstateManager();
    }

    /// <summary>
    /// Saves the estates in the estatemanager to the given filepath. The file can be a .json or .xml.
    /// </summary>
    /// <param name="filePath">Filepath to the file</param>
    /// <returns>True if saved, false if not</returns>
    public bool SaveToFile(string filePath)
    {
        bool success = _estateManager.SaveToFile(filePath);

        return success;
    }

    /// <summary>
    /// Loads estates from a given file and adds it to the manager which is first reset.
    /// </summary>
    /// <param name="filePath">The filpath to open</param>
    /// <returns>True if opened and manager updated, false if not.</returns>
    public bool LoadFromFile(string filePath)
    {
        bool success = _estateManager.LoadFromFile(filePath);

        return success;
    }
}
