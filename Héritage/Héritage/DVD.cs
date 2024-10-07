namespace Héritage
{
    internal class DVD : Media
    {
        public string Director { get; set; }
        public int Duration { get; set; } // Duration in minutes

        public DVD(string title, List<Genre> genres, string director, int duration) : base(title, genres)
        {
            Director = director;
            Duration = duration;
        }

        public override string ToString()
        {
            string genresString = string.Join(", ", Genres);
            return $"Titre: {Title}, Réalisateur: {Director}, Durée: {Duration} minutes, Genres: {genresString}";
        }
    }
}
