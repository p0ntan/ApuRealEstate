// Created by Pontus Åkerberg 2024-09-15
using System.Runtime.Serialization;

namespace RealEstateBLL.Payments;

/// <summary>
/// WesternUnion is one type of payment based on the abstract class Payment.
/// </summary>
[DataContract(Name = "WesternUnion", Namespace = "")]
public class WesternUnion : Payment
{
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Email { get; set; }

    // Default constructor
    public WesternUnion() : base()
    {
        this.Name = string.Empty;
        this.Email = string.Empty;
    }

    public WesternUnion(double amount, string name, string email) : base(amount)
    {
        this.Name = name;
        this.Email = email;
    }

    public override PaymentType GetPaymentType()
    {
        return PaymentType.Western_Union;
    }

    public override void UpdatePayment(Payment payment)
    {
        if (payment is WesternUnion westenUnion)
        {
            this.Amount = westenUnion.Amount;
            this.Name = westenUnion.Name;
            this.Email = westenUnion.Email;
        }
    }

    public override string ToString()
    {
        return $"Type: Western Union. {this.Name}, {this.Email}.";
    }
}
