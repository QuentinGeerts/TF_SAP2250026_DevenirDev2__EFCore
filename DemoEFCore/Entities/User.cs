namespace DemoEFCore.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string? Lastname { get; set; }
    public string? Firstname { get; set; }

}
