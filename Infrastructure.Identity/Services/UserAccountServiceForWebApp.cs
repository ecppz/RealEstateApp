using Application.Dtos.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.Enums;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services
{
    public class UserAccountServiceForWebApp : BaseAccountService, IUserAccountServiceForWebApp
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly IMapper _mapper;
        public UserAccountServiceForWebApp(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager, IEmailService emailService, IMapper mapper)
            : base(userManager,emailService, mapper) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        public async Task<LoginResponseDto> AuthenticateAsync(LoginDto loginDto)
        {
            LoginResponseDto response = new()
            {
                Email = "",
                Id = "",
                LastName = "",
                Name = "",
                UserName = "",
                Status = UserStatus.Inactive,
                HasError = false,
                Errors = []
            };

            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"There is no acccount registered with this username: {loginDto.UserName}");
                return response;
            }

            var rolesList = await _userManager.GetRolesAsync(user);
            var role = rolesList.FirstOrDefault();

            if (role == Roles.Customer.ToString())
            {
                if (!user.EmailConfirmed)
                {
                    response.HasError = true;
                    response.Errors.Add($"Esta cuenta de cliente '{loginDto.UserName}' no está activa, por favor revisa tu email.");
                    return response;
                }
            }
            else if (role == Roles.Customer.ToString())
            {
                if (user.Status != UserStatus.Active)
                {
                    response.HasError = true;
                    response.Errors.Add($"'{loginDto.UserName}' el admin te desactivo por obvias razones comunicate con el");
                    return response;
                }
            }
            else if (role == Roles.Agent.ToString())
            {
                if (user.Status != UserStatus.Active)
                {
                    response.HasError = true;
                    response.Errors.Add($"Este cuenta de agente '{loginDto.UserName}' no está activa, por favor contacta con un admin.");
                    return response;
                }
            }
            else if (role == Roles.Admin.ToString())
            {
                if (user.Status != UserStatus.Active)
                {
                    response.HasError = true;
                    response.Errors.Add($"Este cuenta de administrador '{loginDto.UserName}' no está activa, por favor contacta con el admin principal.");
                    return response;
                }
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName ?? "", loginDto.Password, false, true);

            if (!result.Succeeded)
            {
                response.HasError = true;
                if (result.IsLockedOut)
                {
                    response.Errors.Add($"Your account {loginDto.UserName} has been locked due to multiple failed attempts." +
                        $" Please try again in 10 minutes. If you don’t remember your password, you can go through the password " +
                        $"reset process.");
                }
                else
                {
                    response.Errors.Add($"these credentials are invalid for this user: {user.UserName}");
                }                    
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email ?? "";
            response.UserName = user.UserName ?? "";
            response.Name = user.Name;
            response.LastName = user.LastName;
            response.IsVerified = user.EmailConfirmed;
            response.Roles = rolesList.ToList();
            response.Status = user.Status;
            return response;
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }       

    }
}
