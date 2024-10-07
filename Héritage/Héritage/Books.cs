namespace Héritage
{
    internal class Books : Media
    {
        public string Author { get; set; }
        public int Pages { get; set; }

        public Books(string title, List<Genre> genres, string author, int pages) : base(title, genres)
        {
            Author = author;
            Pages = pages;
        }

        public override string ToString()
        {
            string genresString = string.Join(", ", Genres);
            return $"Titre: {Title}, Auteur: {Author}, Pages: {Pages}, Genres: {genresString}";
        }
    }
}
