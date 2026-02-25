namespace DemoEFCore.Entities;

public class Director
{
    public Guid Id { get; set; }
    public string Lastname { get; set; } = null!;
    public string Firstname { get; set; } = null!;
}
