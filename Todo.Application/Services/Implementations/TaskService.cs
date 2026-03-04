using Todo.Application.DTOs;
using Todo.Application.Services.Interfaces;
using Todo.Domain.Entities;
using Todo.Domain.Interfaces;

namespace Todo.Application.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _uow;

        public TaskService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(int userId)
        {
            var tasks = await _uow.Tasks.GetByUserIdAsync(userId);
            return tasks.Select(ToDto);
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            var task = await _uow.Tasks.GetByIdAsync(id);
            return task == null ? null : ToDto(task);
        }

        public async Task<TaskDto> CreateTaskAsync(int userId, CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                UserId = userId
            };

            await _uow.Tasks.AddAsync(task);
            await _uow.SaveChangesAsync();
            return ToDto(task);
        }

        public async Task<TaskDto?> UpdateTaskAsync(int id, int userId, UpdateTaskDto dto)
        {
            var task = await _uow.Tasks.GetByIdAsync(id);
            if (task == null || task.UserId != userId)
            {
                return null;
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.DueDate = dto.DueDate;
            _uow.Tasks.Update(task);
            await _uow.SaveChangesAsync();
            return ToDto(task);
        }

        public async Task<bool> DeleteTaskAsync(int id, int userId)
        {
            var task = await _uow.Tasks.GetByIdAsync(id);
            if (task == null || task.UserId != userId)
            {
                return false;
            }

            _uow.Tasks.Delete(task);
            await _uow.SaveChangesAsync();
            return true;
        }

        private static TaskDto ToDto(TaskItem task) =>
            new(task.Id, task.Title, task.Description, task.Status, task.CreatedAt, task.DueDate, task.UserId);
    }
}
