// Created by Pontus Åkerberg 2024-10-05
using RealEstateBLL;
using RealEstateBLL.Estates;
using RealEstateBLL.Manager;
using RealEstateDTO;

namespace RealEstateService;

/// <summary>
/// EstateService is the API to use for PL when accessing estate related data and actions.
/// </summary>
public class EstateService
{
    private EstateManager _estateManager;
    private static EstateService? _estateService;

    private EstateService()
    {
        _estateManager = new();
    }

    public static EstateService GetInstance()
    {
        if (_estateService == null)
            _estateService = new EstateService();

        return _estateService;
    }

    public List<string>? GetEstateAsListOfStrings(int estateId)
    {
        Estate? estate = _estateManager.Get(estateId);

        if (estate == null)
            return null;

        List<string> strings = estate.GetDetailsAsList();

        return strings;
    }

    /// <summary>
    /// Create an estate and add it to system/manager. Method is to be seen as the Create in a CRUD API.
    /// </summary>
    /// <param name="estate">EstateDTO to add to system.</param>
    /// <returns>Boolean if success and the new id of the estate to show for user.</returns>
    public (bool, int) CreateEstate(EstateDTO estateDTO)
    {
        BLLService bllService = new(_estateManager);

        return bllService.CreateEstate(estateDTO);
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

    public bool UpdateEstate(EstateDTO estateDTO)
    {
        BLLService bllService = new(_estateManager);
        bool isUpdated = bllService.UpdateEstate(estateDTO);

        return isUpdated;
    }

    /// <summary>
    /// Reads an estate from the system/manager. Method is to be seen as the Read in a CRUD API.
    /// </summary>
    /// <param name="estateID"></param>
    /// <returns></returns>
    public EstateDTO? GetEstate(int estateID)
    {
        BLLService bllService = new(_estateManager);
        EstateDTO? estateDTO = bllService.GetEstate(estateID);

        return estateDTO;
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
}
