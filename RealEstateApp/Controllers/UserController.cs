using Application.Dtos.User;
using Application.Interfaces;
using Application.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Helpers;

namespace RealEstateApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserAccountServiceForWebApp userAccountServiceForWebApp;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(IUserAccountServiceForWebApp userAccountServiceForWebApp, RoleManager<IdentityRole> roleManager)
        {
            this.userAccountServiceForWebApp = userAccountServiceForWebApp;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var dtos = await userAccountServiceForWebApp.GetAllUser(false);

            var listEntityVms = dtos.Select(s =>
              new UserViewModel()
              {
                  Id = s.Id,
                  Name = s.Name,
                  Email = s.Email,
                  UserName = s.UserName,
                  LastName = s.LastName,
                  Role = s.Role,
                  Phone = s.Phone,
                  ProfileImage = s.ProfileImage
              }).ToList();

            return View(listEntityVms);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await roleManager.Roles.ToListAsync();
            return View(new CreateUserViewModel() { Id = 0, Name = "", Email = "", UserName = "", LastName = "", Password = "", ConfirmPassword = ""});
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await roleManager.Roles.ToListAsync();
                return View(vm);
            }

            string origin = Request?.Headers?.Origin.ToString() ?? string.Empty;

            SaveUserDto dto = new()
            {
                Id = "",
                Name = vm.Name,
                Email = vm.Email,
                UserName = vm.UserName,
                LastName = vm.LastName,
                Password = vm.Password,
                Role = vm.Role,
                Phone = vm.Phone,
                ProfileImage = ""
            };

            RegisterResponseDto? returnUser = await userAccountServiceForWebApp.RegisterUser(dto, origin);

            if (returnUser.HasError)
            {
                ViewBag.Roles = await roleManager.Roles.ToListAsync();
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

            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            ViewBag.EditMode = true;
            var dto = await userAccountServiceForWebApp.GetUserById(id);

            if (dto == null)
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            UpdateUserViewModel vm = new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                UserName = dto.UserName,
                LastName = dto.LastName,
                Password = "",
                Role = dto.Role,
                Phone = dto.Phone,
            };

            ViewBag.Roles = await roleManager.Roles.ToListAsync();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await roleManager.Roles.ToListAsync();
                ViewBag.EditMode = true;
                return View(vm);
            }

            string origin = Request?.Headers?.Origin.ToString() ?? string.Empty;

            SaveUserDto dto = new()
            {
                Id = vm.Id,
                Name = vm.Name,
                Email = vm.Email,
                UserName = vm.UserName,
                LastName = vm.LastName,
                Password = vm.Password ?? "",
                Role = vm.Role,
                Phone = vm.Phone,
            };

            var currentDto = await userAccountServiceForWebApp.GetUserById(vm.Id);
            string? currentImagePath = "";

            if (currentDto != null)
            {
                currentImagePath = currentDto.ProfileImage;
            }

            dto.ProfileImage = FileManager.Upload(vm.ProfileImageFile, dto.Id, "Users", true, currentImagePath);
            var returnUser = await userAccountServiceForWebApp.EditUser(dto, origin);
            if (returnUser.HasError)
            {
                ViewBag.Roles = await roleManager.Roles.ToListAsync();
                ViewBag.EditMode = true;
                ViewBag.HasError = true;
                ViewBag.Errors = returnUser.Errors;
                return View(vm);
            }

            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            var dto = await userAccountServiceForWebApp.GetUserById(id);
            if (dto == null)
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }
            DeleteUserViewModel vm = new() { Id = dto.Id, Name = dto.Name, LastName = dto.LastName };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await userAccountServiceForWebApp.DeleteAsync(vm.Id);
            FileManager.Delete(vm.Id, "Users");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
    }
}
