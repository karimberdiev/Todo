using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.DTOs;
using Todo.Application.Services.Interfaces;

namespace Todo.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMine()
        {
            var userId = GetUserId();
            if (userId is null) return Unauthorized();
            var items = await _taskService.GetTasksByUserIdAsync(userId.Value);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var userId = GetUserId();
            if (userId is null) return Unauthorized();
            var item = await _taskService.CreateTaskAsync(userId.Value, dto);
            return Ok(item);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            if (userId is null) return Unauthorized();
            var deleted = await _taskService.DeleteTaskAsync(id, userId.Value);
            return deleted ? NoContent() : NotFound();
        }

        private int? GetUserId()
        {
            var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(raw, out var id) ? id : null;
        }
    }
}
