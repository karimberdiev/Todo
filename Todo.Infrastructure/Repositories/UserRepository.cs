using Todo.Domain.Entities;
using Todo.Domain.Interfaces;
using Todo.Infrastructure.Data;

namespace Todo.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<User?> GetByIdAsync(int id)
        {
            lock (_context)
            {
                return Task.FromResult(_context.Users.FirstOrDefault(u => u.Id == id));
            }
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            lock (_context)
            {
                return Task.FromResult(_context.Users.FirstOrDefault(u =>
                    string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)));
            }
        }

        public Task AddAsync(User user)
        {
            lock (_context)
            {
                user.Id = ++_context.UserSequence;
                _context.Users.Add(user);
            }

            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync(User user)
        {
            lock (_context)
            {
                _context.Users.Remove(user);
            }

            return Task.CompletedTask;
        }
    }
}
