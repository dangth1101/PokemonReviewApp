using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext context;

        public OwnerRepository(DataContext context)
        {
            this.context = context;
        }

        public bool createOwner(Owner owner)
        {
            context.Add(owner);
            return save();
        }

        public bool deleteOwner(Owner owner)
        {
            context.Remove(owner);
            return save();
        }

        public bool exists(int id)
        {
            return context.owners.Any(o => o.id == id);
        }

        public Owner getOwner(int id)
        {
            return context.owners.Where(o => o.id == id).FirstOrDefault();
        }

        public ICollection<Owner> getOwnerOfAPokemon(int id)
        {
            return context.PokemonOwners.Where(po => po.pokemonID == id).Select(po => po.owner).ToList();
        }

        public ICollection<Owner> getOwners()
        {
            return context.owners.ToList();
        }

        public ICollection<Pokemon> getPokemonByOwner(int id)
        {
            return context.PokemonOwners.Where(po => po.ownerID == id).Select(po => po.pokemon).ToList();
        }

        public bool save()
        {
            var save = context.SaveChanges() > 0;
            return save;
        }

        public bool updateOwner(Owner owner)
        {
            context.Update(owner);
            return save();
        }
    }
}
