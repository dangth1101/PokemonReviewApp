namespace PokemonReviewApp.Models
{
    public class Reviewer
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public ICollection<Review> reviews { get; set;}
    }
}
