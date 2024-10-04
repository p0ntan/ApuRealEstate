// Created by Pontus Åkerberg 2024-09-15
using System.Runtime.Serialization;

namespace RealEstateBLL.Payments;

/// <summary>
/// Abstract class payment to use for all concrete payments.
/// </summary>
[DataContract(Name = "Payment", Namespace = "")]
[KnownType(typeof(Bank))]
[KnownType(typeof(Paypal))]
[KnownType(typeof(WesternUnion))]
public abstract class Payment : IPayment
{
    [DataMember]
    public double Amount { get; set; }

    // Default constructor
    public Payment()
    { }

    public Payment(double amount)
    {
        this.Amount = amount;
    }

    /// <summary>
    /// Returns the type of payment as in enum PaymentType
    /// </summary>
    /// <returns>Type of payment</returns>
    public abstract PaymentType GetPaymentType();

    /// <summary>
    /// Updates all fields at once using another instanciated payment object. Has to be of the same type to update. 
    /// </summary>
    /// <param name="payment">Payment object with new info</param>
    public abstract void UpdatePayment(Payment payment);

    /// <summary>
    /// Get details as a list to use for showing in a listbox.
    /// </summary>
    /// <returns>List of strings with details.</returns>
    public List<string> GetDetailsAsList()
    {
        List<string> details = [$"Amount: {this.Amount}", this.ToString()];

        return details;
    }
}
