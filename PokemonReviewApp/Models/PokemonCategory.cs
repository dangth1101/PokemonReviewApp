namespace PokemonReviewApp.Models
{
    public class PokemonCategory
    {
        public int pokemonID { get; set; }
        public int categoryID { get; set; }

        public Pokemon pokemon { get; set; }
        public Category category { get; set; }

    }
}
