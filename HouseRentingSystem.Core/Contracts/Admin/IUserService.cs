using HouseRentingSystem.Core.Models.Admin;
using HouseRentingSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace HouseRentingSystem.Core.Contracts.Admin
{
    public interface IUserService
    {
        Task<string> UserFullName(string userId);

        Task<IEnumerable<UserServiceModel>> All();

        Task<bool> Forget(string userId);
    }
}
