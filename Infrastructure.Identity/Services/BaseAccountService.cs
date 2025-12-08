using Application.Dtos.Admin;
using Application.Dtos.Agent;
using Application.Dtos.Developer;
using Application.Dtos.Email;
using Application.Dtos.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.Enums;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Infrastructure.Identity.Services
{
    public class BaseAccountService : IBaseAccountService
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public BaseAccountService(UserManager<UserAccount> userManager, IEmailService emailService, IMapper mapper)
        { 
            _userManager = userManager;
            _emailService = emailService;
            _mapper = mapper;
        }

        public virtual async Task<RegisterResponseDto> RegisterUser(SaveUserDto saveDto, string? origin, string? documentNumber = null, bool ? isApi = false)
        {
            RegisterResponseDto response = new()
            {
                Email = "",
                Id = "",
                LastName = "",
                Name = "",
                UserName = "",
                HasError = false,
                Errors = []
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(saveDto.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Errors.Add($"this username: {saveDto.UserName} is already taken.");
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(saveDto.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Errors.Add($"this email: {saveDto.Email} is already taken.");
                return response;
            }

            UserAccount user = new UserAccount()
            {
                Name = saveDto.Name,
                LastName = saveDto.LastName,
                Email = saveDto.Email,
                UserName = saveDto.UserName,
                ProfileImage = saveDto.ProfileImage,
                PhoneNumber = saveDto.PhoneNumber,
                Status = UserStatus.Inactive,
                EmailConfirmed = false,       
                DocumentNumber = null   
            };

            switch (saveDto.Role)
            {
                case Roles.Admin:
                case Roles.Developer:
                    user.Status = UserStatus.Active;
                    user.DocumentNumber = documentNumber;
                    user.EmailConfirmed = true;
                    break;

                case Roles.Customer:
                    user.Status = UserStatus.Inactive;
                    user.EmailConfirmed = false;
                    user.DocumentNumber = null;
                    break;

                case Roles.Agent:
                    user.Status = UserStatus.Inactive;
                    user.EmailConfirmed = true;
                    user.DocumentNumber = null;
                    break;
            }

            var result = await _userManager.CreateAsync(user, saveDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, saveDto.Role.ToString());

                if (saveDto.Role == Roles.Customer)
                {
                    if (isApi != null && !isApi.Value)
                    {
                        string verificationUri = await GetVerificationEmailUri(user, origin ?? "");
                        await _emailService.SendAsync(new EmailRequestDto()
                        {
                            To = saveDto.Email,
                            HtmlBody = $"Por favor confirma tu cuenta visitando esta URL <a href='{verificationUri}'> Clic aqui </a>",
                            Subject = "Confirm registration"
                        });
                    }
                    else
                    {
                        string? verificationToken = await GetVerificationEmailToken(user);
                        await _emailService.SendAsync(new EmailRequestDto()
                        {
                            To = saveDto.Email,
                            HtmlBody = $"Please confirm your account use this token {verificationToken}",
                            Subject = "Confirm registration"
                        });
                    }
                }

                var rolesList = await _userManager.GetRolesAsync(user);

                response.Id = user.Id;
                response.Email = user.Email ?? "";
                response.UserName = user.UserName ?? "";
                response.Name = user.Name;
                response.LastName = user.LastName;
                response.IsVerified = user.EmailConfirmed;
                response.Roles = rolesList.ToList();

                return response;
            }
            else
            {
                response.HasError = true;
                response.Errors.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;
            }
        }

        public virtual async Task<EditResponseDto> EditUser(SaveUserDto saveDto, string? origin, string? documentNumber = null, bool? isCreated = false, bool? isApi = false)
        {
            bool isNotcreated = !isCreated ?? false;
            EditResponseDto response = new()
            {
                Email = "",
                Id = "",
                LastName = "",
                Name = "",
                UserName = "",
                HasError = false,
                Errors = []
            };

            var userWithSameUserName = await _userManager.Users.FirstOrDefaultAsync(w => w.UserName == saveDto.UserName && w.Id != saveDto.Id);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Errors.Add($"this username: {saveDto.UserName} is already taken.");
                return response;
            }

            var userWithSameEmail = await _userManager.Users.FirstOrDefaultAsync(w => w.Email == saveDto.Email && w.Id != saveDto.Id);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Errors.Add($"this email: {saveDto.Email} is already taken.");
                return response;
            }

            var user = await _userManager.FindByIdAsync(saveDto.Id);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"There is no acccount registered with this user");
                return response;
            }

            user.Name = saveDto.Name;
            user.LastName = saveDto.LastName;
            user.UserName = saveDto.UserName;
            user.ProfileImage = string.IsNullOrWhiteSpace(saveDto.ProfileImage) ? user.ProfileImage : saveDto.ProfileImage;
            user.EmailConfirmed = user.EmailConfirmed && user.Email == saveDto.Email;
            user.Email = saveDto.Email;
            user.PhoneNumber = saveDto.PhoneNumber;
            user.DocumentNumber = documentNumber;
            user.Status = saveDto.Status;

            if (!string.IsNullOrWhiteSpace(saveDto.Password) && isNotcreated)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resultChange = await _userManager.ResetPasswordAsync(user, token, saveDto.Password);

                if (resultChange != null && !resultChange.Succeeded)
                {
                    response.HasError = true;
                    response.Errors.AddRange(resultChange.Errors.Select(s => s.Description).ToList());
                    return response;
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var rolesList = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, rolesList.ToList());

                await _userManager.AddToRoleAsync(user, saveDto.Role.ToString());


                if (!user.EmailConfirmed && isNotcreated)
                {
                    if (isApi != null && !isApi.Value)
                    {
                        string verificationUri = await GetVerificationEmailUri(user, origin ?? "");
                        await _emailService.SendAsync(new EmailRequestDto()
                        {
                            To = saveDto.Email,
                            HtmlBody = $"Por favor confirma tu cuenta visitando esta URL <a href='{verificationUri}'> Clic aqui </a>",
                            Subject = "Confirmr registro"
                        });
                    }
                    else
                    {
                        string? verificationToken = await GetVerificationEmailToken(user);
                        await _emailService.SendAsync(new EmailRequestDto()
                        {
                            To = saveDto.Email,
                            HtmlBody = $"Por favor confirma tu cuenta usando este token {verificationToken}",
                            Subject = "Confirmr registro"
                        });
                    }
                }

                var updatedRolesList = await _userManager.GetRolesAsync(user);

                response.Id = user.Id;
                response.Email = user.Email ?? "";
                response.UserName = user.UserName ?? "";
                response.Name = user.Name;
                response.LastName = user.LastName;
                response.IsVerified = user.EmailConfirmed;
                response.Roles = updatedRolesList.ToList();

                return response;
            }
            else
            {
                response.HasError = true;
                response.Errors.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;
            }
        }

        public virtual async Task<UserResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request, bool? isApi = false)
        {
            UserResponseDto response = new() { HasError = false, Errors = [] };

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"There is no acccount registered with this username {request.UserName}");
                return response;
            }

            user.EmailConfirmed = false;
            await _userManager.UpdateAsync(user);

            if (isApi != null && !isApi.Value)
            {
                var resetUri = await GetResetPasswordUri(user, request.Origin ?? "");
                await _emailService.SendAsync(new EmailRequestDto()
                {
                    To = user.Email,
                    HtmlBody = $"Por favor resetea tu password visitango esta URL <a href='{resetUri}'> Clic aqui </a>",
                    Subject = "Reset password"
                });
            }
            else
            {
                string? resetToken = await GetResetPasswordToken(user);
                await _emailService.SendAsync(new EmailRequestDto()
                {
                    To = user.Email,
                    HtmlBody = $"Please reset your password account use this token {resetToken}",
                    Subject = "Reset password"
                });
            }

            return response;
        }

        public virtual async Task<UserResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            UserResponseDto response = new() { HasError = false, Errors = [] };

            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"There is no acccount registered with this user");
                return response;
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Errors.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;
            }

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return response;
        }
        public virtual async Task<UserResponseDto> DeleteAsync(string id)
        {
            UserResponseDto response = new() { HasError = false, Errors = [] };
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"There is no acccount registered with this user");
                return response;
            }

            await _userManager.DeleteAsync(user);

            return response;
        }
        public virtual async Task<UserDto?> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {
                Id = user.Id,
                Email = user.Email ?? "",
                LastName = user.LastName,
                Name = user.Name,
                UserName = user.UserName ?? "",
                ProfileImage = user.ProfileImage,
                PhoneNumber = user.PhoneNumber,
                isVerified = user.EmailConfirmed,
                Role = Enum.Parse<Roles>(rolesList.FirstOrDefault()!),
                Status = user.Status

            };

            return userDto;
        }
        public virtual async Task<TDto?> GetUserById<TDto>(string id) where TDto : class
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            var rolesList = await _userManager.GetRolesAsync(user);
            var roleName = rolesList.FirstOrDefault();

            var dto = _mapper.Map<TDto>(user);

            if (dto is UserDto userDto)
                userDto.Role = Enum.Parse<Roles>(roleName!);
            else if (dto is AdminDto adminDto)
                adminDto.Role = Enum.Parse<Roles>(roleName!);
            else if (dto is AgentDto agentDto)
                agentDto.Role = Enum.Parse<Roles>(roleName!);
            else if (dto is DeveloperDto developerDto)
                developerDto.Role = Enum.Parse<Roles>(roleName!);

            return dto;
        }

        public virtual async Task<UserDto?> GetUserByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return null;
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {
                Id = user.Id,
                Email = user.Email ?? "",
                LastName = user.LastName,
                Name = user.Name,
                UserName = user.UserName ?? "",
                ProfileImage = user.ProfileImage,
                PhoneNumber = user.PhoneNumber,
                isVerified = user.EmailConfirmed,
                Role = Enum.Parse<Roles>(rolesList.FirstOrDefault()!),
                Status = user.Status
            };

            return userDto;
        }
        public virtual async Task<List<TDto>> GetAllUsers<TDto>(
            bool? isActive = null,
            Roles? role = null) where TDto : class
        {
            var query = _userManager.Users.AsQueryable();

            if (isActive.HasValue)
            {
                query = isActive.Value
                    ? query.Where(u => u.Status == UserStatus.Active)
                    : query.Where(u => u.Status == UserStatus.Inactive);
            }

            query = query.OrderBy(u => u.Name);

            var users = await query.ToListAsync();
            var result = new List<TDto>();

            foreach (var user in users)
            {
                var roleList = await _userManager.GetRolesAsync(user);
                var roleName = roleList.FirstOrDefault() ?? Roles.Customer.ToString();
                var parsedRole = Enum.Parse<Roles>(roleName);

                if (!role.HasValue || parsedRole == role.Value)
                {
                    var dto = _mapper.Map<TDto>(user);
                    result.Add(dto);
                }
            }

            return result;
        }



        public async Task<List<TDto>> GetUsersByRole<TDto>(Roles role) where TDto : class
        {
            var users = _userManager.Users.AsQueryable();
            var filteredUsers = new List<TDto>();

            foreach (var user in await users.ToListAsync())
            {
                var roleList = await _userManager.GetRolesAsync(user);
                var roleName = roleList.FirstOrDefault();

                if (!string.IsNullOrEmpty(roleName) && Enum.Parse<Roles>(roleName) == role)
                {
                    var dto = _mapper.Map<TDto>(user);
                    filteredUsers.Add(dto);
                }
            }

            return filteredUsers;
        }

        public virtual async Task<UserResponseDto> ConfirmAccountAsync(string userId, string token)
        {
            UserResponseDto response = new() { HasError = false, Errors = [] };

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.Message = "There is no acccount registered with this user";
                response.HasError = true;
                return response;
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                response.Message = $"Account confirmed for {user.Email}. You can now use the app";
                response.HasError = false;
                return response;                
            }
            else
            {
                response.Message = $"An error occurred while confirming this email {user.Email}";
                response.HasError = true;
                return response;                
            }
        }

        #region "Protected methods"

        protected async Task<string> GetVerificationEmailUri(UserAccount user, string origin)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "Login/ConfirmEmail";
            var completeUrl = new Uri(string.Concat(origin, "/", route));// origin = https://localhost:58296 route=Login/ConfirmEmail
            var verificationUri = QueryHelpers.AddQueryString(completeUrl.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri.ToString(), "token", token);

            return verificationUri;
        }

        protected async Task<string?> GetVerificationEmailToken(UserAccount user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return token;
        }
        protected async Task<string> GetResetPasswordUri(UserAccount user, string origin)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "Login/ResetPassword";
            var completeUrl = new Uri(string.Concat(origin, "/", route));// origin = https://localhost:58296 route=Login/ConfirmEmail
            var resetUri = QueryHelpers.AddQueryString(completeUrl.ToString(), "userId", user.Id);
            resetUri = QueryHelpers.AddQueryString(resetUri.ToString(), "token", token);

            return resetUri;
        }

        protected async Task<string?> GetResetPasswordToken(UserAccount user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return token;
        }
        #endregion
    }
}
