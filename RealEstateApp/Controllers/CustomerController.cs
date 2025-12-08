using Application.Services;
using Application.ViewModels.Property;
using AutoMapper;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateApp.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly IPropertyService propertyService;
        private readonly IFavoriteService favoriteService;
        private readonly UserManager<UserAccount> userManager;
        private readonly IMapper mapper;

        public CustomerController(IPropertyService propertyService, IFavoriteService favoriteService, UserManager<UserAccount> userManager, IMapper mapper)
        {
            this.propertyService = propertyService;
            this.favoriteService = favoriteService; 
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var propertiesDto = await propertyService.GetAllProperties(true);

            var propertiesVm = mapper.Map<List<PropertyDisplayViewModel>>(propertiesDto);

            UserAccount? userSession = await userManager.GetUserAsync(User);

            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var favorites = await favoriteService.GetFavoritesByUser(userSession.Id);

            foreach (var property in propertiesVm)
            {
                property.IsFavorite = favorites.Any(f => f.PropertyId == property.Id);
            }

            return View(propertiesVm);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int propertyId)
        {
            UserAccount? userSession = await userManager.GetUserAsync(User);

            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var isFavorite = await favoriteService.IsFavorite(userSession.Id, propertyId);

            if (isFavorite)
            {
                await favoriteService.RemoveFavorite(userSession.Id, propertyId);
            }
            else
            {
                await favoriteService.AddFavorite(userSession.Id, propertyId);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MyFavorites()
        {
            UserAccount? userSession = await userManager.GetUserAsync(User);

            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var favorites = await favoriteService.GetFavoritesByUser(userSession.Id);

            var propertiesDto = favorites.Select(f => f.Properties).ToList();
            var propertiesVm = mapper.Map<List<PropertyDisplayViewModel>>(propertiesDto);

            foreach (var property in propertiesVm)
            {
                property.IsFavorite = true;
            }

            return View(propertiesVm);
        }

    }
}
