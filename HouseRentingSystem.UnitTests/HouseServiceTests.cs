using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Exceptions;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Core.Services;
using HouseRentingSystem.Infrastructure.Data;
using HouseRentingSystem.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.UnitTests
{
    [TestFixture]
    public class HouseServiceTests
    {
        private IRepository repo;
        private ILogger<HouseService> logger;
        private IGuard guard;
        private IHouseService houseService;
        private ApplicationDbContext applicationDbContext;
        
        [SetUp]
        public void Setup()
        {
            guard = new Guard();
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("HouseDB")
                .Options;

            applicationDbContext = new ApplicationDbContext(contextOptions);

            applicationDbContext.Database.EnsureDeleted();
            applicationDbContext.Database.EnsureCreated();
        }

        [Test]
        public async Task TestHouseEdit()
        {
            var loggerMock = new Mock<ILogger<HouseService>>();
            logger = loggerMock.Object;
            var repo = new Repository(applicationDbContext);
            houseService = new HouseService(repo, guard, logger);

            await repo.AddAsync(new House() 
            { 
                Id = 1, 
                Address = "", 
                ImageUrl = "", 
                Title = "", 
                Description = "" 
            });

            await repo.SaveChangesAsync();

            await houseService.Edit(1, new HouseModel() 
            {
                Id = 1,
                Address = "",
                ImageUrl = "",
                Title = "",
                Description = "This house is edited",
            });

            var dbHouse = await repo.GetByIdAsync<House>(1);

            Assert.That(dbHouse.Description, Is.EqualTo("This house is edited"));
        }

        [Test]
        public async Task TestLast3HousesInMemory()
        {
            var loggerMock = new Mock<ILogger<HouseService>>();
            logger = loggerMock.Object;
            var repo = new Repository(applicationDbContext);
            houseService = new HouseService(repo, guard, logger);

            await repo.AddRangeAsync(new List<House>()
            {
                new House() { Id = 1, Address = "", ImageUrl = "", Title = "", Description = "" },
                new House() { Id = 3, Address = "", ImageUrl = "", Title = "", Description = "" },
                new House() { Id = 5, Address = "", ImageUrl = "", Title = "", Description = "" },
                new House() { Id = 2, Address = "", ImageUrl = "", Title = "", Description = "" }
            });

            await repo.SaveChangesAsync();
            var hoseCollection = await houseService.LastThreeHouses();

            Assert.That(3, Is.EqualTo(hoseCollection.Count()));
            Assert.That(hoseCollection.Any(h => h.Id == 1), Is.False);
        }

        [Test]
        public async Task TestLast3HousesNumberAndOrder()
        {
            var loggerMock = new Mock<ILogger<HouseService>>();
            logger = loggerMock.Object;
            var repoMock = new Mock<IRepository>();
            IQueryable<House> houses = new List<House>()
            {
                new House() { Id = 1 },
                new House() { Id = 3 },
                new House() { Id = 5 },
                new House() { Id = 2 }
            }.AsQueryable();
            repoMock.Setup(r => r.AllReadonly<House>())
                .Returns(houses);
            repo = repoMock.Object;
            houseService = new HouseService(repo, guard, logger);

            var hoseCollection = await houseService.LastThreeHouses();

            Assert.That(3, Is.EqualTo(hoseCollection.Count()));
            Assert.That(hoseCollection.Any(h => h.Id == 1), Is.False);
        }

        [TearDown]
        public void TearDown()
        {
            applicationDbContext.Dispose();
        }
    }
}
