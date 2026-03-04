using Todo.Application.DTOs;
using Todo.Application.Services.Interfaces;
using Todo.Domain.Entities;
using Todo.Domain.Interfaces;

namespace Todo.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly Func<string, string> _generateToken;

        public AuthService(IUnitOfWork uow, Func<string, string> generateToken)
        {
            _uow = uow;
            _generateToken = generateToken;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _uow.Users.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
            {
                return new AuthResultDto(false, null, "Username allaqachon mavjud.");
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password
            };

            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync();

            var token = _generateToken(user.Id.ToString());
            return new AuthResultDto(true, token, "Ro'yxatdan o'tish muvaffaqiyatli.");
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            var user = await _uow.Users.GetByUsernameAsync(dto.Username);
            if (user == null || user.Password != dto.Password)
            {
                return new AuthResultDto(false, null, "Username yoki parol xato.");
            }

            var token = _generateToken(user.Id.ToString());
            return new AuthResultDto(true, token, "Login muvaffaqiyatli.");
        }

        public async Task<UserDto?> GetUserById(int id)
        {
            var user = await _uow.Users.GetByIdAsync(id);
            return user == null ? null : new UserDto(user.Id, user.Username, user.Email, user.CreatedAt);
        }
    }
}
