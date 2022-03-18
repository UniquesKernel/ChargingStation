using NUnit.Framework;

namespace ChargingStationTest
{
  public class Tests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
      Assert.That(1,Is.EqualTo(1));
    }
  }
}