using HouseRentingSystem.Core.Contracts;

namespace HouseRentingSystem.Core.Models.House
{
    public class HouseHomeModel : IHouseModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string Address { get; init; } = null!;
    }
}
