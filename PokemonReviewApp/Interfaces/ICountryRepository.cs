using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> getCountries();
        Country getCountry(int id);
        Country getCountryByOwner(int id);
        ICollection<Owner> getOwnersFromACountry(int id);
        bool exists(int id);

        bool createCountry(Country country);
        bool updateCountry(Country country);
        bool deleteCountry(Country country);
        bool save();
        
    }
}
