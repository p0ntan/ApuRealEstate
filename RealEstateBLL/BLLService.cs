// Created by Pontus Åkerberg 2024-10-05
using RealEstateBLL.Estates;
using RealEstateBLL.Manager;
using RealEstateDAL;
using RealEstateDTO;

namespace RealEstateBLL;

/// <summary>
/// BLLServices is the entrypoint for the SL to the BLL.
/// </summary>
public class BLLService
{
    private EstateManager _estateManager;
    public BLLService(EstateManager estateManager)
    {
        _estateManager = estateManager;
    }
    /// <summary>
    /// Gets an estate from the given manager. 
    /// </summary>
    /// <param name="estateID">Id for the estate.</param>
    /// <param name="estateManager">EstateManager to use and find estate.</param>
    /// <returns>The found estate, null if none found.</returns>
    public EstateDTO? GetEstate(int estateID)
    {
        Estate? estate = _estateManager.Get(estateID);

        if (estate == null)
            return null;

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

    public (bool success, int newID) CreateEstate(EstateDTO estateDTO)
    {
        Estate? estate = ConvertDTOToEstate(estateDTO);

        int newID = -1;

        if (estate == null)
            return (false, newID);

        bool estateAdded = _estateManager.Add(estate);

        if (estateAdded)
            newID = estate.ID;

        return (estateAdded,  newID);
    }

    public bool UpdateEstate(EstateDTO estateDTO)
    {
        Estate? estate = ConvertDTOToEstate(estateDTO);

        if (estate == null)
            return false;

        bool estateUpdated = _estateManager.Change(estate.ID, estate);

        return estateUpdated;
    }

    private static Estate? ConvertDTOToEstate(EstateDTO estateDTO)
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

    public bool SaveToFile(string filePath)
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();

        switch (fileExtension)
        {
            case ".json":
                return FileHandler.SaveAsJson<EstateManager>(filePath, _estateManager);
            case ".xml":
                return FileHandler.SaveAsXML<EstateManager>(filePath, _estateManager);
            default:
                return false;
        }
    }
}
