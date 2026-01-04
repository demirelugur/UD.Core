namespace UD.Core.UnitTest
{
    [TestFixture]
    public class Tests
    {
        // dotnet pack UD.Core\UD.Core.csproj -c Release -o ./nupkgs
        [SetUp]
        public void Setup() { }
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