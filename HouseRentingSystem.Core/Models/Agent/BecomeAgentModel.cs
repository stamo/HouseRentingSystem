using System.ComponentModel.DataAnnotations;

namespace HouseRentingSystem.Core.Models.Agent
{
    public class BecomeAgentModel
    {
        [Required]
        [StringLength(15, MinimumLength = 7)]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;
    }
}
