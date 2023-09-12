namespace WEB.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public Genre Genre { get; set; }
        public double TicketPrice { get; set; }
        public string ImgSrc { get; set; }
    }
}
