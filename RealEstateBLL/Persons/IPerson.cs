// Created by Pontus Åkerberg 2024-09-11

namespace RealEstateBLL.Persons;

public interface IPerson
{
    string FirstName { get; set; }
    string LastName { get; set; }
    Address Address { get; set; }
}
