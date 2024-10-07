using System.Text.Json;
using System.Text.Json.Serialization;

namespace Héritage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Library library = LoadLibrary();
            List<Users> users = LoadUsers();

            bool running = true;

            while (running)
            {
                Console.WriteLine("Choisissez une option :");
                Console.WriteLine("1: Se connecter");
                Console.WriteLine("2: Créer un nouvel utilisateur");
                Console.WriteLine("3: Quitter");

                string? initialChoice = Console.ReadLine();
                if (initialChoice == null)
                {
                    Console.WriteLine("Choix non valide.");
                    continue;
                }

                switch (initialChoice)
                {
                    case "1":
                        Users? currentUser = Login(users);
                        if (currentUser == null)
                        {
                            Console.WriteLine("Connexion échouée. Réessayez.");
                            continue;
                        }

                        bool loggedIn = true;
                        while (loggedIn)
                        {
                            Console.WriteLine("Choisissez une action :");
                            Console.WriteLine("1: Lister les médias");
                            Console.WriteLine("2: Rechercher un média");
                            Console.WriteLine("3: Lister les médias par genre"); // Nouvelle option
                            if (currentUser.IsAdministrator)
                            {
                                Console.WriteLine("4: Ajouter un média");
                                Console.WriteLine("5: Supprimer un média");
                                Console.WriteLine("6: Créer un nouvel utilisateur");
                            }
                            Console.WriteLine("7: Emprunter un média");
                            Console.WriteLine("8: Retourner un média");
                            Console.WriteLine("9: Afficher les médias empruntés");
                            Console.WriteLine("10: Se déconnecter");
                            Console.WriteLine("11: Quitter");

                            string? choice = Console.ReadLine();
                            if (choice == null)
                            {
                                Console.WriteLine("Choix non valide.");
                                continue;
                            }

                            switch (choice)
                            {
                                case "1":
                                    library.ListMedia();
                                    break;
                                case "2":
                                    SearchMedia(library);
                                    break;
                                case "3":
                                    ListMediaByGenre(library);
                                    break;
                                case "4":
                                    if (currentUser.IsAdministrator)
                                    {
                                        Admin? adminUser = currentUser as Admin;
                                        if (adminUser != null)
                                        {
                                            adminUser.CreateMedia(library);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Erreur : l'utilisateur actuel n'est pas un administrateur.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Permission refusée.");
                                    }
                                    break;
                                case "5":
                                    if (currentUser.IsAdministrator)
                                    {
                                        DeleteMedia(library, currentUser as Admin);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Permission refusée.");
                                    }
                                    break;
                                case "6":
                                    if (currentUser.IsAdministrator)
                                    {
                                        AdminCreateUser(users);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Permission refusée.");
                                    }
                                    break;
                                case "7":
                                    BorrowMedia(library, currentUser);
                                    break;
                                case "8":
                                    ReturnMedia(library, currentUser);
                                    break;
                                case "9":
                                    currentUser.ShowBorrowedMedia();
                                    break;
                                case "10":
                                    loggedIn = false;
                                    break;
                                case "11":
                                    loggedIn = false;
                                    running = false;
                                    break;
                                default:
                                    Console.WriteLine("Choix non valide.");
                                    break;
                            }
                        }
                        break;
                    case "2":
                        CreateUser(users);
                        break;
                    case "3":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Choix non valide.");
                        break;
                }
            }

            SaveUsers(users);
            SaveLibrary(library);
        }

        static Users? Login(List<Users> users)
        {
            Console.WriteLine("Entrez votre nom d'utilisateur :");
            string? name = Console.ReadLine();
            Console.WriteLine("Entrez votre mot de passe :");
            string? password = Console.ReadLine();

            if (name == null || password == null)
            {
                return null;
            }

            foreach (var user in users)
            {
                if (user.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && user.CheckPassword(password))
                {
                    return user;
                }
            }
            return null;
        }

        static void CreateUser(List<Users> users)
        {
            Console.WriteLine("Entrez le nom d'utilisateur :");
            string? name = Console.ReadLine();
            Console.WriteLine("Entrez le mot de passe :");
            string? password = Console.ReadLine();

            if (name == null || password == null)
            {
                Console.WriteLine("Nom d'utilisateur ou mot de passe invalide.");
                return;
            }

            users.Add(new Users(name, password, false));

            Console.WriteLine("Utilisateur créé avec succès.");
        }

        static void AdminCreateUser(List<Users> users)
        {
            Console.WriteLine("Entrez le nom d'utilisateur :");
            string? name = Console.ReadLine();
            Console.WriteLine("Entrez le mot de passe :");
            string? password = Console.ReadLine();
            Console.WriteLine("Est-ce un administrateur ? (oui/non) :");
            string? isAdminInput = Console.ReadLine();

            if (name == null || password == null || isAdminInput == null)
            {
                Console.WriteLine("Entrée invalide.");
                return;
            }

            bool isAdmin = isAdminInput.Equals("oui", StringComparison.OrdinalIgnoreCase);

            if (isAdmin)
            {
                users.Add(new Admin(name, password));
            }
            else
            {
                users.Add(new Users(name, password, false));
            }

            Console.WriteLine("Utilisateur créé avec succès.");
        }

        static void SearchMedia(Library library)
        {
            Console.WriteLine("Entrez le titre du média à rechercher :");
            string? searchTitle = Console.ReadLine();
            if (searchTitle == null)
            {
                Console.WriteLine("Titre invalide.");
                return;
            }
            Media? foundMedia = library.SearchMedia(searchTitle);
            if (foundMedia != null)
            {
                Console.WriteLine($"{foundMedia.Title} a été trouvé dans la médiathèque.");
                Console.WriteLine(foundMedia.ToString());
            }
            else
            {
                Console.WriteLine($"{searchTitle} n'a pas été trouvé dans la médiathèque.");
            }
        }

        static void DeleteMedia(Library library, Admin? admin)
        {
            if (admin == null)
            {
                Console.WriteLine("Permission refusée.");
                return;
            }

            Console.WriteLine("Entrez le titre du média à supprimer :");
            string? title = Console.ReadLine();
            if (title == null)
            {
                Console.WriteLine("Titre invalide.");
                return;
            }
            Media? media = library.SearchMedia(title);
            if (media != null)
            {
                admin.DeleteMedia(library, media);
            }
            else
            {
                Console.WriteLine($"{title} n'a pas été trouvé dans la médiathèque.");
            }
        }

        static void BorrowMedia(Library library, Users user)
        {
            Console.WriteLine("Entrez le titre du média à emprunter :");
            string? title = Console.ReadLine();
            if (title == null)
            {
                Console.WriteLine("Titre invalide.");
                return;
            }
            Media? media = library.SearchMedia(title);
            if (media != null)
            {
                bool success = library.BorrowMedia(media, user);
                if (success)
                {
                    Console.WriteLine($"{title} a été emprunté avec succès.");
                }
            }
            else
            {
                Console.WriteLine($"{title} n'a pas été trouvé dans la médiathèque.");
            }
        }

        static void ReturnMedia(Library library, Users user)
        {
            Console.WriteLine("Entrez le titre du média à retourner :");
            string? title = Console.ReadLine();
            if (title == null)
            {
                Console.WriteLine("Titre invalide.");
                return;
            }
            Media? media = user.BorrowedMedia.Find(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (media != null)
            {
                library.ReturnMedia(media, user);
            }
            else
            {
                Console.WriteLine($"{title} n'a pas été trouvé dans la médiathèque.");
            }
        }

        static void ListMediaByGenre(Library library)
        {
            Console.WriteLine("Entrez le genre du média à lister :");
            string? genreInput = Console.ReadLine();
            if (genreInput == null || !Enum.TryParse<Genre>(genreInput, true, out Genre genre))
            {
                Console.WriteLine("Genre invalide.");
                return;
            }
            library.ListMediaByGenre(genre);
        }

        static Library LoadLibrary()
        {
            if (!File.Exists("library.json"))
            {
                return new Library();
            }

            string json = File.ReadAllText("library.json");
            var options = new JsonSerializerOptions();
            options.Converters.Add(new MediaJsonConverter());
            return JsonSerializer.Deserialize<Library>(json, options) ?? new Library();
        }


        static List<Users> LoadUsers()
        {
            if (!File.Exists("users.json"))
            {
                return new List<Users>();
            }

            string json = File.ReadAllText("users.json");
            List<Users>? deserializedUsers = JsonSerializer.Deserialize<List<Users>>(json);

            if (deserializedUsers == null)
            {
                return new List<Users>();
            }

            List<Users> users = new List<Users>();
            foreach (var user in deserializedUsers)
            {
                if (user.IsAdministrator)
                {
                    users.Add(new Admin(user.Name, user.Password));
                }
                else
                {
                    users.Add(new Users(user.Name, user.Password, user.IsAdministrator));
                }
            }

            return users;
        }

        static void SaveLibrary(Library library)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            options.Converters.Add(new MediaJsonConverter());
            string json = JsonSerializer.Serialize(library, options);
            File.WriteAllText("library.json", json);
        }


        static void SaveUsers(List<Users> users)
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("users.json", json);
        }
    }
}
