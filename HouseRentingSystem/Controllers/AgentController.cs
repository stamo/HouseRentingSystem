using HouseRentingSystem.Core.Models.Agent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class AgentController : Controller
    {
        [HttpGet]
        public IActionResult Become()
        {
            return View();
        }

        public async Task<IActionResult> Become(BecomeAgentModel model)
        {
            return RedirectToAction("All", "House");
        }
    }
}
