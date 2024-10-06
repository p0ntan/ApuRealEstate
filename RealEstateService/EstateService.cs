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
    /// Get all estate types as an arry of strings (Residential, Commercial, Insitutional).
    /// </summary>
    /// <returns>Array of strings.</returns>
    public string[] GetEstateTypes()
    {
        string[] estateTypes = Enum.GetNames(typeof(EstateType));

        return estateTypes;
    }

    /// <summary>
    /// Get the specific types of estates avalible to create, to be used in any type of form or list.
    /// </summary>
    /// <param name="estateString"></param>
    /// <returns></returns>
    public string[] GetSpecificTypes(string estateString)
    {
        try
        {
            EstateType estateType = EnumConverter.CheckEstateType(estateString);

            string[] estates;

            estates = estateType switch
            {
                EstateType.Residential => Enum.GetNames(typeof(ResidentialType)),
                EstateType.Commercial => Enum.GetNames(typeof(CommercialType)),
                EstateType.Institutional => Enum.GetNames(typeof(InstitutionalType)),
                _ => Array.Empty<string>(),
            };

            return estates;
        }
        catch
        {
            return Array.Empty<string>();
        }
    }

    /// <summary>
    /// Get all legal forms as an arry of strings.
    /// </summary>
    /// <returns>Array of strings.</returns>
    public string[] GetLegalForms()
    {
        string[] legalForms = Enum.GetNames(typeof(LegalForm));

        return legalForms;
    }
}
