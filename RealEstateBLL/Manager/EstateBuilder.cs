using RealEstateBLL.Estates;
using RealEstateBLL.Payments;
using RealEstateBLL.Persons;
using System.Runtime.Serialization;

namespace RealEstateBLL.Manager;

[DataContract(Name = "EstateBuilder", Namespace = "")]
public class EstateBuilder
{
    private Estate _estate;

    public EstateBuilder(EstateType estateType, int specificTypeIndex)
    {
        Estate? estate = CreateEstate(estateType, specificTypeIndex);

        if (estate == null)
            throw new InvalidOperationException("Unable to create estate for the given type and index.");

        _estate = estate;
    }

    public EstateBuilder AddID(int idNumber)
    {
        _estate.ID = idNumber;
        return this;
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

    public EstateBuilder AddPayment(Payment? payment)
    {
        if (_estate.Buyer != null)
            _estate.Buyer.Payment = payment;

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

    public EstateBuilder AddEstateSpecificDetails((int specificOne, int specificTwo) specificDetails)
    {
        switch (_estate)
        {
            case Villa house: // Villa and Rowhouse
                house.Floors = specificDetails.specificOne;
                house.PlotArea = specificDetails.specificTwo;
                break;
            case Apartment apartment: // Rental and Tenement
                apartment.OnFloor = specificDetails.specificOne;
                apartment.MonthlyCost = specificDetails.specificTwo;
                break;
            case Hospital hospital:
                hospital.NumberOfBeds = specificDetails.specificOne;
                hospital.NumberOfParkingSpots = specificDetails.specificTwo;
                break;
            case School school:
                school.NumberOfTeachers = specificDetails.specificOne;
                school.StudentCapacity = specificDetails.specificTwo;
                break;
            case University university:
                university.CampusArea = specificDetails.specificOne;
                university.StudentCapacity = specificDetails.specificTwo;
                break;
            case Factory factory:
                factory.ProductionCapacity = specificDetails.specificOne;
                factory.NumberOfEmployees = specificDetails.specificTwo;
                break;
            case Hotel hotel:
                hotel.NumberOfBeds = specificDetails.specificOne;
                hotel.NumberOfParkingSpots = specificDetails.specificTwo;
                break;
            case Shop shop:
                shop.CustomerCapacity = specificDetails.specificOne;
                shop.StorageArea = specificDetails.specificTwo;
                break;
            case Warehouse warehouse:
                warehouse.StorageArea = specificDetails.specificOne;
                warehouse.NumberOfLoadingDocks = specificDetails.specificTwo;
                break;
        }

        return this;
    }

    private Estate? CreateEstate(EstateType estateType, int specificTypeIndex)
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

    public Estate Build()
    {
        return _estate;
    }
}
