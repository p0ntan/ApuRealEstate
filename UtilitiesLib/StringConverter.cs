// Created by Pontus Åkerberg 2024-10-04
namespace UtilitiesLib;

public class StringConverter
{
    /// <summary>
    /// Tries to convert given number to an integer.
    /// </summary>
    /// <param name="stringNumber">Given string to convert</param>
    /// <returns></returns>
    /// <exception cref="FormatException">If not valid.</exception>
    public static int ConvertToInteger(string stringNumber)
    {
        if (!int.TryParse(stringNumber, out int result))
            throw new FormatException($"'{stringNumber}' is not a valid integer.");

        return result;
    }

    /// <summary>
    /// Tries to convert given number to an integer within a range.
    /// Low beeing the lowest possible, and high beeing the highest possible number.
    /// </summary>
    /// <param name="stringNumber">Given string to convert</param>
    /// <param name="lowLimit">Low limit</param>
    /// <param name="highLimit">High limit</param>
    /// <returns></returns>
    /// <exception cref="FormatException">If not valid.</exception>

    public static int ConvertToInteger(string stringNumber, int lowLimit, int highLimit)
    {
        if (!int.TryParse(stringNumber, out int result))
            throw new FormatException($"'{stringNumber}' is not a valid integer.");

        if (!(result >= lowLimit && result <= highLimit))
            throw new FormatException($"'{stringNumber}' is not in range {lowLimit} - {highLimit}.");

        return result;
    }

    /// <summary>
    /// Tries to convert given number to a double.
    /// </summary>
    /// <param name="stringNumber">Given string to convert</param>
    /// <returns></returns>
    /// <exception cref="FormatException">If not valid.</exception>

    public static double ConvertToDouble(string stringNumber)
    {
        if (!double.TryParse(stringNumber, out double result))
            throw new FormatException($"'{stringNumber}' is not a valid decimal number.");

        return result;
    }

    /// <summary>
    /// Tries to convert given number to a double within a range.
    /// Low beeing the lowest possible, and high beeing the highest possible number.
    /// </summary>
    /// <param name="stringNumber">Given string to convert</param>
    /// <param name="lowLimit">Low limit</param>
    /// <param name="highLimit">High limit</param>
    /// <returns></returns>
    /// <exception cref="FormatException">If not valid.</exception>
    public static double ConvertToDouble(string stringNumber, double lowLimit, double highLimit)
    {
        if (!double.TryParse(stringNumber, out double result))
            throw new FormatException($"'{stringNumber}' is not a valid decimal.");

        if (!(result >= lowLimit && result <= highLimit))
            throw new FormatException($"'{stringNumber}' is not in range {lowLimit} - {highLimit}.");

        return result;
    }
}
