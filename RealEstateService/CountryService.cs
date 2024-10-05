// Created by Pontus Åkerberg 2024-10-05
using RealEstateBLL;

namespace RealEstateService;

public class CountryService
{
    public static string[] GetCountries()
    {
        string[] countries = Enum.GetNames(typeof(Countries));

        return countries;
    }
}
