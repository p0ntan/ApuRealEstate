// Created by Pontus Åkerberg 2024-09-24
using RealEstateBLL.Estates;
using System.Runtime.Serialization;

namespace RealEstateBLL.Manager;

/// <summary>
/// EstateCreator ís handling the creation of a single estate and keeping track of the unique ID for the next estate.
/// </summary>
[DataContract(Name = "EstateManager", Namespace = "")]
public class EstateCreator
{
    [DataMember(Name = "NextID")]
    private int _nextId;

    public EstateCreator()
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
