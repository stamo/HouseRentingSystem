using HouseRentingSystem.Core.Models.House;

namespace HouseRentingSystem.Core.Contracts
{
    public interface IHouseService
    {
        Task<IEnumerable<HouseHomeModel>> LastThreeHouses();

        Task<IEnumerable<HouseCategoryModel>> AllCategories();

        Task<bool> CategoryExists(int categoryId);

        Task<int> Create(HouseModel model, int agentId);

        Task<HousesQueryModel> All(
            string? category = null,
            string? searchTerm = null,
            HouseSorting sorting = HouseSorting.Newest,
            int currentPage = 1,
            int housesPerPage = 1);

        Task<IEnumerable<string>> AllCategoriesNames();

        Task<IEnumerable<HouseServiceModel>> AllHousesByAgentId(int id);

        Task<IEnumerable<HouseServiceModel>> AllHousesByUserId(string userId);

        Task<HouseDetailsModel> HouseDetailsById(int id);

        Task<bool> Exists(int id);

        Task Edit(int houseId, HouseModel model);

        Task<bool> HasAgentWithId(int houseId, string currentUserId);

        Task<int> GetHouseCategoryId(int houseId);

        Task Delete(int houseId);

        Task<bool> IsRented(int houseId);

        Task<bool> IsRentedByUserWithId(int houseId, string currentUserId);

        Task Rent(int houseId, string currentUserId);

        Task Leave(int houseId);
    }
}
