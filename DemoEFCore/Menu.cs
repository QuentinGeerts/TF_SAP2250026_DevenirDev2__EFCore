


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
        throw new NotImplementedException();
    }

    private static void DemoCreate(DataContext context)
    {
        throw new NotImplementedException();
    }
}
