using Application.Dtos.Agent;
using Application.Dtos.User;
using Application.Interfaces;
using Application.ViewModels.Agent;
using AutoMapper;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace RealEstateApp.Controllers
{
    [Authorize(Roles = "Agent")]
    public class AgentController : Controller
    {
        private readonly IBaseAccountService baseAccountService;
        private readonly IMapper mapper;
        private readonly UserManager<UserAccount> userManager;

        public AgentController(IBaseAccountService baseAccountService, IMapper mapper, UserManager<UserAccount> userManager)
        {
            this.baseAccountService = baseAccountService;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

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


    }
}
