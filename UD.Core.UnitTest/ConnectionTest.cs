namespace UD.Core.UnitTest
{
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Globalization;
    using System.IO;
    using UD.Core.Attributes.DataAnnotations;
    using UD.Core.Extensions;
    [TestFixture]
    public class ConnectionTest
    {
        private SqliteConnection connection;
        private TestDbContext context;
        [SetUp]
        public void Setup()
        {
            CultureInfo.CurrentCulture = new("tr-TR");
            CultureInfo.CurrentUICulture = new("tr-TR");
            this.connection = new("Data Source=:memory:");
            this.connection.Open();
            this.context = new(new DbContextOptionsBuilder<TestDbContext>().UseSqlite(this.connection).Options);
            this.context.Database.EnsureCreated();
            this.SeedData();
        }
        [Test]
        public void Test1()
        {

        }
        [Test]
        public async Task Test2()
        {
            //var f = await this.context.Countries.Where(x=>x.CountriesDependent.Count > 0).ToArrayAsync();
            await Task.CompletedTask;
        }
        [TearDown]
        public void Cleanup()
        {
            this.connection.Close();
            this.context.Dispose();
            this.connection.Dispose();
        }
        private void SeedData()
        {
            var cityJsonPath = File.ReadAllText(ResolveJsonFilePath("turkiye-il.json"));
            var districtJsonPath = File.ReadAllText(ResolveJsonFilePath("turkiye-ilce.json"));
            var countryJson = File.ReadAllText(ResolveJsonFilePath("ulke.json"));
            var cityJsonData = JArray.Parse(cityJsonPath).Select(x => new
            {
                id = x["Id"].ToByte(),
                name = x["Name"].ToStringOrEmpty(),
                isMetropolitanMunicipality = Convert.ToBoolean(x["IsMetropolitanMunicipality"])
            }).ToArray();
            var cities = new List<City>();
            foreach (var city in cityJsonData)
            {
                cities.Add(new City
                {
                    Id = city.id,
                    Name = city.name,
                    IsMetropolitanMunicipality = city.isMetropolitanMunicipality,
                });
            }
            this.context.Cities.AddRange(cities);
            var countryJsonData = JArray.Parse(countryJson).Select(x => new
            {
                id = x["CountryId"].ToStringOrEmpty(),
                sovereignCountryId = x["SovereignCountryId"].ParseOrDefault<string>(),
                cca2 = x["Cca2"].ToStringOrEmpty(),
                ccn3 = x["Ccn3"].ParseOrDefault<short?>(),
                nameCommonTR = x["NameCommonTR"].ToStringOrEmpty(),
                nameOfficialTR = x["NameOfficialTR"].ToStringOrEmpty(),
                nameCommonEN = x["NameCommonEN"].ToStringOrEmpty(),
                nameOfficialEN = x["NameOfficialEN"].ToStringOrEmpty(),
                isIndependent = Convert.ToBoolean(x["IsIndependent"]),
                isUNMember = Convert.ToBoolean(x["IsUNMember"]),
                isLandLocked = Convert.ToBoolean(x["IsLandLocked"]),
                area = x["Area"].ToDecimal(),
                borders = (x["Borders"].IsNoneOrNullOrUndefined() ? null : x["Borders"].ToString(Formatting.None)),
                region = x["Region"].ToStringOrEmpty(),
                subRegion = x["SubRegion"].ParseOrDefault<string>(),
                continent = (x["Continent"].IsNoneOrNullOrUndefined() ? null : x["Continent"].ToString(Formatting.None))
            }).ToArray();
            var countries = new List<Country>();
            foreach (var country in countryJsonData)
            {
                countries.Add(new Country
                {
                    CountryId = country.id,
                    SovereignCountryId = country.sovereignCountryId,
                    Cca2 = country.cca2,
                    Ccn3 = country.ccn3,
                    NameCommonTR = country.nameCommonTR,
                    NameOfficialTR = country.nameOfficialTR,
                    NameCommonEN = country.nameCommonEN,
                    NameOfficialEN = country.nameOfficialEN,
                    IsIndependent = country.isIndependent,
                    IsUNMember = country.isUNMember,
                    IsLandLocked = country.isLandLocked,
                    Area = country.area,
                    Borders = country.borders,
                    Region = country.region,
                    SubRegion = country.subRegion,
                    Continent = country.continent
                });
            }
            this.context.Countries.AddRange(countries);
            var districtJsonData = JArray.Parse(districtJsonPath).Select(x => new
            {
                id = x["Id"].ToShort(),
                cityId = x["CityId"].ToByte(),
                name = x["Name"].ToStringOrEmpty(),
                telephoneCode = x["TelephoneCode"].ToShort()
            }).ToArray();
            var districts = new List<District>();
            foreach (var district in districtJsonData)
            {
                districts.Add(new District
                {
                    Id = district.id,
                    CityId = district.cityId,
                    Name = district.name,
                    TelephoneCode = district.telephoneCode
                });
            }
            this.context.Districts.AddRange(districts);
            this.context.SaveChanges();
        }
        private static string ResolveJsonFilePath(string fileName)
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null)
            {
                var candidate = Path.Combine(directory.FullName, "Archive", "Json", fileName);
                if (File.Exists(candidate)) { return candidate; }
                directory = directory.Parent;
            }
            throw new FileNotFoundException($"{fileName} dosyası bulunamadı.");
        }
        private sealed class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
            public DbSet<City> Cities { get; set; }
            public DbSet<Country> Countries { get; set; }
            public DbSet<District> Districts { get; set; }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Country>(entity =>
                {
                    entity.HasOne(x => x.CountrySovereign).WithMany(x => x.CountriesDependent).HasForeignKey(x => x.SovereignCountryId).OnDelete(DeleteBehavior.NoAction);
                });
                modelBuilder.Entity<District>(entity =>
                {
                    entity.HasOne(x => x.City).WithMany(x => x.Districts).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.NoAction);
                });
            }
        }
        [Table("City")]
        private partial class City
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public byte Id { get; set; }
            [UDRequired]
            [UDStringLength(20)]
            [Column(TypeName = "varchar(20)")]
            public string Name { get; set; }
            public bool IsMetropolitanMunicipality { get; set; }
            public virtual ICollection<District> Districts { get; set; } = [];
        }
        [Table("Country")]
        private partial class Country
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            [UDRequired]
            [UDStringLength(3, 3)]
            [Column(TypeName = "char(3)")]
            public string CountryId { get; set; }
            [UDStringLength(3, 3)]
            [Column(TypeName = "char(3)")]
            public string? SovereignCountryId { get; set; }
            [UDRequired]
            [UDStringLength(2, 2)]
            [Column(TypeName = "char(2)")]
            public string Cca2 { get; set; }
            public short? Ccn3 { get; set; }
            [UDRequired]
            [UDStringLength(50)]
            [Column(TypeName = "varchar(50)")]
            public string NameCommonTR { get; set; }
            [UDRequired]
            [UDStringLength(75)]
            [Column(TypeName = "varchar(75)")]
            public string NameOfficialTR { get; set; }
            [UDRequired]
            [UDStringLength(50)]
            [Column(TypeName = "varchar(50)")]
            public string NameCommonEN { get; set; }
            [UDRequired]
            [UDStringLength(75)]
            [Column(TypeName = "varchar(75)")]
            public string NameOfficialEN { get; set; }
            public bool IsIndependent { get; set; }
            public bool IsUNMember { get; set; }
            public bool IsLandLocked { get; set; }
            [Precision(18, 2)]
            [Column(TypeName = "decimal(18, 2)")]
            public decimal Area { get; set; }
            [UDStringLength(100)]
            [UDJson(JTokenType.Array)]
            [Column(TypeName = "varchar(100)")]
            public string? Borders { get; set; }
            [UDRequired]
            [UDStringLength(10)]
            [Column(TypeName = "varchar(10)")]
            public string Region { get; set; }
            [UDStringLength(30)]
            [Column(TypeName = "varchar(30)")]
            public string? SubRegion { get; set; }
            [UDRequired]
            [UDStringLength(20)]
            [UDJson(JTokenType.Array)]
            [Column(TypeName = "varchar(20)")]
            public string Continent { get; set; }
            public virtual Country? CountrySovereign { get; set; } = null;
            public virtual ICollection<Country> CountriesDependent { get; set; } = [];
        }
        [Table("District")]
        private partial class District
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public short Id { get; set; }
            public byte CityId { get; set; }
            [UDRequired]
            [UDStringLength(20)]
            [Column(TypeName = "varchar(20)")]
            public string Name { get; set; }
            public short TelephoneCode { get; set; }
            public virtual City City { get; set; } = null!;
        }
    }
}