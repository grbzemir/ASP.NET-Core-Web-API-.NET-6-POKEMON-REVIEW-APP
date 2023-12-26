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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateCountry([FromBody] CountryDto countryCreate )

        {

            if ( countryCreate == null)

                return BadRequest(ModelState);
            
            var country = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.Trim().ToUpper()).FirstOrDefault();
              
            if (country != null)

            {
                ModelState.AddModelError("", $"Country {countryCreate.Name} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreate);


            if (!_countryRepository.CreateCountry(countryMap))

            {
                ModelState.AddModelError("", $"Something went wrong saving {countryCreate.Name}");
                return StatusCode(500, ModelState);
            }


             return Ok("Succesfully created");


           
            

            return NoContent();
        }
    }
}
