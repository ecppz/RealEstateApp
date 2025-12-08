using Application.Dtos.Admin;
using Application.Dtos.Agent;
using Application.Dtos.Developer;
using Application.Dtos.User;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.Admin;
using Application.ViewModels.Agent;
using Application.ViewModels.Developer;
using AutoMapper;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace RealEstateApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserAccountServiceForWebApp userAccountServiceForWebApp;
        private readonly IPropertyService propertyService;
        private readonly IMapper mapper;

        public AdminController(IUserAccountServiceForWebApp userAccountServiceForWebApp, IPropertyService propertyService, IMapper mapper)
        {
            this.userAccountServiceForWebApp = userAccountServiceForWebApp;
            this.propertyService = propertyService;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var allProperties = await propertyService.GetAllProperties(onlyAvailable: false);
            int availableProperties = allProperties.Count(p => p.Status == PropertyStatus.Available);
            int soldProperties = allProperties.Count(p => p.Status == PropertyStatus.Sold);


            var agents = await userAccountServiceForWebApp.GetUsersByRole<AgentDto>(Roles.Agent);
            int activeAgents = agents.Count(a => a.Status == UserStatus.Active);
            int inactiveAgents = agents.Count(a => a.Status == UserStatus.Inactive);

            var clients = await userAccountServiceForWebApp.GetUsersByRole<UserDto>(Roles.Customer);
            int activeClients = clients.Count(c => c.Status == UserStatus.Active);
            int inactiveClients = clients.Count(c => c.Status == UserStatus.Inactive);

            var developers = await userAccountServiceForWebApp.GetUsersByRole<DeveloperDto>(Roles.Developer);
            int activeDevelopers = developers.Count(d => d.Status == UserStatus.Active);
            int inactiveDevelopers = developers.Count(d => d.Status == UserStatus.Inactive);

            var vm = new AdminHomeViewModel
            {
                AvailableProperties = availableProperties,
                SoldProperties = soldProperties,
                ActiveAgents = activeAgents,
                InactiveAgents = inactiveAgents,
                ActiveClients = activeClients,
                InactiveClients = inactiveClients,
                ActiveDevelopers = activeDevelopers,
                InactiveDevelopers = inactiveDevelopers
            };

            return View(vm);
        }

        #region maintenance agents
        public async Task<IActionResult> Agents()
        {
            var agents = await userAccountServiceForWebApp.GetUsersByRole<AgentDto>(Roles.Agent);
            var vm = mapper.Map<List<AgentViewModel>>(agents);

            foreach (var agent in vm)
            {
                var properties = await propertyService.GetProperties(agent.Id, onlyAvailable: false);
                agent.PropertyCount = properties.Count;
            }

            return View("MaintenanceAgents/Agents", vm);
        }


        public async Task<IActionResult> ActivateAgent(string id)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<AgentDto>(id);
            if (dto == null || dto.Role != Roles.Agent)
            {
                return RedirectToAction("Agents");
            }

            var vm = mapper.Map<ActivateAgentViewModel>(dto);
            return View("MaintenanceAgents/ActivateAgent", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ActivateAgent(ActivateAgentViewModel vm)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<AgentDto>(vm.Id);
            if (dto == null || dto.Role != Roles.Agent)
            {
                TempData["ErrorMessage"] = "No se puede activar este agente.";
                return RedirectToAction("Agents");
            }

            dto.Status = UserStatus.Active;

            var saveDto = mapper.Map<SaveUserDto>(dto);

            var result = await userAccountServiceForWebApp.EditUser(saveDto, origin: "Admin");

            if (result.HasError)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault() ?? "Error al activar agente.";
                return RedirectToAction("Agents");
            }

            TempData["SuccessMessage"] = "Agente activado correctamente.";
            return RedirectToAction("Agents");
        }

        public async Task<IActionResult> DeactivateAgent(string id)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<AgentDto>(id);
            if (dto == null || dto.Role != Roles.Agent)
            {
                return RedirectToAction("Agents");
            }

            var vm = mapper.Map<DeactivateAgentViewModel>(dto);
            return View("MaintenanceAgents/DeactivateAgent", vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateAgent(DeactivateAgentViewModel vm)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<AgentDto>(vm.Id);
            if (dto == null || dto.Role != Roles.Agent)
            {
                TempData["ErrorMessage"] = "No se puede desactivar este agente.";
                return RedirectToAction("Agents");
            }

            dto.Status = UserStatus.Inactive;

            var saveDto = mapper.Map<SaveUserDto>(dto);
            var result = await userAccountServiceForWebApp.EditUser(saveDto, origin: "Admin");

            if (result.HasError)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault() ?? "Error al desactivar agente.";
                return RedirectToAction("Agents");
            }

            TempData["SuccessMessage"] = "Agente desactivado correctamente.";
            return RedirectToAction("Agents");
        }

        public async Task<IActionResult> DeleteAgent(string id)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<AgentDto>(id);
            if (dto == null || dto.Role != Roles.Agent)
            {
                TempData["ErrorMessage"] = "Error al eliminar el agente.";
                return RedirectToAction("Agents");
            }

            var vm = mapper.Map<DeleteAgentViewModel>(dto);
            return View("MaintenanceAgents/DeleteAgent", vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAgent(DeleteAgentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("MaintenanceAgents/DeleteAgent", vm);
            }

            await userAccountServiceForWebApp.DeleteAsync(vm.Id);

            TempData["SuccessMessage"] = "Agente eliminado correctamente.";
            return RedirectToAction("Agents");
        }

        #endregion

        #region maintenance admin
        public async Task<IActionResult> Admins()
        {
            var admins = await userAccountServiceForWebApp.GetUsersByRole<AdminDto>(Roles.Admin);
            var vm = mapper.Map<List<AdminViewModel>>(admins);
            return View("MaintenanceAdmins/Admins", vm);
        }
        public IActionResult CreateAdmin()
        {
            ViewBag.EditMode = false;
            return View("MaintenanceAdmins/CreateAdmin", new CreateAdminViewModel
            {
                Name = string.Empty,
                LastName = string.Empty,
                DocumentNumber = string.Empty,
                Email = string.Empty,
                UserName = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                Status = UserStatus.Active,
                Role = Roles.Admin
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(CreateAdminViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("MaintenanceAdmins/CreateAdmin", vm);
            }

            var createadminDto = mapper.Map<CreateAdminDto>(vm);

            var saveDto = mapper.Map<SaveUserDto>(createadminDto);
            var result = await userAccountServiceForWebApp.RegisterUser(saveDto, origin: "Admin", documentNumber: vm.DocumentNumber);

            if (result.HasError)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault() ?? "Error al crear administrador.";
                return View("MaintenanceAdmins/CreateAdmin", vm);
            }

            TempData["SuccessMessage"] = "Administrador creado correctamente.";
            return RedirectToAction("Admins");
        }


        public async Task<IActionResult> EditAdmin(string id)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<AdminDto>(id);
            if (dto == null || dto.Role != Roles.Admin)
            {
                TempData["ErrorMessage"] = "Administrador no encontrado.";
                return RedirectToAction("Admins");
            }
            var vm = mapper.Map<EditAdminViewModel>(dto);
            return View("MaintenanceAdmins/EditAdmin", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditAdmin(EditAdminViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("MaintenanceAdmins/EditAdmin", vm);
            }

            var editAdminDto = mapper.Map<EditAdminDto>(vm);
            var saveDto = mapper.Map<SaveUserDto>(editAdminDto);

            var result = await userAccountServiceForWebApp.EditUser(saveDto, origin: null, documentNumber: vm.DocumentNumber, isCreated: false);
            if (result.HasError)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault() ?? "Error al editar administrador.";
                return View("MaintenanceAdmins/EditAdmin", vm);
            }

            TempData["SuccessMessage"] = "Administrador editado correctamente.";
            return RedirectToAction("Admins");
        }

        public async Task<IActionResult> ActivateAdmin(string id)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<AdminDto>(id);
            if (dto == null || dto.Role != Roles.Admin)
            {
                TempData["ErrorMessage"] = "No se puede activar este administrador.";
                return RedirectToAction("Admins");
            }

            var vm = mapper.Map<ActivateAdminViewModel>(dto);
            return View("MaintenanceAdmins/ActivateAdmin", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ActivateAdmin(ActivateAdminViewModel vm)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<AdminDto>(vm.Id);
            if (dto == null || dto.Role != Roles.Admin)
            {
                TempData["ErrorMessage"] = "No se puede activar este administrador.";
                return RedirectToAction("Admins");
            }

            dto.Status = UserStatus.Active;

            var saveDto = mapper.Map<SaveUserDto>(dto);
            var result = await userAccountServiceForWebApp.EditUser(saveDto, origin: null, isCreated: false);

            if (result.HasError)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault() ?? "Error al activar administrador.";
                return RedirectToAction("Admins");
            }

            TempData["SuccessMessage"] = "Administrador activado correctamente.";
            return RedirectToAction("Admins");
        }

        public async Task<IActionResult> DeactivateAdmin(string id)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<AdminDto>(id);
            if (dto == null || dto.Role != Roles.Admin)
            {
                return RedirectToAction("Admins");
            }

            var vm = mapper.Map<DeactivateAdminViewModel>(dto);
            return View("MaintenanceAdmins/DeactivateAdmin", vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateAdmin(DeactivateAdminViewModel vm)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<AdminDto>(vm.Id);
            if (dto == null || dto.Role != Roles.Admin)
            {
                TempData["ErrorMessage"] = "No se puede desactivar este administrador.";
                return RedirectToAction("Admins");
            }

            dto.Status = UserStatus.Inactive;

            var saveDto = mapper.Map<SaveUserDto>(dto);
            var result = await userAccountServiceForWebApp.EditUser(saveDto, origin: null, isCreated: false);

            if (result.HasError)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault() ?? "Error al desactivar administrador.";
                return RedirectToAction("Admins");
            }

            TempData["SuccessMessage"] = "Administrador desactivado correctamente.";
            return RedirectToAction("Admins");
        }

        #endregion

        #region maintenance developer
        public async Task<IActionResult> Developers()
        {
            var developers = await userAccountServiceForWebApp.GetUsersByRole<DeveloperDto>(Roles.Developer);
            var vm = mapper.Map<List<DeveloperViewModel>>(developers);
            return View("MaintenanceDevelopers/Developers", vm);
        }

        public IActionResult CreateDeveloper()
        {
            ViewBag.EditMode = false;
            return View("MaintenanceDevelopers/CreateDeveloper", new CreateDeveloperViewModel
            {
                Name = string.Empty,
                LastName = string.Empty,
                DocumentNumber = string.Empty,
                Email = string.Empty,
                UserName = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                Status = UserStatus.Active,
                Role = Roles.Developer
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeveloper(CreateDeveloperViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("MaintenanceDevelopers/CreateDeveloper", vm);
            }

            var createDeveloperDto = mapper.Map<CreateDeveloperDto>(vm);
            var saveDto = mapper.Map<SaveUserDto>(createDeveloperDto);

            var result = await userAccountServiceForWebApp.RegisterUser(saveDto, origin: "Developer", documentNumber: vm.DocumentNumber);

            if (result.HasError)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault() ?? "Error al crear desarrollador.";
                return View("MaintenanceDevelopers/CreateDeveloper", vm);
            }

            TempData["SuccessMessage"] = "Desarrollador creado correctamente.";
            return RedirectToAction("Developers");
        }

        public async Task<IActionResult> EditDeveloper(string id)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<DeveloperDto>(id);
            if (dto == null || dto.Role != Roles.Developer)
            {
                TempData["ErrorMessage"] = "Desarrollador no encontrado.";
                return RedirectToAction("Developers");
            }

            var vm = mapper.Map<EditDeveloperViewModel>(dto);
            return View("MaintenanceDevelopers/EditDeveloper", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditDeveloper(EditDeveloperViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("MaintenanceDevelopers/EditDeveloper", vm);
            }

            var editDeveloperDto = mapper.Map<EditDeveloperDto>(vm);
            var saveDto = mapper.Map<SaveUserDto>(editDeveloperDto);

            var result = await userAccountServiceForWebApp.EditUser(saveDto, origin: null, documentNumber: vm.DocumentNumber, isCreated: false);
            if (result.HasError)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault() ?? "Error al editar desarrollador.";
                return View("MaintenanceDevelopers/EditDeveloper", vm);
            }

            TempData["SuccessMessage"] = "Desarrollador editado correctamente.";
            return RedirectToAction("Developers");
        }

        public async Task<IActionResult> ActivateDeveloper(string id)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<DeveloperDto>(id);
            if (dto == null || dto.Role != Roles.Developer)
            {
                TempData["ErrorMessage"] = "No se puede activar este desarrollador.";
                return RedirectToAction("Developers");
            }

            var vm = mapper.Map<ActivateDeveloperViewModel>(dto);
            return View("MaintenanceDevelopers/ActivateDeveloper", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ActivateDeveloper(ActivateDeveloperViewModel vm)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<DeveloperDto>(vm.Id);
            if (dto == null || dto.Role != Roles.Developer)
            {
                TempData["ErrorMessage"] = "No se puede activar este desarrollador.";
                return RedirectToAction("Developers");
            }

            dto.Status = UserStatus.Active;

            var saveDto = mapper.Map<SaveUserDto>(dto);
            var result = await userAccountServiceForWebApp.EditUser(saveDto, origin: null, isCreated: false);

            if (result.HasError)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault() ?? "Error al activar desarrollador.";
                return RedirectToAction("Developers");
            }

            TempData["SuccessMessage"] = "Desarrollador activado correctamente.";
            return RedirectToAction("Developers");
        }

        public async Task<IActionResult> DeactivateDeveloper(string id)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<DeveloperDto>(id);
            if (dto == null || dto.Role != Roles.Developer)
            {
                return RedirectToAction("Developers");
            }

            var vm = mapper.Map<DeactivateDeveloperViewModel>(dto);
            return View("MaintenanceDevelopers/DeactivateDeveloper", vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateDeveloper(DeactivateDeveloperViewModel vm)
        {
            var dto = await userAccountServiceForWebApp.GetUserById<DeveloperDto>(vm.Id);
            if (dto == null || dto.Role != Roles.Developer)
            {
                TempData["ErrorMessage"] = "No se puede desactivar este desarrollador.";
                return RedirectToAction("Developers");
            }

            dto.Status = UserStatus.Inactive;

            var saveDto = mapper.Map<SaveUserDto>(dto);
            var result = await userAccountServiceForWebApp.EditUser(saveDto, origin: null, isCreated: false);

            if (result.HasError)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault() ?? "Error al desactivar desarrollador.";
                return RedirectToAction("Developers");
            }

            TempData["SuccessMessage"] = "Desarrollador desactivado correctamente.";
            return RedirectToAction("Developers");
        }
        #endregion


    }
}