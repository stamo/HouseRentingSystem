namespace HouseRentingSystem.Core.Models.House
{
    public class HousesQueryModel
    {
        public int TotalHousesCount { get; set; }

        public IEnumerable<HouseServiceModel> Houses { get; set; } = new List<HouseServiceModel>();
    }
}
