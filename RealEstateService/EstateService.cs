// Created by Pontus Åkerberg 2024-10-05
using RealEstateBLL;
using RealEstateBLL.Estates;
using RealEstateBLL.Manager;
using RealEstateBLL.Persons;
using RealEstateDTO;

namespace RealEstateService;

/// <summary>
/// EstateService is the API to use for PL when accessing estate related data and actions.
/// </summary>
public class EstateService
{
    private EstateManager _estateManager = new();

    private static EstateService? _estateService;

    private EstateService()
    { }

    public static EstateService GetInstance()
    {
        if (_estateService == null)
            _estateService = new EstateService();

        return _estateService;
    }

    public List<string>? GetEstate(int estateId)
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
    public (bool, int) CreateEstate(EstateCreateDTO estate)
    {
        // Map all details and create classes
        EstateType eType = EstateMapper.MapEstateType(estate.EstateType);
        Address estateAddress = EstateMapper.MapAddress(estate.Address);
        Seller? seller = (Seller?)EstateMapper.MapPerson(PersonType.Seller, estate.Seller);
        Buyer? buyer = (Buyer?)EstateMapper.MapPerson(PersonType.Buyer, estate.Buyer);

        // Build estate
        EstateBuilder estateBuilder = new EstateBuilder(eType, estate.SpecificTypeIndex)
            .AddLegalForm((LegalForm)estate.LegalForm)
            .AddAddress(estateAddress)
            .AddSeller(seller)
            .AddBuyer(buyer)
            .AddEstateTypeDetails((estate.TypeDataOne, estate.TypeDataTwo))
            .AddEstateSpecificDetails((estate.SpecificDataOne, estate.SpecificDataTwo));

        Estate newEstate = estateBuilder.Build();

        // Add estate to Manager, using the add without given key to let the manager set the key.
        bool added = _estateManager.Add(newEstate);

        return (added, newEstate.ID);
    }

    /// <summary>
    /// Method to delete an estate from the system/manager. Method is to be seen as the Delete in a CRUD API.
    /// </summary>
    /// <param name="estateID">Id of estate to delete.</param>
    /// <returns>True if deleted, False if not.</returns>
    public bool DeleteEstate(int estateID)
    {
        bool isDeleted = _estateManager.Delete(estateID);

        return isDeleted;
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
}
