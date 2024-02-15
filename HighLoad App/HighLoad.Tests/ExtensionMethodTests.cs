using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace HighLoad.Tests;

[TestClass]
public class ExtensionMethodTests
{
    #region --- conversions ---
    [TestMethod]
    public void TestOtherwiseMethod()
    {
        // Arrange
        string firstChoice = null;
        string alternativeValue = "alternative";

        // Act
        var result = firstChoice.Otherwise(alternativeValue);

        // Assert
        Assert.AreEqual(alternativeValue, result);
    }

    [TestMethod]
    public void TestAsStringMethodWithDefaultValue()
    {
        // Arrange
        object obj = null;

        // Act
        var result = obj.AsString();

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void TestAsStringMethodWithAlternativeValue()
    {
        // Arrange
        object obj = null;
        var alternativeValue = "alternative";

        // Act
        var result = obj.AsString(alternativeValue);

        // Assert
        Assert.AreEqual(alternativeValue, result);
    }

    [TestMethod]
    public void TestAsDateNMethod()
    {
        //Arrange
        object validDate = "2022-01-01";
        object invalidDate = "not a date";
        DateTime expectedDate = new DateTime(2022, 01, 01);

        //Act
        var resultValid = validDate.AsDateN();
        var resultInvalid = invalidDate.AsDateN();

        //Assert
        Assert.AreEqual(expectedDate, resultValid);
        Assert.IsNull(resultInvalid);
    }

    [TestMethod]
    public void TestAsDateMethod()
    {
        // Arrange
        object validDate = "2022-01-01";
        object invalidDate = "not a date";

        DateTime expectedDate = new DateTime(2022, 01, 01);
        DateTime expectedMinDate = DateTime.MinValue;

        // Act
        var resultValid = validDate.AsDate();
        var resultInvalid = invalidDate.AsDate();

        // Assert
        Assert.AreEqual(expectedDate, resultValid);
        Assert.AreEqual(expectedMinDate, resultInvalid);
    }

    [TestMethod]
    public void TestAsIntN()
    {
        object obj = 123;
        Assert.AreEqual(123, obj.AsIntN());

        object objNull = null;
        Assert.IsNull(objNull.AsIntN());

        object objStr = "Hello";
        Assert.IsNull(objStr.AsIntN());
    }

    [TestMethod]
    public void TestAsInt()
    {
        object obj = 123;
        Assert.AreEqual(123, obj.AsInt());

        object objNull = null;
        Assert.AreEqual(0, objNull.AsInt());

        object objStr = "Hello";
        Assert.AreEqual(0, objStr.AsInt());
    }

    [TestMethod]
    public void TestAsIntWithDefaultValue()
    {
        object obj = 123;
        Assert.AreEqual(123, obj.AsInt(999));

        object objNull = null;
        Assert.AreEqual(999, objNull.AsInt(999));

        object objStr = "Hello";
        Assert.AreEqual(999, objStr.AsInt(999));
    }

    [TestMethod]
    public void TestAsFloat()
    {
        object obj = 12.3f;
        Assert.AreEqual(12.3f, obj.AsFloat());

        object objNull = null;
        Assert.AreEqual(0f, objNull.AsFloat());

        object objStr = "Hello";
        Assert.AreEqual(0f, objStr.AsFloat());
    }

    [TestMethod]
    public void TestAsFloatWithDefaultValue()
    {
        object obj = 12.3f;
        Assert.AreEqual(12.3f, obj.AsFloat(999.9f));

        object objNull = null;
        Assert.AreEqual(999.9f, objNull.AsFloat(999.9f));

        object objStr = "Hello";
        Assert.AreEqual(999.9f, objStr.AsFloat(999.9f));
    }

    [TestMethod]
    public void TestAsBool()
    {
        object obj = true;
        Assert.AreEqual(true, obj.AsBool());

        object objNull = null;
        Assert.AreEqual(false, objNull.AsBool());

        object objStr = "Hello";
        Assert.AreEqual(true, objStr.AsBool());
    }

    [TestMethod]
    public void TestAsBoolWithDefaultValue()
    {
        object obj = true;
        Assert.AreEqual(true, obj.AsBool(false));

        object objNull = null;
        Assert.AreEqual(true, objNull.AsBool(true));

        object objStr = "Hello";
        Assert.AreEqual(true, objStr.AsBool(false));
    }
    #endregion

    #region --- string extensions ---
    [TestMethod]
    public void TestIsEmpty()
    {
        Assert.IsTrue("".IsEmpty());
        Assert.IsTrue(((string)null).IsEmpty());
        Assert.IsFalse("notEmpty".IsEmpty());
    }

    [TestMethod]
    public void TestIsNotEmpty()
    {
        Assert.IsFalse("".IsNotEmpty());
        Assert.IsFalse(((string)null).IsNotEmpty());
        Assert.IsTrue("notEmpty".IsNotEmpty());
    }

    [TestMethod]
    public void TestMaxLen()
    {
        Assert.AreEqual("12...", "123456".MaxLen(5), "Default three dots have not been appended correctly");
        Assert.AreEqual("123456", "123456".MaxLen(10));
        Assert.AreEqual("", "".MaxLen(5));
        Assert.AreEqual("12---", "123456".MaxLen(5, "---"), "Three dashes have not been appended correctly");
    }

    [TestMethod]
    public void TestPlural()
    {
        Assert.AreEqual("1 apple", 1.Plural("apple"));
        Assert.AreEqual("2 apples", 2.Plural("apple"));
        Assert.AreEqual("1 mouse", 1.Plural("mouse", "mice"));
        Assert.AreEqual("2 mice", 2.Plural("mouse", "mice"));
    }

    [TestMethod]
    public void TestReverse()
    {
        Assert.AreEqual("", "".Reverse());
        Assert.AreEqual("54321", "12345".Reverse());
        Assert.AreEqual(string.Empty, ((string)null).Reverse());
    }

    [TestMethod]
    public void TestIsGuid()
    {
        Assert.IsTrue("a1111111-1111-1111-1111-111111111111".IsGuid());
        Assert.IsFalse("Not a Guid".IsGuid());
    }

    [TestMethod]
    public void TestToMD5()
    {
        var input = "Hello World";
        var data = MD5.HashData(Encoding.Default.GetBytes(input));

        var sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
            sBuilder.Append(data[i].ToString("x2"));
        var expectedResult = sBuilder.ToString();

        Assert.AreEqual(expectedResult, input.ToMD5());
    }

    [TestMethod]
    public void TestChecksum()
    {
        var input = "Hello World";
        var checksum1 = input.Checksum();

        input = "Hello Universe";
        var checksum2 = input.Checksum();

        Assert.AreNotEqual(checksum1, checksum2);
        Assert.AreEqual(32, checksum1.Length);
    }

    [TestMethod]
    public void TestMultiply()
    {
        var input = "Hello World";
        var actual = input.Multiply(5);

        Assert.AreEqual(55, actual.Length);
        Assert.AreEqual("Hello WorldHello WorldHello WorldHello WorldHello World", actual);

        input = "   \t   ";
        actual = input.Multiply(5);
        Assert.AreEqual(0, actual.Length);
        Assert.AreEqual(string.Empty, actual);
    }
    #endregion

    // #region --- path stuff ---
    // [TestMethod]
    // public void TestGetAllFiles()
    // {
    //     var path = @"C:\TestDirectory"; // replace this with a real directory path on your machine
    //     var files1 = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
    //     var files2 = path.GetAllFiles();

    //     Assert.IsTrue(files1.SequenceEqual(files2));
    // }

    // [TestMethod]
    // public void TestFindFilePath()
    // {
    //     var rootPath = @"C:\TestDirectory"; // replace this with a real directory path on your machine
    //     var fileName = "testFile.txt"; // replace this with a real file name in the above directory

    //     var expectedPath = Path.Combine(rootPath, fileName);
    //     var foundPath = rootPath.FindFilePath(fileName);

    //     Assert.AreEqual(expectedPath, foundPath);
    // }
    // #endregion
}