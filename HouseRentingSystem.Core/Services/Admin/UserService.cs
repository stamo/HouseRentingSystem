using HouseRentingSystem.Core.Contracts.Admin;
using HouseRentingSystem.Core.Models.Admin;
using HouseRentingSystem.Infrastructure.Data;
using HouseRentingSystem.Infrastructure.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Services.Admin
{
    public class UserService : IUserService
    {
        private readonly IRepository repo;

        private readonly UserManager<ApplicationUser> userManager;

        public UserService(
            IRepository _repo,
            UserManager<ApplicationUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
        }

        public async Task<IEnumerable<UserServiceModel>> All()
        {
            List<UserServiceModel> result;

            result = await repo.AllReadonly<Agent>()
                .Where(a => a.User.IsActive)
                .Select(a => new UserServiceModel() 
                {
                    UserId = a.UserId,
                    Email = a.User.Email,
                    FullName = $"{ a.User.FirstName } { a.User.LastName }",
                    PhoneNumber = a.PhoneNumber
                })
                .ToListAsync();

            string[] agentIds = result.Select(a => a.UserId).ToArray();

            result.AddRange(await repo.AllReadonly<ApplicationUser>()
                .Where(u => agentIds.Contains(u.Id) == false)
                .Where(u => u.IsActive)
                .Select(u => new UserServiceModel() 
                {
                    UserId = u.Id,
                    Email = u.Email,
                    FullName = $"{ u.FirstName } { u.LastName }"
                }).ToListAsync());

            return result;
        }

        public async Task<bool> Forget(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            user.PhoneNumber = null;
            user.FirstName = null;
            user.Email = null;
            user.IsActive = false;
            user.LastName = null;
            user.NormalizedEmail = null;
            user.NormalizedUserName = null;
            user.PasswordHash = null;
            user.UserName = $"forgottenUser-{DateTime.Now.Ticks}";

            var result = await userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<string> UserFullName(string userId)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(userId);

            return $"{user?.FirstName} {user?.LastName}".Trim();
        }
    }
}
