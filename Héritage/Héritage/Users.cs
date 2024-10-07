using Héritage;
using System.Text.Json.Serialization;

internal class Users
{
    public string Password { get; set; }
    public string Name { get; set; }
    public bool IsAdministrator { get; set; }
    public List<Media> BorrowedMedia { get; set; }

    [JsonConstructor]
    public Users(string name, string password, bool isAdministrator)
    {
        Name = name;
        Password = password;
        IsAdministrator = isAdministrator;
        BorrowedMedia = new List<Media>();
    }

    public bool CheckPassword(string password)
    {
        return Password == password;
    }

    public void BorrowMedia(Media media)
    {
        BorrowedMedia.Add(media);
    }

    public void ReturnMedia(Media media)
    {
        BorrowedMedia.Remove(media);
    }

    public void ShowBorrowedMedia()
    {
        foreach (var media in BorrowedMedia)
        {
            string genresString = string.Join(", ", media.Genres);
            Console.WriteLine($"Titre: {media.Title}, Genres: {genresString}");
        }
    }
}

