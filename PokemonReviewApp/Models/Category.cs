namespace PokemonReviewApp.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}
