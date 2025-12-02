using Application.Dtos.PropertyType;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.PropertyType;
using AutoMapper;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RealEstateApp.Controllers
{
    public class PropertyTypeMaintenanceController : Controller
    {

        private readonly IPropertyService PropertyService;
        private readonly IPropertyTypeService PropertyTypeService;
        private readonly IMapper mapper;
        private readonly UserManager<UserAccount> userManager;
        private readonly IUserAccountServiceForWebApp userAccountServiceForWebApp;

        public PropertyTypeMaintenanceController(IPropertyService propertyService, IPropertyTypeService propertyTypeService, IMapper mapper, UserManager<UserAccount> userManager, IUserAccountServiceForWebApp userAccountServiceForWebApp)
        {
            PropertyService = propertyService;
            PropertyTypeService = propertyTypeService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.userAccountServiceForWebApp = userAccountServiceForWebApp;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var types = await PropertyTypeService.GetAll();
            var viewModel = mapper.Map<List<PropertyTypeListViewModel>>(types);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new PropertyTypeCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyTypeCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = mapper.Map<PropertyTypeCreateDto>(model);
            await PropertyTypeService.AddAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await PropertyTypeService.GetById(id);
            if (dto == null)
                return NotFound();

            var viewModel = mapper.Map<PropertyTypeEditViewModel>(dto);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropertyTypeEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = mapper.Map<PropertyTypeUpdateDto>(model);
            await PropertyTypeService.UpdateAsync(dto, model.Id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await PropertyTypeService.GetById(id);
            if (dto == null)
                return NotFound();

            var viewModel = mapper.Map<PropertyTypeListViewModel>(dto);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await PropertyTypeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }




    }
}
