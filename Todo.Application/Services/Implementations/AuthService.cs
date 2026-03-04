
using Todo.Application.DTOs;
using Todo.Domain.Entities;
using Todo.Domain.Interfaces;

namespace Todo.Application.Services.Interfaces
{
    class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly Func<string, string> _generateToken;
        public AuthService(IUnitOfWork uow, Func<string,string> g) 
        {
            _uow = uow;
            _generateToken = g;
        }
        public async Task<AuthResultDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _uow.Users.GetByUsernameAsync(dto.Username);
            if (existingUser != null )
                return new AuthResultDto ( false, null, "bunday username mavjud" );
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password
            };
            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync();
            var token = _generateToken(user.Id.ToString());
            return new AuthResultDto (  true, token,"muvaffaqiyatli royxatdan otdi" );
        }
        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            var user = await _uow.Users.GetByUsernameAsync(dto.Username);
            if (user == null || user.Password != dto.Password)
                return new AuthResultDto(false, null, "username yoki password xato");
            var token = _generateToken(user.Id.ToString());
            return new AuthResultDto(true, token, "muvaffaqiyatli login");
        }
        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _uow.Users.GetByIdAsync(id);
            if (user == null)
                return null;
            return new UserDto
            (
                user.Id,
                user.Username,
                user.Email,
                user.CreatedAt
            );
        }
    }
}
