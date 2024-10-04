// Created by Pontus Åkerberg 2024-09-15
using System.Runtime.Serialization;

namespace RealEstateBLL.Payments;

/// <summary>
/// Paypal is one type of payment based on the abstract class Payment.
/// </summary>
[DataContract(Name = "Paypal", Namespace = "")]
public class Paypal : Payment
{
    [DataMember]
    public string Email { get; set; }

    // Default constructor
    public Paypal() : base()
    {
        this.Email = string.Empty;
    }

    public Paypal(double amount, string email) : base(amount)
    {
        this.Email = email;
    }

    public override PaymentType GetPaymentType()
    {
        return PaymentType.Paypal;
    }

    public override void UpdatePayment(Payment payment)
    {
        if (payment is Paypal paypal)
        {
            this.Amount = paypal.Amount;
            this.Email = paypal.Email;
        }
    }

    public override string ToString()
    {
        return $"Type: Paypal. {this.Email}.";
    }
}
