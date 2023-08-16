using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> getPokemon();
        Pokemon getPokemon(int id);
        Pokemon getPokemon(string name);
        decimal getPokemonRating(int id);
        bool exists(int id);
        bool createPokemon(int ownerID, int categoryID, Pokemon pokemon);
        bool updatePokemon(Pokemon pokemon);
        bool deletePokemon(Pokemon pokemon);
        bool save();
    }
}
