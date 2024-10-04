using ApuRealEstate.Estates;
using ApuRealEstate.Persons;
using ApuRealEstate;
using ApuRealEstate.Manager;
using System.Runtime.Serialization;

namespace RealEstateBLL;

/// <summary>
/// EstateFactory ís handling the creation of a single estate and keeping track of the unique ID for the next estate.
/// </summary>
[DataContract(Name = "EstateFactory", Namespace = "")]
public class EstateFactory
{
    [DataMember(Name = "NextID")]
    private int _nextId;

    public EstateFactory()
    {
        _nextId = 10000;
    }

    public Estate? CreateEstate(EstateType estateType, int specificTypeIndex)
    {
        Estate? estate = null;

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

        if (estate != null)
            estate.ID = _nextId++;

        return estate;
    }

    /// <summary>
    /// Creates a residential estate.
    /// </summary>
    /// <param name="residentialType"></param>
    /// <returns></returns>
    private Residential? CreateResidential(ResidentialType residentialType)
    {
        Residential? estate = null;

        switch (residentialType)
        {
            case ResidentialType.Villa:
                estate = new Villa();
                break;
            case ResidentialType.Rowhouse:
                estate = new Rowhouse();
                break;
            case ResidentialType.Rental:
                estate = new Rental();
                break;
            case ResidentialType.Tenement:
                estate = new Tenement();
                break;
        }

        return estate;
    }

    /// <summary>
    /// Creates a commercial estate.
    /// </summary>
    /// <param name="commercialType"></param>
    /// <returns></returns>
    private Commercial? CreateCommercial(CommercialType commercialType)
    {
        Commercial? estate = null;

        switch (commercialType)
        {
            case CommercialType.Hotel:
                estate = new Hotel();
                break;
            case CommercialType.Shop:
                estate = new Shop();
                break;
            case CommercialType.Warehouse:
                estate = new Warehouse();
                break;
            case CommercialType.Factory:
                estate = new Factory();
                break;
        }

        return estate;
    }

    /// <summary>
    /// Creates a Institutional estate.
    /// </summary>
    /// <param name="institutionalType"></param>
    /// <returns></returns>
    private Institutional? CreateInstitutional(InstitutionalType institutionalType)
    {
        Institutional? estate = null;

        switch (institutionalType)
        {
            case InstitutionalType.School:
                estate = new School();
                break;
            case InstitutionalType.University:
                estate = new University();
                break;
            case InstitutionalType.Hospital:
                estate = new Hospital();
                break;
        }

        return estate;
    }
}

public class EstateBuilder
{
    private Estate _estate;
    private EstateFactory _estateFactory;

    public EstateBuilder(EstateType estateType, int specificTypeIndex)
    {
        _estateFactory = new();
        Estate? estate = _estateFactory.CreateEstate(estateType, specificTypeIndex);

        if (estate == null)
            throw new InvalidOperationException("Unable to create estate for the given type and index.");

        _estate = estate;
    }

    public EstateBuilder AddLegalForm(LegalForm legalForm)
    {
        _estate.LegalForm = legalForm;
        return this;
    }

    public EstateBuilder AddAddress(Address address)
    {
        _estate.Address = address;
        return this;
    }

    public EstateBuilder AddSeller(Seller seller)
    {
        _estate.Seller = seller;
        return this;
    }

    public EstateBuilder AddBuyer(Buyer buyer)
    {
        _estate.Buyer = buyer;
        return this;
    }

    public EstateBuilder AddEstateTypeDetails((int typeOne, int typeTwo) typeDetails)
    {
        switch (_estate)
        {
            case Residential residential:
                residential.Area = typeDetails.typeOne;
                residential.Bedrooms = typeDetails.typeTwo;
                break;
            case Commercial commercial:
                commercial.YearBuilt = typeDetails.typeOne;
                commercial.YearlyRevenue = typeDetails.typeTwo;
                break;
            case Institutional institutional:
                institutional.EstablishedYear = typeDetails.typeOne;
                institutional.NumberOfBuildings = typeDetails.typeTwo;
                break;
        }

        return this;
    }

    public EstateBuilder AddEstateSpecificDetails((int typeOne, int typeTwo) specificDetails)
    {
        switch (_estate)
        {
            case Villa house: // Villa and Rowhouse
                house.Floors = specificDetails.typeOne;
                house.PlotArea = specificDetails.typeTwo;
                break;
            case Apartment apartment: // Rental and Tenement
                apartment.OnFloor = specificDetails.typeOne;
                apartment.MonthlyCost = specificDetails.typeTwo;
                break;
            case Hospital hospital:
                hospital.NumberOfBeds = specificDetails.typeOne;
                hospital.NumberOfParkingSpots = specificDetails.typeTwo;
                break;
            case School school:
                school.NumberOfTeachers = specificDetails.typeOne;
                school.StudentCapacity = specificDetails.typeTwo;
                break;
            case University university:
                university.CampusArea = specificDetails.typeOne;
                university.StudentCapacity = specificDetails.typeTwo;
                break;
            case Factory factory:
                factory.ProductionCapacity = specificDetails.typeOne;
                factory.NumberOfEmployees = specificDetails.typeTwo;
                break;
            case Hotel hotel:
                hotel.NumberOfBeds = specificDetails.typeOne;
                hotel.NumberOfParkingSpots = specificDetails.typeTwo;
                break;
            case Shop shop:
                shop.CustomerCapacity = specificDetails.typeOne;
                shop.StorageArea = specificDetails.typeTwo;
                break;
            case Warehouse warehouse:
                warehouse.StorageArea = specificDetails.typeOne;
                warehouse.NumberOfLoadingDocks = specificDetails.typeTwo;
                break;
        }

        return this;
    }

    public Estate Build()
    {
        return _estate;
    }
}
