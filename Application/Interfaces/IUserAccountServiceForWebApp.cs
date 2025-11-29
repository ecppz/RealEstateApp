using Application.Dtos.User;

namespace Application.Interfaces
{
    public interface IUserAccountServiceForWebApp : IBaseAccountService
    {
        Task<LoginResponseDto> AuthenticateAsync(LoginDto loginDto); 
        Task SignOutAsync();
    }
}