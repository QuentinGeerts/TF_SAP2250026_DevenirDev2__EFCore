namespace DemoEFCore.Entities;

public class FilmActor
{
    public Guid FilmId { get; set; }
    public Guid ActorId { get; set; }

    public string CharacterName { get; set; } = null!;

    // Propriétés de navigations

    public Film Film { get; set; } = null!;
    public Actor Actor { get; set; } = null!;
}
