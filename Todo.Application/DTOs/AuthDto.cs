using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.DTOs
{
    public record LoginDto ( string Username, string Password);
    public record RegisterDto(string Username, string Password, string Email);
    public record AuthResultDto(bool Success, string? Token, string? Message);
    public record UserDto(int Id, string Username, string Email, DateTime CreatedAt);
}
