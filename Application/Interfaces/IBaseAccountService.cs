using Application.Dtos.User;
using Domain.Common.Enums;

namespace Application.Interfaces
{
    public interface IBaseAccountService
    {
        Task<UserResponseDto> ConfirmAccountAsync(string userId, string token);
        Task<UserResponseDto> DeleteAsync(string id);
        Task<EditResponseDto> EditUser(SaveUserDto saveDto, string? origin, string? documentNumber = null, bool? isCreated = false, bool? isApi = false);
        Task<UserResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request, bool? isApi = false);
        Task<List<UserDto>> GetAllUser(bool? isActive);
        Task<List<TDto>> GetUsersByRole<TDto>(Roles role) where TDto : class;
        Task<UserDto?> GetUserByEmail(string email);
        Task<TDto?> GetUserById<TDto>(string id) where TDto : class;
        Task<UserDto?> GetUserByUserName(string userName);
        Task<RegisterResponseDto> RegisterUser(SaveUserDto saveDto, string? origin, string? documentNumber = null, bool? isApi = false);
        Task<UserResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request);
    }
}