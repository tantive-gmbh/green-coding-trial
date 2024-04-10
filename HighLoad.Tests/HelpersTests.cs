namespace HighLoad.Tests;

[TestClass]
public class HelpersTests
{
    private Helper m_Helper => Helper.Instance;
    private readonly string m_HelperTag = Helper.Instance.IdentityTag;

    [TestInitialize]
    public void TestSetup()
    {
        m_Helper.Reset();
        Assert.AreEqual(m_HelperTag, m_Helper.IdentityTag);
    }

    [TestMethod]
    public void InitializeTest()
    {
        Assert.AreEqual(0, m_Helper.Dict.Count);

        // Act
        int actual = m_Helper.Initialize();

        // Assert
        Assert.AreEqual(100, actual);
    }

    [TestMethod]
    public void MultiInitializeTest()
    {
        // Arrange
        int taskCount = 5;
        int entryCount = 200;

        // Act
        int actual = m_Helper.Initialize(entryCount, taskCount);

        // Assert
        Assert.AreEqual(taskCount * entryCount, actual);
    }

    private int Init(int task, int entry)
    {
        m_Helper.Reset();
        return m_Helper.Initialize(entry, task);
    }

    [TestMethod]
    public void MultiMegaInitializeTest_1000_2()
    {
        Assert.AreEqual(2000, Init(1000, 2));
    }

    [TestMethod]
    public void MultiMegaInitializeTest_100_20()
    {
        Assert.AreEqual(2000, Init(100, 20));
    }

    [TestMethod]
    public void MultiMegaInitializeTest_10_200()
    {
        Assert.AreEqual(2000, Init(10, 200));
    }

    [TestMethod]
    public void MultiMegaInitializeTest_1_2000()
    {
        Assert.AreEqual(2000, Init(1, 2000));
    }
}