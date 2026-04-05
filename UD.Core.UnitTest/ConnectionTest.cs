namespace UD.Core.UnitTest
{
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Globalization;
    using System.IO;
    using UD.Core.Extensions;
    [TestFixture]
    public class ConnectionTest
    {
        private SqliteConnection connection;
        private TestDbContext context;
        [SetUp]
        public void Setup()
        {
            CultureInfo.CurrentCulture = new("en-US");
            CultureInfo.CurrentUICulture = new("en-US");
            this.connection = new("Data Source=:memory:");
            this.connection.Open();
            this.context = new(new DbContextOptionsBuilder<TestDbContext>().UseSqlite(this.connection).Options);
            this.context.Database.EnsureCreated();
            this.SeedIlAndIlceData();
        }
        [Test]
        public void Test1()
        {

        }
        [Test]
        public async Task Test2()
        {
            await Task.CompletedTask;
        }
        [TearDown]
        public void Cleanup()
        {
            this.connection.Close();
            this.context.Dispose();
            this.connection.Dispose();
        }
        private void SeedIlAndIlceData()
        {
            var json = File.ReadAllText(ResolveJsonFilePath);
            var ils = JArray.Parse(json).Select(x => new
            {
                plaka = x["plaka"].ToByte(),
                adi = x["adi"].ToStringOrEmpty(),
                buyuksehir = Convert.ToBoolean(x["buyuksehir"]),
                ilceler = x["ilceler"].Select((ilce, i) => new
                {
                    i = (i + 1),
                    adi = ilce["adi"].ToStringOrEmpty(),
                    telefonkodu = ilce["telefonkodu"].ToShort()
                }).ToArray()
            }).ToArray();
            foreach (var il in ils)
            {
                this.context.Ils.Add(new Il
                {
                    Id = il.plaka,
                    Name = il.adi,
                    BuyukSehir = il.buyuksehir,
                });
                foreach (var ilce in il.ilceler)
                {
                    this.context.Ilces.Add(new Ilce
                    {
                        Id = ((il.plaka * 100) + ilce.i).ToShort(),
                        IlId = il.plaka,
                        Name = ilce.adi,
                        TelefonKodu = ilce.telefonkodu
                    });
                }
            }
            this.context.SaveChanges();
        }
        private static string ResolveJsonFilePath
        {
            get
            {
                var directory = new DirectoryInfo(AppContext.BaseDirectory);
                while (directory != null)
                {
                    var candidate = Path.Combine(directory.FullName, "Archive", "Json", "turkiye-il-ilce.json");
                    if (File.Exists(candidate)) { return candidate; }
                    directory = directory.Parent;
                }
                throw new FileNotFoundException("turkiye-il-ilce.json dosyası bulunamadı.");
            }
        }
        private sealed class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
            public DbSet<Il> Ils { get; set; }
            public DbSet<Ilce> Ilces { get; set; }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Ilce>(entity =>
                {
                    entity.HasOne(x => x.Il).WithMany(x => x.Ilces).HasForeignKey(x => x.IlId).OnDelete(DeleteBehavior.NoAction);
                });
            }
        }
        [Table("Il")]
        private class Il
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public byte Id { get; set; }
            [Required]
            [MaxLength(20)]
            public string Name { get; set; }
            public bool BuyukSehir { get; set; }
            public virtual ICollection<Ilce> Ilces { get; set; } = [];
        }
        [Table("Ilce")]
        private class Ilce
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public short Id { get; set; }
            public byte IlId { get; set; }
            [Required]
            [MaxLength(20)]
            public string Name { get; set; }
            public short TelefonKodu { get; set; }
            public virtual Il Il { get; set; } = null!;
        }
    }
}