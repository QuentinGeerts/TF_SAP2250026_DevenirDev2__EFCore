


using DemoEFCore.Entities;

namespace DemoEFCore;

public static class Menu
{
    public static void AfficherMenu(DataContext context)
    {
        bool continuer = true;
        while (continuer)
        {
            Console.Clear();
            Console.WriteLine($"1. Démonstrations sur le CRUD");
            Console.WriteLine($"0. Quitter");
            Console.WriteLine();
            Console.Write("Entrez votre choix: ");

            string choix = Console.ReadLine() ?? "";

            switch (choix)
            {
                case "1":
                    MenuCRUD(context);
                    break;

                case "0":
                    continuer = false;
                    break;

                default:
                    Console.WriteLine($"Choix invalid. Appuyez sur Enter pour continuer.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void MenuCRUD(DataContext context)
    {
        bool continuer = true;
        while (continuer)
        {
            Console.Clear();
            Console.WriteLine($"1. Create - Ajouter une donnée");
            Console.WriteLine($"2. Read - Récupérer une ou plusieurs données");
            Console.WriteLine($"3. Update - Mettre à jour une donnée");
            Console.WriteLine($"4. Delete - Supprimer une donnée");
            Console.WriteLine($"0. Retour au menu principal");
            Console.WriteLine();
            Console.Write("Entrez votre choix: ");

            string choix = Console.ReadLine() ?? "";

            switch (choix)
            {
                case "1":
                    DemoCreate(context);
                    break;

                case "2":
                    DemoRead(context);
                    break;

                case "3":
                    DemoUpdate(context);
                    break;

                case "4":
                    DemoDelete(context);
                    break;

                case "0":
                    continuer = false;
                    break;

                default:
                    Console.WriteLine($"Choix invalid. Appuyez sur Enter pour continuer.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void DemoDelete(DataContext context)
    {
        throw new NotImplementedException();
    }

    private static void DemoUpdate(DataContext context)
    {
        throw new NotImplementedException();
    }

    private static void DemoRead(DataContext context)
    {
        Console.Clear();
        Console.WriteLine($"Démonstration sur le Read");

        bool continuer = true;

        while (continuer)
        {
            Console.Clear();
            Console.WriteLine($"1. Afficher tous les films");
            Console.WriteLine($"2. Afficher un film sur base du titre");
            Console.WriteLine($"3. Revenir en arrière");
            Console.WriteLine();
            Console.WriteLine("Choix :");

            string choix = Console.ReadLine() ?? "";

            switch (choix)
            {
                case "1":
                    ReadAll(context);
                    break;

                case "2":
                    ReadByName(context);
                    break;

                case "3":
                    continuer = false;
                    break;

                default:
                    Console.WriteLine($"Erreur, entrée invalide.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void ReadByName(DataContext context)
    {
        Console.Clear();
        Console.WriteLine($"Récupération d'un film sur base de son titre");

        Console.Write($"Entrez le titre d'un film: ");
        var title = Console.ReadLine() ?? "";

        var film = context.Films
            .Where(f => f.Title == title)
            .FirstOrDefault();

        if (film != null)
        {
            Console.WriteLine($"Film trouvé: {film.Title} {film.CreatedAt}");
        }
        else
        {
            Console.WriteLine($"Aucun film trouvé sous le nom de '{title}'");
        }
    }

    private static void ReadAll(DataContext context)
    {
        Console.Clear();
        Console.WriteLine($"Récupération de tous les films");

        //var films = context.Films.Select(f => new { f.Title });

        IQueryable<Film> films = context.Films
            .Select(f => f)
            .OrderByDescending(f => f.ReleasedYear);

        foreach (Film film in films)
        {
            Console.WriteLine($" - {film.Title.PadRight(50)} sorti en {film.ReleasedYear}");
        }

        Console.WriteLine($"Appuyer sur une touche pour continuer...");
        Console.ReadKey();
    }

    private static void DemoCreate(DataContext context)
    {
        Console.Clear();
        Console.WriteLine($"Démonstration sur le Create");

        // Demande à l'utilisateur le nom du nouveau film
        Console.WriteLine($"Entrez le titre du film :");
        string title = Console.ReadLine() ?? "";

        // Demande à l'utilisateur l'année de sortie du nouveau film
        Console.WriteLine($"Entrez l'année de sortie :");
        int releasedYear = 0;

        while (!int.TryParse(Console.ReadLine(), out releasedYear))
        {
            Console.WriteLine($"Entrée invalide.");
        }

        // Création de l'objet C# du nouveau film
        Film film = new Film
        {
            Title = title,
            ReleasedYear = releasedYear
        };

        // Ajout du nouveau film dans la collection côté C#
        context.Films.Add(film);

        // Sauvegarder les changements du côté SQL
        context.SaveChanges();
    }
}
