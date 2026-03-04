using System.ComponentModel.DataAnnotations;

namespace Todo.AuthMVC.ViewModels;

public class TodoItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = "Todo";
}

public class CreateTodoViewModel
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(1);
}

public class HomeIndexViewModel
{
    public List<TodoItemViewModel> Items { get; set; } = new();
    public CreateTodoViewModel CreateTodo { get; set; } = new();
}
