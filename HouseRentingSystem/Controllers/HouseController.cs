using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Extensions;
using HouseRentingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HouseRentingSystem.Core.Extensions;
using static HouseRentingSystem.Areas.Admin.Constants.AdminConstants;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly IHouseService houseService;

        private readonly IAgentService agentService;

        private readonly ILogger logger;

        public HouseController(
            IHouseService _houseService,
            IAgentService _agentService,
            ILogger<HouseController> _logger)
        {
            houseService = _houseService;
            agentService = _agentService;
            logger = _logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]AllHousesQueryModel query)
        {
            var result = await houseService.All(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllHousesQueryModel.HousesPerPage);

            query.TotalHousesCount = result.TotalHousesCount;
            query.Categories = await houseService.AllCategoriesNames();
            query.Houses = result.Houses;

            return View(query);
        }

        public async Task<IActionResult> Mine()
        {
            if (User.IsInRole(AdminRolleName))
            {
                return RedirectToAction("Mine", "House", new { area = AreaName });
            }

            IEnumerable<HouseServiceModel> myHhouses;
            var userId = User.Id();

            if (await agentService.ExistsById(userId))
            {
                int agentId = await agentService.GetAgentId(userId);
                myHhouses = await houseService.AllHousesByAgentId(agentId);
            }
            else
            {
                myHhouses = await houseService.AllHousesByUserId(userId);
            }

            return View(myHhouses);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id, string information)
        {
            if ((await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            var model = await houseService.HouseDetailsById(id);

            if (information != model.GetInformation())
            {
                TempData["ErrorMessage"] = "Don't touch my slug!";

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add() 
        {
            if ((await agentService.ExistsById(User.Id())) == false)
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }

            var model = new HouseModel()
            {
                HouseCategories = await houseService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(HouseModel model)
        {
            if ((await agentService.ExistsById(User.Id())) == false)
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }

            if ((await houseService.CategoryExists(model.CategoryId)) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exists");
            }

            if (!ModelState.IsValid)
            {
                model.HouseCategories = await houseService.AllCategories();

                return View(model);
            }

            int agentId = await agentService.GetAgentId(User.Id());

            int id = await houseService.Create(model, agentId);

            return RedirectToAction(nameof(Details), new { id = id, information = model.GetInformation()  });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if ((await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await houseService.HasAgentWithId(id, User.Id())) == false)
            {
                logger.LogInformation("User with id {0} attempted to open other agent house", User.Id());

                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var house = await houseService.HouseDetailsById(id);
            var categoryId = await houseService.GetHouseCategoryId(id);

            var model = new HouseModel()
            {
                Id = id,
                Address = house.Address,
                CategoryId = categoryId,    
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                Title = house.Title,
                HouseCategories = await houseService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HouseModel model)
        {
            if (id != model.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if ((await houseService.Exists(model.Id)) == false)
            {
                ModelState.AddModelError("", "House does not exist");
                model.HouseCategories = await houseService.AllCategories();

                return View(model);
            }

            if ((await houseService.HasAgentWithId(model.Id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if ((await houseService.CategoryExists(model.CategoryId)) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist");
                model.HouseCategories = await houseService.AllCategories();

                return View(model);
            }

            if (ModelState.IsValid == false)
            {
                model.HouseCategories = await houseService.AllCategories();

                return View(model);
            }

            await houseService.Edit(model.Id, model);

            return RedirectToAction(nameof(Details), new { id = model.Id, information = model.GetInformation() });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await houseService.HasAgentWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var house = await houseService.HouseDetailsById(id);
            var model = new HouseDetailsViewModel()
            {
                Address = house.Address,
                ImageUrl = house.ImageUrl,
                Title = house.Title
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id, HouseDetailsViewModel model)
        {
            if ((await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await houseService.HasAgentWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await houseService.Delete(id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            if ((await houseService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if (!User.IsInRole(AdminRolleName) && await agentService.ExistsById(User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (await houseService.IsRented(id))
            {
                return RedirectToAction(nameof(All));
            }

            await houseService.Rent(id, User.Id());

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            if ((await houseService.Exists(id)) == false ||
                (await houseService.IsRented(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await houseService.IsRentedByUserWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await houseService.Leave(id);

            return RedirectToAction(nameof(Mine));
        }
    }
}
