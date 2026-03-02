namespace DemoEFCore.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;

    // Propriété de navigation
    public UserDetails? Details { get; set; }
}

public class UserDetails
{
    public Guid Id { get; set; }
    public string? Lastname { get; set; }
    public string? Firstname { get; set; }
    public Guid UserId { get; set; } // Clef étrangère
}
