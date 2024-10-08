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
    private EstateManager _estateManager;
    private Mapper _mapper;

    private static EstateService? _estateService;

    private EstateService()
    {
        _estateManager = new();
        _mapper = new();
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
    /// Reads an estate from the system/manager. Method is to be seen as the Read in a CRUD API.
    /// </summary>
    /// <param name="estateID"></param>
    /// <returns></returns>
    public EstateDTO? GetEstate(int estateID)
    {
        Estate? estate = _estateManager.Get(estateID);

        if (estate == null)
            return null;

        EstateDTO? estateDTO = estate switch
        {
            Rowhouse rowhouse => _mapper.MapToDTO<Rowhouse, RowhouseDTO>(rowhouse),
            Villa villa => _mapper.MapToDTO<Villa, VillaDTO>(villa),
            Rental rental => _mapper.MapToDTO<Rental, RentalDTO>(rental),
            Tenement tenement => _mapper.MapToDTO<Tenement, TenementDTO>(tenement),
            Apartment apartment => _mapper.MapToDTO<Apartment, ApartmentDTO>(apartment),
            Hospital hospital => _mapper.MapToDTO<Hospital, HospitalDTO>(hospital),
            School school => _mapper.MapToDTO<School, SchoolDTO>(school),
            University university => _mapper.MapToDTO<University, UniversityDTO>(university),
            Shop shop => _mapper.MapToDTO<Shop, ShopDTO>(shop),
            Factory factory => _mapper.MapToDTO<Factory, FactoryDTO>(factory),
            Warehouse warehouse => _mapper.MapToDTO<Warehouse, WarehouseDTO>(warehouse),
            _ => null
        };

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
