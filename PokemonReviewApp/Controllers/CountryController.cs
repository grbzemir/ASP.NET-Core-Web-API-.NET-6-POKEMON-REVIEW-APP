using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace PokemonReviewApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
           private readonly ICountryRepository _countryRepository;
            private readonly IMapper _mapper;
    
            public CountryController(ICountryRepository countryRepository , IMapper mapper)
           
        {
    
                _countryRepository = countryRepository;
                _mapper = mapper;
                
         }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]

        public IActionResult GetCountries()

        {

            var countries = _mapper.Map < List < CountryDto >> (_countryRepository.GetCountries());
            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(countries);

        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(404)]

        public IActionResult GetCountry(int countryId)

        {

            if(!_countryRepository.CountryExists(countryId))

                return NotFound();

            var country = _mapper.Map<Country>(_countryRepository.GetCountry(countryId));

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(country);

        }

        [HttpGet("owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(404)]

        public IActionResult GetCountryOfOwner(int ownerId)

        {

            if(!_countryRepository.CountryExists(ownerId))

                return NotFound();

            var country = _mapper.Map<Country>(_countryRepository.GetCountryOfOwner(ownerId));

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(country);

        }
    }
}
