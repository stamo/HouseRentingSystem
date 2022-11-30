﻿using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Contracts.Admin;
using HouseRentingSystem.Core.Exceptions;
using HouseRentingSystem.Core.Services;
using HouseRentingSystem.Core.Services.Admin;
using HouseRentingSystem.Infrastructure.Data.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HouseRentingServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IHouseService, HouseService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IGuard, Guard>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
