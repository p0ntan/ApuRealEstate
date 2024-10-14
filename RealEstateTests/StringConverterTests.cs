// Created by Pontus Åkerberg 2024-10-09
using UtilitiesLib;

namespace RealEstateTests;

/// <summary>
/// Runs some unit tests on the string converter methods.
/// </summary>
[TestClass]
public class StringConverterTests
{
    [TestMethod]
    public void ConvertToInteger_Valid()
    {
        string input = "123";

        int result = StringConverter.ConvertToInteger(input);

        Assert.AreEqual(123, result);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ConvertToInteger_Invalid_ThrowsException()
    {
        string input = "abc";

        StringConverter.ConvertToInteger(input);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ConvertToInteger_ValidDouble_ThrowsException()
    {
        string input = "10,2";

        StringConverter.ConvertToInteger(input);
    }

    [TestMethod]
    public void ConvertToIntegerInRange_Valid()
    {
        string input = "50";
        int lowLimit = 0;
        int highLimit = 100;

        int result = StringConverter.ConvertToInteger(input, lowLimit, highLimit);

        Assert.AreEqual(50, result);
    }

    [TestMethod]
    public void ConvertToIntegerInRange_Valid_LowLimit()
    {
        string input = "0";
        int lowLimit = 0;
        int highLimit = 100;

        int result = StringConverter.ConvertToInteger(input, lowLimit, highLimit);

        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void ConvertToIntegerInRange_Valid_HighLimit()
    {
        string input = "100";
        int lowLimit = 0;
        int highLimit = 100;

        int result = StringConverter.ConvertToInteger(input, lowLimit, highLimit);

        Assert.AreEqual(100, result);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ConvertToIntegerInRange_ValidIntegerOutOfRange_ThrowsException()
    {
        string input = "101";
        int lowLimit = 0;
        int highLimit = 100;

        StringConverter.ConvertToInteger(input, lowLimit, highLimit);
    }

    [TestMethod]
    public void ConvertToDouble_Valid()
    {
        string input = "123,10";

        double result = StringConverter.ConvertToDouble(input);

        Assert.AreEqual(123.10, result);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ConvertToDouble_Invalid_ThrowsException()
    {
        string input = "abc";

        StringConverter.ConvertToDouble(input);
    }

    [TestMethod]
    public void ConvertToDoubleInRange_ValidIInRange()
    {
        string input = "005,25";
        double lowLimit = 0.220;
        double highLimit = 10.00;

        double result = StringConverter.ConvertToDouble(input, lowLimit, highLimit);

        Assert.AreEqual(5.25, result);
    }

    [TestMethod]
    public void ConvertToDoubleInRange_ValidInRange_LowLimit()
    {
        string input = "00,220";
        double lowLimit = 0.220;
        double highLimit = 10.00;

        double result = StringConverter.ConvertToDouble(input, lowLimit, highLimit);

        Assert.AreEqual(0.220, result);
    }

    [TestMethod]
    public void ConvertToDoubleInRange_ValidInRange_HighLimit()
    {
        string input = "10,000";
        double lowLimit = 0.220;
        double highLimit = 10.00;

        double result = StringConverter.ConvertToDouble(input, lowLimit, highLimit);

        Assert.AreEqual(10.000, result);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ConvertToDoubleInRange_ValidOutOfRange_ThrowsException()
    {
        string input = "10,0001";
        double lowLimit = 0.220;
        double highLimit = 10.00;

        StringConverter.ConvertToDouble(input, lowLimit, highLimit);
    }
}
