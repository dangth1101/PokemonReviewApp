namespace PokemonReviewApp.Models
{
    public class Pokemon
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime birthDay { get; set; }
        public ICollection<Review> reviews { get; set; }
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}
