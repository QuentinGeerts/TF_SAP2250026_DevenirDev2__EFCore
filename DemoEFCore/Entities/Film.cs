namespace DemoEFCore.Entities;

public class Film
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int ReleasedYear { get; set; }
    public DateTime? CreatedAt { get; set; }

    public Guid DirectorId { get; set; } // Foreign Key

    // Propriété de navigation
    public Director Director { get; set; } = null!;
    public IEnumerable<Actor> Actors { get; set; } = [];
}
