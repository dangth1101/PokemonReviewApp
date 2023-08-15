namespace PokemonReviewApp.Models
{
    public class Review
    {
        public int id { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public int rating { get; set; }
        public Reviewer reviewer { get; set; }
        public Pokemon pokemon { get; set; }
        
    }
}
