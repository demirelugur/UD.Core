namespace UD.Core.UnitTest
{
    using UD.Core.Helper;
    [TestFixture]
    public class UnitTest1
    {
        [SetUp]
        public void Setup()
        {
            Utilities.SetDefaultThreadCulture("tr-TR");
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