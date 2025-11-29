using Application.Dtos.User;
using Application.Interfaces;
using Application.ViewModels.User;
using AutoMapper;
using Azure;
using Domain.Common.Enums;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Helpers;

namespace RealEstateApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserAccountServiceForWebApp userAccountServiceForWebApp;  
        private readonly IMapper mapper;
        private readonly UserManager<UserAccount> userManager;

        public LoginController(IUserAccountServiceForWebApp userAccountServiceForWebApp, IMapper mapper, UserManager<UserAccount> userManager)
        {
            this.userAccountServiceForWebApp = userAccountServiceForWebApp;        
            this.mapper = mapper;
            this.userManager = userManager;
        }
        public async Task <IActionResult> Index()
        {
            UserAccount? userSession = await userManager.GetUserAsync(User);

            if (userSession != null)
            {       var user = await userAccountServiceForWebApp.GetUserByUserName(userSession.UserName ?? "");

                    if (user != null && user.Role == Roles.Admin)
                    {
                        return RedirectToRoute(new { controller = "Admin", action = "Index" });
                    }
                    else if (user != null && user.Role == Roles.Agent)
                    {
                        return RedirectToRoute(new { controller = "Agent", action = "Index" });
                    }
                    else if (user != null && user.Role == Roles.Customer)
                    {
                        return RedirectToRoute(new { controller = "Customer", action = "Index" });
                    }
            }

            return View(new LoginViewModel() { Password = "", UserName = "" });
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            UserAccount? userSession = await userManager.GetUserAsync(User);

            if (userSession != null)
            {
                var user = await userAccountServiceForWebApp.GetUserByUserName(userSession.UserName ?? "");

                if (user != null && user.Role == Roles.Admin)
                {
                    return RedirectToRoute(new { controller = "Admin", action = "Index" });
                }
                else if (user != null && user.Role == Roles.Agent)
                {
                    return RedirectToRoute(new { controller = "Agent", action = "Index" });
                }
                else if (user != null && user.Role == Roles.Customer)
                {
                    return RedirectToRoute(new { controller = "Customer", action = "Index" });
                }

            }

            if (!ModelState.IsValid)
            {
                vm.Password = "";
                return View(vm);
            }

            LoginResponseDto? userDto = await userAccountServiceForWebApp.AuthenticateAsync(new LoginDto()
            {
                Password = vm.Password,
                UserName = vm.UserName
            });

            if (userDto != null && !userDto.HasError)
            {

                var role = userDto.Roles?.FirstOrDefault();

                if (role == Roles.Admin.ToString())
                    return RedirectToRoute(new { controller = "Admin", action = "Index" });

                if (role == Roles.Agent.ToString())
                    return RedirectToRoute(new { controller = "Agent", action = "Index" });

                if (role == Roles.Customer.ToString())
                    return RedirectToRoute(new { controller = "Customer", action = "Index" });

                ModelState.AddModelError("userValidation", "Rol no reconocido.");
            }
            else
            {
                foreach(var error in userDto?.Errors ?? [])
                {
                    ModelState.AddModelError("userValidation", error);
                }                
            }

            vm.Password = "";
            return View(vm);
        }
        public async Task<IActionResult> Logout()
        {
            await userAccountServiceForWebApp.SignOutAsync();
            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel()
            {
                ConfirmPassword = "",
                Email = "",
                LastName = "",
                Name = "",
                Password = "",
                UserName = "",
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            SaveUserDto dto = mapper.Map<SaveUserDto>(vm);
            string origin = Request?.Headers?.Origin.ToString() ?? string.Empty;

            RegisterResponseDto? returnUser = await userAccountServiceForWebApp.RegisterUser(dto, origin);

            if (returnUser.HasError)
            {
                ViewBag.HasError = true;
                ViewBag.Errors = returnUser.Errors;
                return View(vm);
            }

            if (returnUser != null && !string.IsNullOrWhiteSpace(returnUser.Id))
            {
                dto.Id = returnUser.Id;
                dto.ProfileImage = FileManager.Upload(vm.ProfileImageFile, dto.Id, "Users");
                await userAccountServiceForWebApp.EditUser(dto, origin, true);
            }

            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }

        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            UserResponseDto response = await userAccountServiceForWebApp.ConfirmAccountAsync(userId, token);
            return View("ConfirmEmail", response.Message);
        }

        public IActionResult ForgotPassword()
        {           
            return View(new ForgotPasswordRequestViewModel() {UserName = ""});
        }        

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            string origin = Request?.Headers?.Origin.ToString() ?? string.Empty;

            ForgotPasswordRequestDto dto = new() { UserName = vm.UserName,Origin = origin};

            UserResponseDto? returnUser = await userAccountServiceForWebApp.ForgotPasswordAsync(dto);

            if (returnUser.HasError)
            {
                ViewBag.HasError = true;
                ViewBag.Errors = returnUser.Errors;
                return View(vm);
            }          

            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }
        public IActionResult ResetPassword(string userId, string token)
        {           
            return View(new ResetPasswordRequestViewModel() { Id = userId,Token = token,Password = ""});
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }  

            ResetPasswordRequestDto dto = new() { UserId = vm.Id ,Password = vm.Password,Token = vm.Token };

            UserResponseDto? returnUser = await userAccountServiceForWebApp.ResetPasswordAsync(dto);

            if (returnUser.HasError)
            {
                ViewBag.HasError = true;
                ViewBag.Errors = returnUser.Errors;
                return View(vm);
            }

            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }

        public async Task<IActionResult> AccessDenied()
        {
            UserAccount? userSession = await userManager.GetUserAsync(User);

            if (userSession != null)
            {
                var user = await userAccountServiceForWebApp.GetUserByUserName(userSession.UserName ?? "");
                ViewBag.CurrentRol = user?.Role.ToString() ?? "";
                return View();
            }          

            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }
    }
}
