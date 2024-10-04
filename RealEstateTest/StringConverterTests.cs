using UtilitiesLib;

namespace RealEstateTests;

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
    public void ConvertToInteger_ValidDecimal_ThrowsException()
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
    public void ConvertToDecimal_Valid()
    {
        string input = "123,10";

        decimal result = StringConverter.ConvertToDecimal(input);

        Assert.AreEqual(123.10m, result);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ConvertToDecimal_Invalid_ThrowsException()
    {
        string input = "abc";

        StringConverter.ConvertToDecimal(input);
    }

    [TestMethod]
    public void ConvertToDecimalInRange_ValidIInRange()
    {
        string input = "005,25";
        decimal lowLimit = 0.220m;
        decimal highLimit = 10.00m;

        decimal result = StringConverter.ConvertToDecimal(input, lowLimit, highLimit);

        Assert.AreEqual(5.25m, result);
    }

    [TestMethod]
    public void ConvertToDecimalInRange_ValidInRange_LowLimit()
    {
        string input = "00,220";
        decimal lowLimit = 0.220m;
        decimal highLimit = 10.00m;

        decimal result = StringConverter.ConvertToDecimal(input, lowLimit, highLimit);

        Assert.AreEqual(0.220m, result);
    }

    [TestMethod]
    public void ConvertToDecimalInRange_ValidInRange_HighLimit()
    {
        string input = "10,000";
        decimal lowLimit = 0.220m;
        decimal highLimit = 10.00m;

        decimal result = StringConverter.ConvertToDecimal(input, lowLimit, highLimit);

        Assert.AreEqual(10.000m, result);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ConvertToDecimalInRange_ValidOutOfRange_ThrowsException()
    {
        string input = "10,0001";
        decimal lowLimit = 0.220m;
        decimal highLimit = 10.00m;

        StringConverter.ConvertToDecimal(input, lowLimit, highLimit);
    }
}
