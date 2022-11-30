using HouseRentingSystem.Core.Models.Admin;

namespace HouseRentingSystem.Core.Contracts.Admin
{
    public interface IUserService
    {
        Task<string> UserFullName(string userId);

        Task<IEnumerable<UserServiceModel>> All();
    }
}
