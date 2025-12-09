using Application.Dtos.Agent;
using Application.Dtos.Property;
using Application.Dtos.PropertyImage;
using Application.Dtos.PropertyImprovement;
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
        private readonly IUserAccountServiceForWebApp userAccountServiceForWebApp;
        private readonly IPropertyService propertyService;
        private readonly IPropertyTypeService propertyTypeService;
        private readonly IImprovementService improvementService;
        private readonly ISaleTypeService saleTypeService;
        private readonly IMapper mapper;
        private readonly UserManager<UserAccount> userManager;

        public AgentController(IUserAccountServiceForWebApp userAccountServiceForWebApp, IPropertyService propertyService, IPropertyTypeService propertyTypeService,
                ISaleTypeService saleTypeService, IImprovementService improvementService, IMapper mapper, UserManager<UserAccount> userManager)
        {
            this.userAccountServiceForWebApp = userAccountServiceForWebApp;
            this.propertyService = propertyService;
            this.propertyTypeService = propertyTypeService;
            this.improvementService = improvementService;
            this.saleTypeService = saleTypeService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        #region agent home
        public async Task<IActionResult> Index()
        {
            UserAccount? userSession = await userManager.GetUserAsync(User);

            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var propertiesDto = await propertyService.GetProperties(userSession.Id, onlyAvailable: false);

            var propertiesVm = mapper.Map<List<PropertyViewModel>>(propertiesDto);

            return View(propertiesVm);
        }
        #endregion


        #region MyProfile
        public async Task<IActionResult> Profile()
        {
            UserAccount? userSession = await userManager.GetUserAsync(User);

            var userDto = await userAccountServiceForWebApp.GetUserById<AgentDto>(userSession.Id);

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

            var currentAgent = await userAccountServiceForWebApp.GetUserById<AgentDto>(vm.Id);

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

            var result = await userAccountServiceForWebApp.EditUser(saveUserDto, origin: "AgentProfile");

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
                Improvements = new List<int>(), 
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
                Id = 0,
                Code = "",
                PropertyTypeId = vm.PropertyTypeId,
                SaleTypeId = vm.SaleTypeId,
                AgentId = userSession.Id,
                Price = vm.Price,
                Description = vm.Description,
                SizeInMeters = vm.SizeInMeters,
                Bedrooms = vm.Bedrooms,
                Bathrooms = vm.Bathrooms,
                Status = PropertyStatus.Available,
                Improvements = vm.Improvements
                    .Select(id => new PropertyImprovementDto { ImprovementId = id })
                    .ToList()
            };

            var created = await propertyService.AddPropertyAsync(dto);

            if (created == null)
            {
                TempData["ErrorMessage"] = "Propiedad no encontrada.";
                return RedirectToAction("Properties");
            }

            if (vm.Images != null && vm.Images.Any())
            {
                created.Images = vm.Images.Select(file =>
                {
                    var path = FileManager.Upload(file, created.Id.ToString(), "Properties");
                    return new PropertyImageDto
                    {
                        Id = 0,
                        PropertyId = created.Id,
                        ImageUrl = path
                    };
                }).ToList();

                await propertyService.UpdatePropertyAsync(created.Id, created);
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

            var propertyTypes = await propertyTypeService.GetAllPropertyList();
            ViewBag.PropertyTypes = new SelectList(propertyTypes, "Id", "Name", vm.PropertyTypeId);

            var saleTypes = await saleTypeService.GetAllList();
            ViewBag.SaleTypes = new SelectList(saleTypes, "Id", "Name", vm.SaleTypeId);

            var improvements = await improvementService.GetAllList();
            ViewBag.Improvements = improvements
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Name,
                    Selected = vm.Improvements.Contains(i.Id) 
                })
                .ToList();

            return View("MaintenanceProperties/EditProperty", vm);
        }


        [HttpPost]
        public async Task<IActionResult> EditProperty(EditPropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var propertyTypes = await propertyTypeService.GetAllPropertyList();
                ViewBag.PropertyTypes = new SelectList(propertyTypes, "Id", "Name", vm.PropertyTypeId);

                var saleTypes = await saleTypeService.GetAllList();
                ViewBag.SaleTypes = new SelectList(saleTypes, "Id", "Name", vm.SaleTypeId);

                var improvements = await improvementService.GetAllList();
                ViewBag.Improvements = new SelectList(improvements, "Id", "Name", vm.Improvements);

                TempData["ErrorMessage"] = "Datos inválidos al editar propiedad.";
                return View("MaintenanceProperties/EditProperty", vm);
            }

            var currentDto = await propertyService.GetPropertyById(vm.Id);

            if (currentDto == null)
            {
                TempData["ErrorMessage"] = "Propiedad no encontrada.";
                return RedirectToAction("Properties");
            }

            var editDto = new EditPropertyDto
            {
                Id = vm.Id,
                Code = vm.Code,
                PropertyTypeId = currentDto.PropertyTypeId,
                SaleTypeId = currentDto.SaleTypeId,
                AgentId = currentDto.AgentId,
                Price = vm.Price,
                Description = vm.Description,
                SizeInMeters = vm.SizeInMeters,
                Status = currentDto.Status,
                Bedrooms = vm.Bedrooms,
                Bathrooms = vm.Bathrooms,
                Improvements = vm.Improvements
                    .Select(id => new PropertyImprovementDto
                    {
                        PropertyId = vm.Id,
                        ImprovementId = id
                    }).ToList(),
                Images = currentDto.Images

            };

            if (vm.NewImages != null && vm.NewImages.Any())
            {
                editDto.Images = new List<PropertyImageDto>();
                foreach (var file in vm.NewImages)
                {
                    var url = FileManager.Upload(file, vm.Id.ToString(), "Properties");
                    editDto.Images.Add(new PropertyImageDto { Id = 0, PropertyId = 0, ImageUrl = url });
                }
            }


            var propertyDto = mapper.Map<PropertyDto>(editDto);
            await propertyService.UpdatePropertyAsync(vm.Id, propertyDto);

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
            var deleted = await propertyService.DeletePropertyAsync(vm.Id); 

            TempData["SuccessMessage"] = "Propiedad eliminada correctamente.";
            return RedirectToAction("Properties");
        }

        #endregion


    }
}
