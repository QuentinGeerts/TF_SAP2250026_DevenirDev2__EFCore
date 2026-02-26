namespace DemoEFCore.Entities;

public class Film
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int ReleasedYear { get; set; }
    public DateTime? CreatedAt { get; set; }
}
