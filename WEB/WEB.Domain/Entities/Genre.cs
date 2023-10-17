namespace WEB.Domain.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public List<Movie> movies { get; set; } = new List<Movie>();

    }
}
