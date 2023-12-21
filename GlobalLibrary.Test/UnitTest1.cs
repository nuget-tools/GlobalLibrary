namespace GlobalLibrary.Test;

using Global;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        TestContext.WriteLine("Test1() start");
        TestContext.WriteLine(Util.ToJson(new { a = 123, b = 456 }));
        Assert.Pass();
        TestContext.WriteLine("Test1() end");
    }

    [Test]
    public void AddTest()
    {
        TestContext.WriteLine("AddTest() start");
        int param1 = 5;
        int param2 = 11;
        int answer = 16;
        Assert.AreEqual(
            answer,
            param1 + param2,
            $"足し算ロジックNG：param1={param1}, param2={param2}");
        TestContext.WriteLine("AddTest() end");
    }

}