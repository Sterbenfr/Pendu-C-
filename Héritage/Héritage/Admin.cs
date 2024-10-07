namespace Héritage
{
    internal class Admin : Users
    {
        public Admin(string name, string password) : base(name, password, true) { }

        public void CreateMedia(Library library)
        {
            Console.WriteLine("Quel type de média souhaitez-vous créer ? (1: Livre, 2: DVD)");
            string input = Console.ReadLine();
            int choice;

            if (!int.TryParse(input, out choice))
            {
                Console.WriteLine("Choix invalide.");
                return;
            }

            Console.WriteLine("Entrez le titre du média :");
            string title = Console.ReadLine();

            List<Genre> genres = new List<Genre>();
            Console.WriteLine("Sélectionnez les genres du média (séparés par des virgules) :");
            foreach (var genre in Enum.GetValues(typeof(Genre)))
            {
                Console.WriteLine($"{(int)genre}: {genre}");
            }
            string[] genreInputs = Console.ReadLine().Split(',');
            foreach (var genreInput in genreInputs)
            {
                if (Enum.TryParse(genreInput.Trim(), out Genre genreChoice))
                {
                    genres.Add(genreChoice);
                }
                else
                {
                    Console.WriteLine($"Genre invalide: {genreInput}");
                    return;
                }
            }

            Media media = null;

            if (choice == 1)
            {
                Console.WriteLine("Entrez l'auteur du livre :");
                string author = Console.ReadLine();

                Console.WriteLine("Entrez le nombre de pages :");
                if (int.TryParse(Console.ReadLine(), out int pages))
                {
                    media = new Books(title, genres, author, pages);
                }
                else
                {
                    Console.WriteLine("Nombre de pages invalide.");
                    return;
                }
            }
            else if (choice == 2)
            {
                Console.WriteLine("Entrez le réalisateur du DVD :");
                string director = Console.ReadLine();

                Console.WriteLine("Entrez la durée en minutes :");
                if (int.TryParse(Console.ReadLine(), out int duration))
                {
                    media = new DVD(title, genres, director, duration);
                }
                else
                {
                    Console.WriteLine("Durée invalide.");
                    return;
                }
            }

            if (media != null)
            {
                library.AddMedia(media);
                Console.WriteLine($"{Name} a créé {media.Title}");
            }
            else
            {
                Console.WriteLine("Type de média non valide.");
            }
        }

        public void DeleteMedia(Library library, Media media)
        {
            library.RemoveMedia(media);
            Console.WriteLine($"{Name} a supprimé {media.Title}");
        }
    }
}

