using PokemonReviewApp.Models;

namespace PokemonReviewApp.Dto
{
    public class CountryDto
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Owner> Owners { get; set; }
    }
}
