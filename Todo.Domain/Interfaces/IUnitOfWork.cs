namespace Todo.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskRepository Tasks { get; }
        IUserRepository Users { get; }
        Task SaveChangesAsync();
    }
}
