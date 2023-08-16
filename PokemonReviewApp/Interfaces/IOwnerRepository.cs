using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> getOwners();
        Owner getOwner(int id);
        ICollection<Pokemon> getPokemonByOwner(int id);
        ICollection<Owner> getOwnerOfAPokemon(int id);
        bool exists(int id);
        bool createOwner(Owner owner);
        bool updateOwner(Owner owner);
        bool deleteOwner(Owner owner);
        bool save();
    }
}
