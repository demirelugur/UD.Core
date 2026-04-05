namespace UD.Core.UnitTest
{
    using System.Globalization;
    using UD.Core.Enums;
    using UD.Core.Extensions;
    using static UD.Core.Enums.CRetMesaj;

    [TestFixture]
    public class UnitTest1
    {
        [SetUp]
        public void Setup()
        {
            CultureInfo.CurrentCulture = new("tr-TR");
            CultureInfo.CurrentUICulture = new("tr-TR");
        }
        [Test]
        public void Test1()
        {
            var a = Guid.NewGuid().ToString();
            var b = a.ParseOrDefault<Guid>();
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