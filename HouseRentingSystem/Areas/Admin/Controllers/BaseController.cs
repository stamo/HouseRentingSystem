using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HouseRentingSystem.Areas.Admin.Constants.AdminConstants;

namespace HouseRentingSystem.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Route("/Admin/[controller]/[Action]/{id?}")]
    [Authorize(Roles = AdminRolleName)]

    public class BaseController : Controller
    {
    }
}
