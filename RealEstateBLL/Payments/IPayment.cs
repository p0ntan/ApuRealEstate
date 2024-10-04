// Created by Pontus Åkerberg 2024-09-15

namespace RealEstateBLL.Payments;

/// <summary>
/// Interface for Payments
/// </summary>
public interface IPayment
{
    double Amount { get; set; }
}
