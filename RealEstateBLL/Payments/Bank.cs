// Created by Pontus Åkerberg 2024-09-15

using System.Runtime.Serialization;

namespace RealEstateBLL.Payments;

/// <summary>
/// Bank is one type of payment based on the abstract class Payment.
/// </summary>
[DataContract(Name = "Bank", Namespace = "")]
public class Bank : Payment
{
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public string AccountNumber { get; set; }

    public Bank() : base()
    {
        this.Name = string.Empty;
        this.AccountNumber = string.Empty;
    }

    public Bank(double amount, string name, string accountNumber): base(amount)
    {
        Name = name;
        AccountNumber = accountNumber;
    }

    public override PaymentType GetPaymentType()
    {
        return PaymentType.Bank;
    }

    public override void UpdatePayment(Payment payment)
    {
        if (payment is Bank bank)
        {
            this.Amount = bank.Amount;
            this.AccountNumber = bank.AccountNumber;
            this.Name = bank.Name;
        }
    }

    public override string ToString()
    {
        return $"Type: Bank. {this.Name}. {this.AccountNumber}";
    }
}
