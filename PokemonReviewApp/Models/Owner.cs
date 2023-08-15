namespace PokemonReviewApp.Models
{
    public class Owner
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gym { get; set; }
        public Country country { get; set; }
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
    }
}
