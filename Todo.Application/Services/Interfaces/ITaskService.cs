using Todo.Application.DTOs;

namespace Todo.Application.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(int userId);
        Task<TaskDto?> GetTaskByIdAsync(int id);
        Task<TaskDto> CreateTaskAsync(int userId, CreateTaskDto dto);
        Task<TaskDto?> UpdateTaskAsync(int id, int userId, UpdateTaskDto dto);
        Task<bool> DeleteTaskAsync(int id, int userId);
    }
}
