namespace PokemonReviewApp.Models
{
    public class PokemonOwner
    {
        public int pokemonID { get; set; }
        public int ownerID { get; set; }
        public Pokemon pokemon { get; set; }
        public Owner owner { get; set; }
    }
}
