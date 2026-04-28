using Application.Dtos.Agent;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.Agent;
using Application.ViewModels.Property;
using AutoMapper;
using Azure;
using Domain.Common.Enums;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Helpers;

namespace RealEstateApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserAccountServiceForWebApp userAccountServiceForWebApp;
        private readonly IPropertyService propertyService;
        private readonly IMapper mapper;
        private readonly UserManager<UserAccount> userManager;

        public HomeController(IUserAccountServiceForWebApp userAccountServiceForWebApp, IPropertyService propertyService, IMapper mapper, UserManager<UserAccount> userManager)
        {
            this.userAccountServiceForWebApp = userAccountServiceForWebApp;
            this.propertyService = propertyService;
            this.mapper = mapper;
            this.userManager = userManager;
        }



        #region home

        public async Task<IActionResult> Index( string code,int? propertyTypeId,decimal? minPrice,decimal? maxPrice,int? bedrooms,
            int? bathrooms)
        {

            var propertiesDto = await propertyService.GetAllProperties(true);


            var query = propertiesDto.AsQueryable();


            if (!string.IsNullOrEmpty(code))
                query = query.Where(p => p.Code.Contains(code));


            if (propertyTypeId.HasValue)
                query = query.Where(p => p.PropertyTypeId == propertyTypeId.Value);


            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (bedrooms.HasValue)
                query = query.Where(p => p.Bedrooms == bedrooms.Value);

            if (bathrooms.HasValue)
                query = query.Where(p => p.Bathrooms == bathrooms.Value);

            var filteredProperties = query
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            var propertiesVm = mapper.Map<List<PropertyViewModel>>(filteredProperties);

            ViewData["Code"] = code;
            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;
            ViewData["Bedrooms"] = bedrooms;
            ViewData["Bathrooms"] = bathrooms;
            ViewData["PropertyTypeId"] = propertyTypeId;

            return View(propertiesVm);
        }


        #endregion

        public async Task<IActionResult> ListAgents(string search)
        {
  
            var agentsDto = await userAccountServiceForWebApp.GetAllUsers<AgentDto>(
                isActive: true,
                role: Roles.Agent
            );

            var agentsVm = mapper.Map<List<AgentViewModel>>(agentsDto);

            if (!string.IsNullOrEmpty(search))
            {
                agentsVm = agentsVm
                    .Where(a => a.Name.ToLower().Contains(search.ToLower()))
                    .OrderBy(a => a.Name)
                    .ToList();
            }
            else
            {
                agentsVm = agentsVm.OrderBy(a => a.Name).ToList();
            }

            return View("Agents/ListAgents", agentsVm);
        }

        public async Task<IActionResult> AgentProperties(string id)
        {
            var propertiesDto = await propertyService.GetProperties(id, onlyAvailable: true);

            var propertiesVm = mapper.Map<List<PropertyViewModel>>(propertiesDto);

            propertiesVm = propertiesVm.OrderBy(p => p.Code).ToList();

            return View("Agents/AgentProperties", propertiesVm);
        }

        public async Task<IActionResult> PropertyDetail(int id)
        {
            var propertyDto = await propertyService.GetPropertyById(id);

            if (propertyDto == null)
            {
                TempData["ErrorMessage"] = "La propiedad no existe o fue eliminada.";
                return RedirectToAction("ListAgents");
            }

            var propertyVm = mapper.Map<PropertyViewModel>(propertyDto);

            return View("Agents/PropertyDetail", propertyVm);
        }



    }

}
