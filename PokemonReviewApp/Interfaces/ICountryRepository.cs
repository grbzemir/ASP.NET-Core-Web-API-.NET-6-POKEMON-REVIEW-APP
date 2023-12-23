using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int countryId);
        Country GetCountryOfOwner(int ownerId);
        ICollection<Owner> GetOwnersFromCountry(int countryId);
        bool CountryExists(int countryId);
       
    }
}
