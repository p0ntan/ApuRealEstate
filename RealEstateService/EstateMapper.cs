// Created by Pontus Åkerberg 2024-10-06

using RealEstateBLL.Estates;
using RealEstateBLL.Payments;
using RealEstateBLL.Persons;
using RealEstateBLL;
using RealEstateDTO;

namespace RealEstateService;

public class EstateMapper
{
    public static EstateType MapEstateType(int estateIndex)
    {
        EstateType eType = (EstateType)estateIndex;

        return eType;
    }

    public static Address MapAddress(AddressDTO addressDTO)
    {
        Address address = new Address(
            addressDTO.Street,
            addressDTO.City,
            addressDTO.ZipCode,
            (Countries)addressDTO.Country);

        return address;
    }

    public static Person? MapPerson(PersonType personType, PersonDTO personDTO)
    {
        Person? person;
        Address personAddress = MapAddress(personDTO.Address);

        person = personType switch
        {
            PersonType.Seller => new Seller(personDTO.FirstName, personDTO.LastName, personAddress),
            PersonType.Buyer => new Buyer(personDTO.FirstName, personDTO.LastName, personAddress),
            _ => null
        };

        if (person is Buyer buyer)
            buyer.Payment = MapPayment(((BuyerDTO)personDTO).Payment);

        return person;
    }

    public static Payment? MapPayment(PaymentDTO? paymentDTO)
    {       
        Payment? payment;
        
        payment = paymentDTO switch
        {
            BankDTO => new Bank(paymentDTO.Amount, ((BankDTO)paymentDTO).Name, ((BankDTO)paymentDTO).AccountNumber),
            PaypalDTO => new Paypal(paymentDTO.Amount, ((PaypalDTO)paymentDTO).Email),
            WesternUnionDTO => new WesternUnion(paymentDTO.Amount, ((WesternUnionDTO)paymentDTO).Name, ((WesternUnionDTO)paymentDTO).Email),
            _ => null,
        };

        return payment;
    }
}
