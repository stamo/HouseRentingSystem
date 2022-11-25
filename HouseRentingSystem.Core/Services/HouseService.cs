using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Exceptions;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Infrastructure.Data;
using HouseRentingSystem.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HouseRentingSystem.Core.Services
{
    public class HouseService : IHouseService
    {
        private readonly IRepository repo;

        private readonly IGuard guard;

        private readonly ILogger logger;

        public HouseService(
            IRepository _repo,
            IGuard _guard,
            ILogger<HouseService> _logger)
        {
            repo = _repo;
            guard = _guard;
            logger = _logger;
        }

        public async Task<HousesQueryModel> All(string? category = null, string? searchTerm = null, HouseSorting sorting = HouseSorting.Newest, int currentPage = 1, int housesPerPage = 1)
        {
            var result = new HousesQueryModel();
            var houses = repo.AllReadonly<House>()
                .Where(h => h.IsActive);

            if (string.IsNullOrEmpty(category) == false)
            {
                houses = houses
                    .Where(h => h.Category.Name == category);
            }

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                houses = houses
                    .Where(h => EF.Functions.Like(h.Title.ToLower(), searchTerm) ||
                        EF.Functions.Like(h.Address.ToLower(), searchTerm) ||
                        EF.Functions.Like(h.Description.ToLower(), searchTerm));
            }

            //switch (sorting)
            //{
            //    case HouseSorting.Price:
            //        houses = houses
            //        .OrderBy(h => h.PricePerMonth);
            //        break;
            //    case HouseSorting.NotRentedFirst:
            //        houses = houses
            //        .OrderBy(h => h.RenterId);
            //        break;
            //    default:
            //        houses = houses.OrderByDescending(h => h.Id);
            //        break;
            //}

            houses = sorting switch
            {
                HouseSorting.Price => houses
                    .OrderBy(h => h.PricePerMonth),
                HouseSorting.NotRentedFirst => houses
                    .OrderBy(h => h.RenterId),
                _ => houses.OrderByDescending(h => h.Id)
            };

            result.Houses = await houses
                .Skip((currentPage - 1) * housesPerPage)
                .Take(housesPerPage)
                .Select(h => new HouseServiceModel() 
                {
                    Address = h.Address,
                    Id = h.Id,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth,
                    Title = h.Title
                })
                .ToListAsync();

            result.TotalHousesCount = await houses.CountAsync();

            return result;
        }

        public async Task<IEnumerable<HouseCategoryModel>> AllCategories()
        {
            return await repo.AllReadonly<Category>()
                .OrderBy(c => c.Name)
                .Select(c => new HouseCategoryModel() 
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> AllCategoriesNames()
        {
            return await repo.AllReadonly<Category>()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByAgentId(int id)
        {
            return await repo.AllReadonly<House>()
                .Where(c => c.IsActive)
                .Where(c => c.AgentId == id)
                .Select(c => new HouseServiceModel() 
                {
                    Address = c.Address,
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    IsRented = c.RenterId != null,
                    PricePerMonth = c.PricePerMonth,
                    Title = c.Title
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByUserId(string userId)
        {
            return await repo.AllReadonly<House>()
                .Where(c => c.RenterId == userId)
                .Where(c => c.IsActive)
                .Select(c => new HouseServiceModel()
                {
                    Address = c.Address,
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    IsRented = c.RenterId != null,
                    PricePerMonth = c.PricePerMonth,
                    Title = c.Title
                })
                .ToListAsync();
        }

        public async Task<bool> CategoryExists(int categoryId)
        {
            return await repo.AllReadonly<Category>()
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task<int> Create(HouseModel model, int agentId)
        {
            var house = new House()
            {
                Address = model.Address,
                CategoryId = model.CategoryId,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                Title = model.Title,
                AgentId = agentId
            };

            try
            {
                await repo.AddAsync(house);
                await repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(Create), ex);
                throw new ApplicationException("Database failed to save info", ex);
            }

            return house.Id;
        }

        public async Task Delete(int houseId)
        {
            var house = await repo.GetByIdAsync<House>(houseId);
            house.IsActive = false;

            await repo.SaveChangesAsync();
        }

        public async Task Edit(int houseId, HouseModel model)
        {
            var house = await repo.GetByIdAsync<House>(houseId);

            house.Description = model.Description;
            house.ImageUrl = model.ImageUrl;
            house.PricePerMonth = model.PricePerMonth;
            house.Title = model.Title;
            house.Address = model.Address;
            house.CategoryId = model.CategoryId;

            await repo.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await repo.AllReadonly<House>()
                .AnyAsync(h => h.Id == id && h.IsActive);
        }

        public async Task<int> GetHouseCategoryId(int houseId)
        {
            return (await repo.GetByIdAsync<House>(houseId)).CategoryId;
        }

        public async Task<bool> HasAgentWithId(int houseId, string currentUserId)
        {
            bool result = false;
            var house = await repo.AllReadonly<House>()
                .Where(h => h.IsActive)
                .Where(h => h.Id == houseId)
                .Include(h => h.Agent)
                .FirstOrDefaultAsync();

            if (house?.Agent != null && house.Agent.UserId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<HouseDetailsModel> HouseDetailsById(int id)
        {
            return await repo.AllReadonly<House>()
                .Where(h => h.IsActive)
                .Where(h => h.Id == id)
                .Select(h => new HouseDetailsModel() 
                {
                    Address = h.Address,
                    Category = h.Category.Name,
                    Description = h.Description,
                    Id = id,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth,
                    Title = h.Title,
                    Agent = new Models.Agent.AgentServiceModel() 
                    {
                        Email = h.Agent.User.Email,
                        PhoneNumber = h.Agent.PhoneNumber
                    }
                    
                })
                .FirstAsync();
        }

        public async Task<bool> IsRented(int houseId)
        {
            return (await repo.GetByIdAsync<House>(houseId)).RenterId != null; 
        }

        public async Task<bool> IsRentedByUserWithId(int houseId, string currentUserId)
        {
            bool result = false;
            var house = await repo.AllReadonly<House>()
                .Where(h => h.IsActive)
                .Where(h => h.Id == houseId)
                .FirstOrDefaultAsync();

            if (house != null && house.RenterId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<HouseHomeModel>> LastThreeHouses()
        {
            return await repo.AllReadonly<House>()
                .Where(h => h.IsActive)
                .OrderByDescending(h => h.Id)
                .Select(h => new HouseHomeModel() 
                {
                    Id = h.Id,
                    ImageUrl = h.ImageUrl,
                    Title = h.Title,
                    Address = h.Address
                })
                .Take(3)
                .ToListAsync();
        }

        public async Task Leave(int houseId)
        {
            var house = await repo.GetByIdAsync<House>(houseId);
            guard.AgainstNull(house, "House can not be found");
            house.RenterId = null;

            await repo.SaveChangesAsync();
        }

        public async Task Rent(int houseId, string currentUserId)
        {
            var house = await repo.GetByIdAsync<House>(houseId);

            if (house != null && house.RenterId != null)
            {
                throw new ArgumentException("House is already rented");
            }

            guard.AgainstNull(house, "House can not be found");
            house.RenterId = currentUserId;

            await repo.SaveChangesAsync();
        }
    }
}
