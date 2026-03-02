namespace DemoEFCore.Entities;

public class Actor
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public IEnumerable<Film> Films { get; set; } = [];
}
