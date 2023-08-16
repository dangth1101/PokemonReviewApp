using Microsoft.IdentityModel.Tokens;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext context;
        public PokemonRepository(DataContext context) { 
            this.context = context;
        }

        public bool createPokemon(int ownerID, int categoryID, Pokemon pokemon)
        {
            var owner = context.owners.Where(o => o.id == ownerID).FirstOrDefault();
            var category = context.categories.Where(c => c.id == categoryID).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                owner = owner,
                pokemon = pokemon,
            };

            context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                category = category,
                pokemon = pokemon,
            };

            context.Add(pokemonCategory);

            context.Add(pokemon);
            return save();
        }

        public bool deletePokemon(Pokemon pokemon)
        {
            context.Remove(pokemon);
            return save();
        }

        public bool exists(int id)
        {
            return context.pokemon.Any(p => p.id == id);
        }

        public ICollection<Pokemon> getPokemon() 
        {
            return context.pokemon.OrderBy(p => p.id).ToList();
        }

        public Pokemon getPokemon(int id)
        {
            return context.pokemon.Where(p => p.id == id).FirstOrDefault();
        }

        public Pokemon getPokemon(string name)
        {
            return context.pokemon.Where(p => p.name == name).FirstOrDefault();
        }

        public decimal getPokemonRating(int id)
        {
            var review = context.reviews.Where(r => r.pokemon.id == id);

            if (review.Count() <= 0) 
                return 0;
            else
                return (decimal) review.Average(r => r.rating);
        }

        public bool save()
        {
            var save = context.SaveChanges() > 0;
            return save;
        }

        public bool updatePokemon(Pokemon pokemon)
        {
            context.Update(pokemon);
            return save();
        }
    }
}
