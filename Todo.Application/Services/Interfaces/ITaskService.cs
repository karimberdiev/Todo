using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.DTOs;

namespace Todo.Application.Services.Interfaces
{
    interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(int userId);
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task<TaskDto> CreateTaskAsync(int userId, CreateTaskDto dto);
        Task<TaskDto> UpdateTaskAsync(int id, int UserId, UpdateTaskDto dto);
        Task<bool> DeleteTaskAsync(int id, int UserId);
    }
}
