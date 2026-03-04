using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Enums;

namespace Todo.Application.DTOs
{
    public record TaskDto(
        int Id,
        string Title,
        string? Description,
        TaskItemStatus Status,
        DateTime CreatedAt,
        DateTime DueDate,
        int UserId);
    public record CreateTaskDto(
        string Title,
        string? Description,
        DateTime DueDate);
    public record UpdateTaskDto(
        string Title,
        string? Description,
        TaskItemStatus Status,
        DateTime DueDate);
}
