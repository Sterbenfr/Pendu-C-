using System.Text.Json.Serialization;

namespace Héritage
{
    internal class Library
    {
        [JsonInclude]
        public List<Media> MediaCollection { get; private set; } = new List<Media>();

        public Library() { }

        public void AddMedia(Media media)
        {
            var existingMedia = MediaCollection.Find(m => m.Title.Equals(media.Title, StringComparison.OrdinalIgnoreCase));
            if (existingMedia == null)
            {
                MediaCollection.Add(media);
                Console.WriteLine($"{media.Title} a été ajouté à la médiathèque.");
            }
            else
            {
                existingMedia.Count += media.Count;
                Console.WriteLine($"{media.Count} copies de {media.Title} ont été ajoutées à la médiathèque.");
            }
        }

        public void RemoveMedia(Media media)
        {
            var existingMedia = MediaCollection.Find(m => m.Title.Equals(media.Title, StringComparison.OrdinalIgnoreCase));
            if (existingMedia != null)
            {
                Console.WriteLine("Combien de copies à supprimer ? (vide = toutes)");
                string input = Console.ReadLine();
                int removeCount;
                if (string.IsNullOrEmpty(input))
                {
                    removeCount = existingMedia.Count;
                }
                else if (!int.TryParse(input, out removeCount) || removeCount <= 0)
                {
                    Console.WriteLine("Nombre de copies invalide.");
                    return;
                }

                if (removeCount >= existingMedia.Count)
                {
                    MediaCollection.Remove(existingMedia);
                    Console.WriteLine($"Toutes les copies de {media.Title} ont été supprimées de la médiathèque.");
                }
                else
                {
                    existingMedia.Count -= removeCount;
                    Console.WriteLine($"{removeCount} copies de {media.Title} ont été supprimées de la médiathèque. Restant: {existingMedia.Count}");
                }
            }
            else
            {
                Console.WriteLine($"{media.Title} n'a pas été trouvé dans la médiathèque.");
            }
        }

        public void ListMedia()
        {
            var dvds = MediaCollection.OfType<DVD>().ToList();
            var books = MediaCollection.OfType<Books>().ToList();

            Console.WriteLine("DVDs :");
            foreach (var dvd in dvds)
            {
                Console.WriteLine($"- {dvd.Title} ({dvd.Count} copies)");
            }

            Console.WriteLine("Livres :");
            foreach (var book in books)
            {
                Console.WriteLine($"- {book.Title} ({book.Count} copies)");
            }
        }

        public Media SearchMedia(string title)
        {
            return MediaCollection.Find(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        private bool MediaExists(string title)
        {
            return MediaCollection.Exists(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public bool BorrowMedia(Media media, Users user)
        {
            var existingMedia = MediaCollection.Find(m => m.Title.Equals(media.Title, StringComparison.OrdinalIgnoreCase));
            if (existingMedia != null && existingMedia.Count > 0)
            {
                existingMedia.Count--;
                if (existingMedia.Count == 0)
                {
                    MediaCollection.Remove(existingMedia);
                }
                user.BorrowMedia(media);
                return true;
            }
            else
            {
                Console.WriteLine($"{media.Title} n'est pas disponible dans la médiathèque.");
                return false;
            }
        }

        public void ReturnMedia(Media media, Users user)
        {
            user.ReturnMedia(media);
            var existingMedia = MediaCollection.Find(m => m.Title.Equals(media.Title, StringComparison.OrdinalIgnoreCase));
            if (existingMedia != null)
            {
                existingMedia.Count++;
            }
            else
            {
                MediaCollection.Add(media);
            }
        }

        public void ListMediaByGenre(Genre genre)
        {
            var dvdsByGenre = MediaCollection.OfType<DVD>().Where(m => m.Genres.Contains(genre)).ToList();
            var booksByGenre = MediaCollection.OfType<Books>().Where(m => m.Genres.Contains(genre)).ToList();

            if (dvdsByGenre.Count == 0 && booksByGenre.Count == 0)
            {
                Console.WriteLine($"Aucun média trouvé pour le genre {genre}.");
            }
            else
            {
                if (dvdsByGenre.Count > 0)
                {
                    Console.WriteLine($"DVDs du genre {genre} :");
                    foreach (var dvd in dvdsByGenre)
                    {
                        Console.WriteLine($"- {dvd.Title} ({dvd.Count} copies)");
                    }
                }

                if (booksByGenre.Count > 0)
                {
                    Console.WriteLine($"Livres du genre {genre} :");
                    foreach (var book in booksByGenre)
                    {
                        Console.WriteLine($"- {book.Title} ({book.Count} copies)");
                    }
                }
            }
        }
    }
}


