using Todo.Domain.Interfaces;
using Todo.Infrastructure.Data;

namespace Todo.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public ITaskRepository Tasks { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(AppDbContext context)
        {
            Tasks = new TaskRepository(context);
            Users = new UserRepository(context);
        }

        public Task SaveChangesAsync() => Task.CompletedTask;

        public void Dispose()
        {
        }
    }
}
