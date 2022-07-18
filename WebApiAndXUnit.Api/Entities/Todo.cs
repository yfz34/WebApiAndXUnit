namespace WebApiAndXUnit.Api.Entities;

public class Todo
{
    public int Id { get; set; }
    public string? ItemName { get; set; }
    public bool IsCompleted { get; set; }
}