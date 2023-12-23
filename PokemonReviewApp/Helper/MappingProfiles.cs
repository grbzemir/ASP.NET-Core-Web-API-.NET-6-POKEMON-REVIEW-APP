using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles: Profile
    {

        public MappingProfiles()

        {

            CreateMap<Pokemon, PokemonDto>();
            CreateMap<Category, Pokemon>();
            CreateMap<Country, CountryDto>();

  
        }

    }
}
