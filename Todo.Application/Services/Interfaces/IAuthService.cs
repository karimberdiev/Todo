using Todo.Application.DTOs;

namespace Todo.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto dto);
        Task<AuthResultDto> LoginAsync(LoginDto dto);
        Task<UserDto?> GetUserById(int id);
    }
}
