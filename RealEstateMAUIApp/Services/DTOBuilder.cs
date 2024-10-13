// Created by Pontus Åkerberg 2024-10-08
using RealEstateDTO;
using RealEstateMAUIApp.Enums;

/// <summary>
/// DTO builder is for the creation of DTOs.
/// </summary>
public class EstateDTOBuilder
{
    /// <summary>
    /// Field to hold the DTO while creating.
    /// </summary>
    private EstateDTO _estate;

    /// <summary>
    /// Create a builder that creates an specific EstateDTO and saves it in own variable.
    /// </summary>
    /// <param name="eType">Type of estate (residential, commercial etc.)</param>
    /// <param name="specificIndex">Index of concrete type of estate as in Enum (Ex. 0 = villa),</param>
    /// <exception cref="InvalidOperationException"></exception>
    public EstateDTOBuilder(EstateType eType, int specificIndex)
    {
        EstateDTO? estate = CreateEstate(eType, specificIndex);

        if (estate == null)
            throw new InvalidOperationException("Unable to create estate for the given type and index.");

        _estate = estate;
    }

    /// <summary>
    /// Add id to TO
    /// </summary>
    /// <param name="idNumber">Id number</param>
    /// <returns>DTO builder</returns>
    public EstateDTOBuilder AddID(int idNumber)
    {
        _estate.ID = idNumber;
        return this;
    }

    /// <summary>
    /// Add legal form
    /// </summary>
    /// <param name="legalForm"></param>
    /// <returns></returns>
    public EstateDTOBuilder AddLegalForm(int legalForm)
    {
        _estate.LegalForm = legalForm;
        return this;
    }

    /// <summary>
    /// Add address.
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public EstateDTOBuilder AddAddress(AddressDTO address)
    {
        _estate.Address = address;
        return this;
    }

    /// <summary>
    /// Add Seller.
    /// </summary>
    /// <param name="seller"></param>
    /// <returns></returns>
    public EstateDTOBuilder AddSeller(SellerDTO seller)
    {
        _estate.Seller = seller;
        return this;
    }

    /// <summary>
    /// Add buyer.
    /// </summary>
    /// <param name="buyer"></param>
    /// <returns></returns>
    public EstateDTOBuilder AddBuyer(BuyerDTO buyer)
    {
        _estate.Buyer = buyer;
        return this;
    }

    /// <summary>
    /// Add estate type details
    /// </summary>
    /// <param name="typeDetails"></param>
    /// <returns></returns>
    public EstateDTOBuilder AddEstateTypeDetails((int typeOne, int typeTwo) typeDetails)
    {
        switch (_estate)
        {
            case ResidentialDTO residential:
                residential.Area = typeDetails.typeOne;
                residential.Bedrooms = typeDetails.typeTwo;
                break;
            case CommercialDTO commercial:
                commercial.YearBuilt = typeDetails.typeOne;
                commercial.YearlyRevenue = typeDetails.typeTwo;
                break;
            case InstitutionalDTO institutional:
                institutional.EstablishedYear = typeDetails.typeOne;
                institutional.NumberOfBuildings = typeDetails.typeTwo;
                break;
        }

        return this;
    }

