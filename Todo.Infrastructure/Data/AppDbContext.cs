using Todo.Domain.Entities;

namespace Todo.Infrastructure.Data
{
    public class AppDbContext
    {
        public List<User> Users { get; } = new();
        public List<TaskItem> Tasks { get; } = new();

        public int UserSequence { get; set; }
        public int TaskSequence { get; set; }
    }
}
