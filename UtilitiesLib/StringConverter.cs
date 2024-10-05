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

    public static double ConvertToDouble(string stringNumber)
    {
        if (!double.TryParse(stringNumber, out double result))
            throw new FormatException($"'{stringNumber}' is not a valid integer.");

        return result;
    }

    public static double ConvertToDouble(string stringNumber, double lowLimit, double highLimit)
    {
        if (!double.TryParse(stringNumber, out double result))
            throw new FormatException($"'{stringNumber}' is not a valid integer.");

        if (!(result >= lowLimit && result <= highLimit))
            throw new FormatException($"'{stringNumber}' is not a valid integer.");

        return result;
    }
}
