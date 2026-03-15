namespace UD.Core.UnitTest
{
    using System.Globalization;
    [TestFixture]
    public class UnitTest1
    {
        [SetUp]
        public void Setup()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("tr-TR");
        }
        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
        [Test]
        public async Task Test2()
        {
            await Task.CompletedTask;
            Assert.Pass();
        }
        [TearDown]
        public void Cleanup() { }
    }
}