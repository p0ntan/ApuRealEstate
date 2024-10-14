// Created by Pontus Åkerberg 2024-09-09
using RealEstateBLL.Persons;
using System.Runtime.Serialization;

namespace RealEstateBLL.Estates;

/// <summary>
/// A base abstract class that all concrete estates (classes) inherits from. 
/// </summary>
[DataContract(Name = "Estate", Namespace = "")]
[KnownType(typeof(Villa))]
[KnownType(typeof(Rowhouse))]
[KnownType(typeof(Rental))]
[KnownType(typeof(Tenement))]
[KnownType(typeof(Factory))]
[KnownType(typeof(Hotel))]
[KnownType(typeof(Shop))]
[KnownType(typeof(Warehouse))]
[KnownType(typeof(Hospital))]
[KnownType(typeof(School))]
[KnownType(typeof(University))]
public abstract class Estate : IEstate
{
    [DataMember]
    public int ID { get; set; }

    [DataMember]
    public Address? Address { get; set; }

    [DataMember]
    public Seller? Seller { get; set; }

    [DataMember]
    public Buyer? Buyer { get; set; }

    [DataMember]
    public LegalForm LegalForm { get; set; }

    /// <summary>
    /// Default constructor. Estates get created and then data gets set through property setters.
    /// </summary>
    public Estate() { }

    /// <summary>
    /// Method that returns the type of estate as in enum EstateType.
    /// </summary>
    /// <returns>The type of estate.</returns>
    public abstract EstateType GetEstateType();

    /// <summary>
    /// Method that should return the index of the specific type as in enum for that specific type.
    /// </summary>
    /// <returns>The index of the specific type.</returns>
    public abstract int GetSpecficTypeIndex();

    /// <summary>
    /// Method to get the specific estate data as an array of strings. Used to simplify updating GUI with saved data.
    /// </summary>
    /// <returns>Array with strings with specific estate info.</returns>
    public abstract string[] GetSpecificInfo();

    /// <summary>
    /// Method to get the labels for specific data as an array of strings. Used to simplify the creation of input fields in GUI.
    /// </summary>
    /// <returns>Array with strings with specific estate labels.</returns>
    public abstract string[] GetSpecificLabels();

    /// <summary>
    /// Get details as a list to use for showing in a listbox.
    /// </summary>
    /// <returns>List of strings with details.</returns>
    public virtual List<string> GetDetailsAsList()
    {
        List<string> details = new List<string>();

        string info = $"ID: {this.ID}. ";
        string specificType = this switch
        {
            Residential => ((ResidentialType)GetSpecficTypeIndex()).ToString(),
            Commercial => ((CommercialType)GetSpecficTypeIndex()).ToString(),
            Institutional => ((InstitutionalType)GetSpecficTypeIndex()).ToString(),
            _ => ""
        };
        
        info += $"{this.GetEstateType()}, {specificType}. ";
        info += this.LegalForm.ToString();
        details.Add(info);
        details.Add("---------");

        if (this.Address != null)
        {
            details.Add(this.Address.ToString());
            details.Add("---------");
        }

        if (this.Seller != null)
        {
            details.Add("Seller:");
            details.AddRange(this.Seller.GetDetailsAsList());
            details.Add("---------");
        }

        if (this.Buyer != null)
        {
            details.Add("Buyer:");
            details.AddRange(this.Buyer.GetDetailsAsList());
            details.Add("---------");
        }

        return details;
    }

    /// <summary>
    /// Generates string representation of an Estate. Contains basic info.
    /// </summary>
    /// <returns>String with basic info about the estate.</returns>
    public override string ToString()
    {
        return $"{ID}; {GetEstateType().ToString()}; {Address?.ToString()}";
    }
}
