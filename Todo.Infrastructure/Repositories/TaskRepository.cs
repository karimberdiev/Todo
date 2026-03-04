using Todo.Domain.Entities;
using Todo.Domain.Interfaces;
using Todo.Infrastructure.Data;

namespace Todo.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<TaskItem?> GetByIdAsync(int id)
        {
            lock (_context)
            {
                return Task.FromResult(_context.Tasks.FirstOrDefault(t => t.Id == id));
            }
        }

        public Task<IEnumerable<TaskItem>> GetByUserIdAsync(int userId)
        {
            lock (_context)
            {
                var items = _context.Tasks
                    .Where(t => t.UserId == userId)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToList();
                return Task.FromResult<IEnumerable<TaskItem>>(items);
            }
        }

        public Task AddAsync(TaskItem task)
        {
            lock (_context)
            {
                task.Id = ++_context.TaskSequence;
                _context.Tasks.Add(task);
            }

            return Task.CompletedTask;
        }

        public void Update(TaskItem task)
        {
        }

        public void Delete(TaskItem task)
        {
            lock (_context)
            {
                _context.Tasks.Remove(task);
            }
        }
    }
}
