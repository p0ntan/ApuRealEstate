// Created by Pontus Åkerberg 2024-10-04
namespace UtilitiesLib;

public class StringConverter
{
    public static int ConvertToInteger(string stringNumber)
    {
        if (!int.TryParse(stringNumber, out int result))
            throw new FormatException($"'{stringNumber}' is not a valid integer.");

        return result;
    }

    public static int ConvertToInteger(string stringNumber, int lowLimit, int highLimit)
    {
        if (!int.TryParse(stringNumber, out int result))
            throw new FormatException($"'{stringNumber}' is not a valid integer.");

        if (!(result >= lowLimit && result <= highLimit))
            throw new FormatException($"'{stringNumber}' is not a valid integer.");

        return result;
    }

    public static decimal ConvertToDecimal(string stringNumber)
    {
        if (!decimal.TryParse(stringNumber, out decimal result))
            throw new FormatException($"'{stringNumber}' is not a valid integer.");

        return result;
    }

    public static decimal ConvertToDecimal(string stringNumber, decimal lowLimit, decimal highLimit)
    {
        if (!decimal.TryParse(stringNumber, out decimal result))
            throw new FormatException($"'{stringNumber}' is not a valid integer.");

        if (!(result >= lowLimit && result <= highLimit))
            throw new FormatException($"'{stringNumber}' is not a valid integer.");

        return result;
    }
}
