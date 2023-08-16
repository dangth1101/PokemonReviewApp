using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext context;

        public CountryRepository(DataContext context)
        {
            this.context = context;
        }

        public bool createCountry(Country country)
        {
            context.Add(country);
            return save();
        }

        public bool deleteCountry(Country country)
        {
            context.Remove(country);
            return save();
        }

        public bool exists(int id)
        {
            return context.countries.Any(c => c.id == id);
        }

        public ICollection<Country> getCountries()
        {
            return context.countries.OrderBy(c => c.name).ToList();
        }

        public Country getCountry(int id)
        {
            return context.countries.Where(c => c.id == id).FirstOrDefault();
        }

        public Country getCountryByOwner(int id)
        {
            return context.owners.Where(o => o.id == id).Select(o => o.country).FirstOrDefault();
        }

        public ICollection<Owner> getOwnersFromACountry(int id)
        {
            return context.owners.Where(o => o.country.id == id).ToList();
        }

        public bool save()
        {
            var save = context.SaveChanges() > 0;
            return save;
        }

        public bool updateCountry(Country country)
        {
            context.Update(country);
            return save();
        }
    }
}
