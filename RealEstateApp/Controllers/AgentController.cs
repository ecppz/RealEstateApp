using Application.Dtos.Agent;
using Application.Dtos.Property;
using Application.Dtos.PropertyImage;
using Application.Dtos.User;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.Agent;
using Application.ViewModels.Property;
using AutoMapper;
using Domain.Common.Enums;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstateApp.Helpers;
namespace RealEstateApp.Controllers
{
    [Authorize(Roles = "Agent")]
    public class AgentController : Controller
    {
        private readonly IBaseAccountService baseAccountService;
        private readonly IPropertyService propertyService;
        private readonly IPropertyTypeService propertyTypeService;
        private readonly IImprovementService improvementService;
        private readonly ISaleTypeService saleTypeService;
        private readonly IMapper mapper;
        private readonly UserManager<UserAccount> userManager;

        public AgentController(IBaseAccountService baseAccountService, IPropertyService propertyService, IPropertyTypeService propertyTypeService,
                ISaleTypeService saleTypeService, IImprovementService improvementService, IMapper mapper, UserManager<UserAccount> userManager)
        {
            this.baseAccountService = baseAccountService;
            this.propertyService = propertyService;
            this.propertyTypeService = propertyTypeService;
            this.improvementService = improvementService;
            this.saleTypeService = saleTypeService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        #region agent home
        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region MyProfile
        public async Task<IActionResult> Profile()
        {
            UserAccount? userSession = await userManager.GetUserAsync(User);

            var userDto = await baseAccountService.GetUserById<AgentDto>(userSession.Id);

            var vm = mapper.Map<AgentProfileViewModel>(userDto);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(AgentProfileViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Perfil no se pudo actualizar";
                return View(vm);
            }

            var currentAgent = await baseAccountService.GetUserById<AgentDto>(vm.Id);

            var editAgentDto = new EditAgentDto
            {
                Id = vm.Id,
                Name = vm.Name,
                LastName = vm.LastName,
                PhoneNumber = vm.PhoneNumber,
                UserName = currentAgent.UserName,
                ProfileImage = currentAgent.ProfileImage,
                Email = currentAgent.Email,
                Status = currentAgent.Status,
                Role = currentAgent.Role
            };

            var saveUserDto = mapper.Map<SaveUserDto>(editAgentDto);

            var result = await baseAccountService.EditUser(saveUserDto, origin: "AgentProfile");

            if (result != null)
            {
                TempData["SuccessMessage"] = "Perfil actualizado correctamente";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Perfil no se pudo actualizar";
                return View(vm);
            }
        }
        #endregion



        #region maintenance properties
        public async Task<IActionResult> Properties()
        {
            UserAccount? userSession = await userManager.GetUserAsync(User);
            var properties = await propertyService.GetProperties(userSession.Id, onlyAvailable: true);

            var vm = mapper.Map<List<PropertyViewModel>>(properties);
            return View("MaintenanceProperties/Properties", vm);
        }

        public async Task<IActionResult> CreateProperty()
        {
            var propertyTypes = await propertyTypeService.GetAllPropertyList();
            ViewBag.PropertyTypes = new SelectList(propertyTypes, "Id", "Name");

            var saleTypes = await saleTypeService.GetAllList();
            ViewBag.SaleTypes = new SelectList(saleTypes, "Id", "Name");

            var improvements = await improvementService.GetAllList();
            ViewBag.Improvements = new SelectList(improvements, "Id", "Name");

            return View("MaintenanceProperties/CreateProperty", new CreatePropertyViewModel
            {
                PropertyTypeId = 0,
                SaleTypeId = 0,
                Price = 0,
                Description = string.Empty,
                SizeInMeters = 0,
                Bedrooms = 0,
                Bathrooms = 0,
                ImprovementsIds = new List<int>(),
                Images = new List<IFormFile>()
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProperty(CreatePropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Datos inválidos al crear propiedad.";

                var propertyTypes = await propertyTypeService.GetAllPropertyList();
                ViewBag.PropertyTypes = new SelectList(propertyTypes, "Id", "Name");

                var saleTypes = await saleTypeService.GetAllList();
                ViewBag.SaleTypes = new SelectList(saleTypes, "Id", "Name");

                var improvements = await improvementService.GetAllList();
                ViewBag.Improvements = new SelectList(improvements, "Id", "Name");

                return View("MaintenanceProperties/CreateProperty", vm);
            }

            UserAccount? userSession = await userManager.GetUserAsync(User);

            var dto = new CreatePropertyDto
            {
                Code = "",
                Id = vm.Id,
                PropertyTypeId = vm.PropertyTypeId,
                SaleTypeId = vm.SaleTypeId,
                AgentId = userSession.Id,
                Price = vm.Price,
                Description = vm.Description,
                SizeInMeters = vm.SizeInMeters,
                Bedrooms = vm.Bedrooms,
                Bathrooms = vm.Bathrooms,
                Status = PropertyStatus.Available,
                ImprovementsIds = vm.ImprovementsIds ?? new List<int>(),
                Images = new List<PropertyImageDto>()
            };

            foreach (var file in vm.Images)
            {
                var path = FileManager.Upload(file, dto.Id.ToString(), "Properties");
                dto.Images.Add(new PropertyImageDto
                {
                    Id = dto.Id,
                    PropertyId = dto.Id,
                    ImageUrl = path
                });
            }

            var created = await propertyService.AddPropertyAsync(dto);

            if (created == null)
            {
                TempData["ErrorMessage"] = "Propiedad no encontrada.";
                return RedirectToAction("Properties");
            }

            TempData["SuccessMessage"] = "Propiedad creada correctamente.";
            return RedirectToAction("Properties");
        }


        public async Task<IActionResult> EditProperty(int id)
        {
            var dto = await propertyService.GetPropertyById(id);
            if (dto == null)
            {
                TempData["ErrorMessage"] = "Propiedad no encontrada.";
                return RedirectToAction("Properties");
            }

            var vm = mapper.Map<EditPropertyViewModel>(dto);
            return View("MaintenanceProperties/EditProperty", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditProperty(EditPropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Datos inválidos al editar propiedad.";
                return View("MaintenanceProperties/EditProperty", vm);
            }

            var editDto = new EditPropertyDto
            {
                Id = vm.Id,
                Code = vm.Code,
                PropertyTypeId = vm.PropertyTypeId,
                SaleTypeId = vm.SaleTypeId,
                Price = vm.Price,
                Description = vm.Description,
                SizeInMeters = vm.SizeInMeters,
                Bedrooms = vm.Bedrooms,
                Bathrooms = vm.Bathrooms,
                ImprovementsIds = vm.ImprovementsIds,
                Images = vm.Images
            };

            var propertyDto = mapper.Map<PropertyDto>(editDto);

            var result = await propertyService.UpdateAsync(propertyDto, vm.Id);

            TempData["SuccessMessage"] = "Propiedad editada correctamente.";
            return RedirectToAction("Properties");
        }
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var dto = await propertyService.GetById(id);
            if (dto == null)
            {
                TempData["ErrorMessage"] = "Propiedad no encontrada.";
                return RedirectToAction("Properties");
            }

            var vm = mapper.Map<DeletePropertyViewModel>(dto);
            return View("MaintenanceProperties/DeleteProperty", vm);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProperty(DeletePropertyViewModel vm)
        {
            var deleted = await propertyService.DeleteAsync(vm.Id);

            TempData["SuccessMessage"] = "Propiedad eliminada correctamente.";
            return RedirectToAction("Properties");
        }

        #endregion


    }
}
