namespace Héritage
{
    public enum Genre
    {
        Action,
        Comedy,
        Drama,
        Horror,
        SciFi,
        Documentary
    }

    internal class Media
    {
        public string Title { get; set; }
        public int Count { get; set; }
        public List<Genre> Genres { get; set; } 

        public Media(string title, List<Genre> genres, int count = 1)
        {
            Title = title;
            Genres = genres; 
            Count = count;
        }
    }
}
