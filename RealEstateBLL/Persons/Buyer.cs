// Created by Pontus Åkerberg 2024-09-11
using RealEstateBLL.Payments;
using System.Runtime.Serialization;

namespace RealEstateBLL.Persons;

/// <summary>
/// Buyer is one type of a Person. The one buying the Estate.
/// </summary>
[DataContract(Name = "Buyer", Namespace = "")]
public class Buyer : Person
{
    [DataMember]
    public Payment? Payment { get; set; }

    public Buyer()
    { }

    public Buyer(
        string firstName,
        string lastName,
        Address address
    ) : base(firstName, lastName, address)
    { }

    /// <summary>
    /// Updates the buyers payment method without replacing the object or sets a new one if no payment exists.
    /// Making sure to keep the instanciated reference if the buyer already has an payment.
    /// If an existing payment is to be replaced with a new Buyer-object, the property setter should be used instead.
    /// </summary>
    /// <param name="payment">Updates payment object.</param>
    public void UpdatePayment(Payment payment)
    {
        // If buyer has no payment or the payment is another type, set payment to the supplied payment
        if (this.Payment == null || this.Payment.GetPaymentType() != payment.GetPaymentType())
        {
            this.Payment = payment;
            return;
        }

        this.Payment.UpdatePayment(payment);
    }

    /// <summary>
    /// Creates a list with details about the payment. Uses the basemethod and to keep adding strings to display.
    /// </summary>
    /// <returns>List of strings with data.</returns>
    public override List<string> GetDetailsAsList()
    {
        List<string> details = base.GetDetailsAsList();

        if (this.Payment != null)
        {
            details.Add("\n");
            details.Add("Payment:");
            details.AddRange(this.Payment.GetDetailsAsList());
        }
        
        return details;
    }
}