    /// <summary>
    /// Add estate specific details.
    /// </summary>
    /// <param name="specificDetails"></param>
    /// <returns></returns>
    public EstateDTOBuilder AddEstateSpecificDetails((int specificOne, int specificTwo) specificDetails)
    {
        switch (_estate)
        {
            case VillaDTO house: // Villa and Rowhouse
                house.Floors = specificDetails.specificOne;
                house.PlotArea = specificDetails.specificTwo;
                break;
            case ApartmentDTO apartment: // Rental and Tenement
                apartment.OnFloor = specificDetails.specificOne;
                apartment.MonthlyCost = specificDetails.specificTwo;
                break;
            case HospitalDTO hospital:
                hospital.NumberOfBeds = specificDetails.specificOne;
                hospital.NumberOfParkingSpots = specificDetails.specificTwo;
                break;
            case SchoolDTO school:
                school.NumberOfTeachers = specificDetails.specificOne;
                school.StudentCapacity = specificDetails.specificTwo;
                break;
            case UniversityDTO university:
                university.CampusArea = specificDetails.specificOne;
                university.StudentCapacity = specificDetails.specificTwo;
                break;
            case FactoryDTO factory:
                factory.ProductionCapacity = specificDetails.specificOne;
                factory.NumberOfEmployees = specificDetails.specificTwo;
                break;
            case HotelDTO hotel:
                hotel.NumberOfBeds = specificDetails.specificOne;
                hotel.NumberOfParkingSpots = specificDetails.specificTwo;
                break;
            case ShopDTO shop:
                shop.CustomerCapacity = specificDetails.specificOne;
                shop.StorageArea = specificDetails.specificTwo;
                break;
            case WarehouseDTO warehouse:
                warehouse.StorageArea = specificDetails.specificOne;
                warehouse.NumberOfLoadingDocks = specificDetails.specificTwo;
                break;
        }

        return this;
    }

    /// <summary>
    /// Create the DTO with given estate type and specific index as in enum.
    /// </summary>
    /// <param name="estateType">The type of estate.</param>
    /// <param name="specificTypeIndex">Specific index of estate as in enum.</param>
    /// <returns></returns>
    private EstateDTO? CreateEstate(EstateType estateType, int specificTypeIndex)
    {
        EstateDTO? estate = null;

        switch (estateType)
        {
            case EstateType.Residential:
                estate = CreateResidential((ResidentialType)specificTypeIndex);
                break;
            case EstateType.Commercial:
                estate = CreateCommercial((CommercialType)specificTypeIndex);
                break;
            case EstateType.Institutional:
                estate = CreateInstitutional((InstitutionalType)specificTypeIndex);
                break;
        }

        return estate;
    }

    /// <summary>
    /// Creates a ResidentialDTO.
    /// </summary>
    /// <param name="residentialType"></param>
    /// <returns></returns>
    private ResidentialDTO? CreateResidential(ResidentialType residentialType)
    {
        ResidentialDTO? estate = null;

        switch (residentialType)
        {
            case ResidentialType.Villa:
                estate = new VillaDTO();
                break;
            case ResidentialType.Rowhouse:
                estate = new RowhouseDTO();
                break;
            case ResidentialType.Rental:
                estate = new RentalDTO();
                break;
            case ResidentialType.Tenement:
                estate = new TenementDTO();
                break;
        }

        return estate;
    }

    /// <summary>
    /// Creates a CommercialDTO.
    /// </summary>
    /// <param name="commercialType"></param>
    /// <returns></returns>
    private CommercialDTO? CreateCommercial(CommercialType commercialType)
    {
        CommercialDTO? estate = null;

        switch (commercialType)
        {
            case CommercialType.Hotel:
                estate = new HotelDTO();
                break;
            case CommercialType.Shop:
                estate = new ShopDTO();
                break;
            case CommercialType.Warehouse:
                estate = new WarehouseDTO();
                break;
            case CommercialType.Factory:
                estate = new FactoryDTO();
                break;
        }

        return estate;
    }

    /// <summary>
    /// Creates a InstitutionalDTO.
    /// </summary>
    /// <param name="institutionalType"></param>
    /// <returns></returns>
    private InstitutionalDTO? CreateInstitutional(InstitutionalType institutionalType)
    {
        InstitutionalDTO? estate = null;

        switch (institutionalType)
        {
            case InstitutionalType.School:
                estate = new SchoolDTO();
                break;
            case InstitutionalType.University:
                estate = new UniversityDTO();
                break;
            case InstitutionalType.Hospital:
                estate = new HospitalDTO();
                break;
        }

        return estate;
    }

    /// <summary>
    /// Returns the created DTO with all added blocks.
    /// </summary>
    /// <returns>The created DTO with all given data.</returns>
    public EstateDTO Build()
    {
        return _estate;
    }
}
