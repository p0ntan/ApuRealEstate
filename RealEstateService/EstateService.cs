// Created by Pontus Åkerberg 2024-10-05
using RealEstateBLL;
using RealEstateBLL.Estates;
using RealEstateBLL.Manager;
using RealEstateDAL;
using RealEstateDTO;

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
        Estate? estate = ConvertDTOToEstate(estateDTO);

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
        Estate? estate = ConvertDTOToEstate(estateDTO);

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

        EstateDTO? estateDTO = ConvertEstateToDTO(estate);

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

    /// <summary>
    /// Converts a EstateDTO to a concrete estate class.
    /// </summary>
    /// <param name="estateDTO">The DTO to convert</param>
    /// <returns>A concrete estate object</returns>
    private Estate? ConvertDTOToEstate(EstateDTO estateDTO)
    {
        EstateMapper mapper = new();
        Estate? estate = estateDTO switch
        {
            RowhouseDTO rowhouse => mapper.MapDTO<RowhouseDTO, Rowhouse>(rowhouse),
            VillaDTO villa => mapper.MapDTO<VillaDTO, Villa>(villa),
            RentalDTO rental => mapper.MapDTO<RentalDTO, Rental>(rental),
            TenementDTO tenement => mapper.MapDTO<TenementDTO, Tenement>(tenement),
            HospitalDTO hospital => mapper.MapDTO<HospitalDTO, Hospital>(hospital),
            SchoolDTO school => mapper.MapDTO<SchoolDTO, School>(school),
            UniversityDTO university => mapper.MapDTO<UniversityDTO, University>(university),
            ShopDTO shop => mapper.MapDTO<ShopDTO, Shop>(shop),
            FactoryDTO factory => mapper.MapDTO<FactoryDTO, Factory>(factory),
            HotelDTO hotel => mapper.MapDTO<HotelDTO, Hotel>(hotel),
            WarehouseDTO warehouse => mapper.MapDTO<WarehouseDTO, Warehouse>(warehouse),
            _ => null
        };

        return estate;
    }

    /// <summary>
    /// Converts an estate to an estateDTO.
    /// </summary>
    /// <param name="estate"></param>
    /// <returns></returns>
    private EstateDTO? ConvertEstateToDTO(Estate estate)
    {
        EstateMapper mapper = new();
        EstateDTO? estateDTO = estate switch
        {
            Rowhouse rowhouse => mapper.MapDTO<Rowhouse, RowhouseDTO>(rowhouse),
            Villa villa => mapper.MapDTO<Villa, VillaDTO>(villa),
            Rental rental => mapper.MapDTO<Rental, RentalDTO>(rental),
            Tenement tenement => mapper.MapDTO<Tenement, TenementDTO>(tenement),
            Hospital hospital => mapper.MapDTO<Hospital, HospitalDTO>(hospital),
            School school => mapper.MapDTO<School, SchoolDTO>(school),
            University university => mapper.MapDTO<University, UniversityDTO>(university),
            Shop shop => mapper.MapDTO<Shop, ShopDTO>(shop),
            Factory factory => mapper.MapDTO<Factory, FactoryDTO>(factory),
            Hotel hotel => mapper.MapDTO<Hotel, HotelDTO>(hotel),
            Warehouse warehouse => mapper.MapDTO<Warehouse, WarehouseDTO>(warehouse),
            _ => null
        };

        return estateDTO;
    }

    /// <summary>
    /// Saves the estates in the estatemanager to the given filepath. The file can be a .json or .xml.
    /// </summary>
    /// <param name="filePath">Filepath to the file</param>
    /// <returns>True if saved, false if not</returns>
    public bool SaveToFile(string filePath)
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();

        List<Estate> estateList = _estateManager.GetAll();  // Could be DTOs instead?
        List<EstateDTO> estateDTOList = [];

        foreach (Estate item in estateList)
        {
            EstateDTO? estateDTO = ConvertEstateToDTO(item);

            if (estateDTO != null)
                estateDTOList.Add(estateDTO);
        }

        switch (fileExtension)
        {
            case ".json":
                return FileHandler.SaveAsJson<List<EstateDTO>>(filePath, estateDTOList);
            case ".xml":
                return FileHandler.SaveAsXML<List<EstateDTO>>(filePath, estateDTOList);
            default:
                return false;
        }
    }

    /// <summary>
    /// Loads estates from a given file and adds it to the manager which is first reset.
    /// </summary>
    /// <param name="filePath">The filpath to open</param>
    /// <returns>True if opened and manager updated, false if not.</returns>
    public bool LoadFromFile(string filePath)
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();

        List<EstateDTO>? dtoList = fileExtension switch
        {
            ".json" => FileHandler.OpenJson<List<EstateDTO>>(filePath),
            ".xml" => FileHandler.OpenXML<List<EstateDTO>>(filePath),
            _ => null
        };

        if (dtoList == null)
            return false;

        ResetManager();

        foreach (EstateDTO dto in dtoList)
        {
            Estate? estate = ConvertDTOToEstate(dto);

            if (estate != null)
                _estateManager.Add(estate.ID, estate);
        }

        return true;
    }
}
