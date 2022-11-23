using HouseRentingSystem.Core.Models.Statistics;

namespace HouseRentingSystem.Core.Contracts
{
    public interface IStatisticsService
    {
        Task<StatisticsServiceModel> Total();
    }
}
