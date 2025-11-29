using Application.Dtos.User;

namespace Application.Interfaces
{
    public interface IUserAccountServiceForWebApi : IBaseAccountService
    {
        Task<LoginResponseForApiDto> AuthenticateAsync(LoginDto loginDto);
    }
}