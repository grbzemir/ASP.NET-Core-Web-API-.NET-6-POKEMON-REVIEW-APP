using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/controller]")]
    [ApiController]
    public class PokemonController : Controller
    {

        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper ,  IReviewRepository reviewRepository)
        {

            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _reviewRepository = reviewRepository;

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]


        public IActionResult GetPokemons()

        {

            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());



            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(pokemons);

        }

        [HttpGet("{pokeId")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(404)]

        public IActionResult GetPokemons(int pokeId)

        {

            if (!_pokemonRepository.PokemonExists(pokeId))

                return NotFound();

            var pokemon = _mapper.Map<Pokemon>(_pokemonRepository.GetPokemon(pokeId));

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(pokemon);

        }

        [HttpGet("rating/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(404)]

        public IActionResult GetPokemonRating(int pokeId)

        {

            if (!_pokemonRepository.PokemonExists(pokeId))

                return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(pokeId);

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(rating);

        }


        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]


        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId , [FromBody] PokemonDto pokemonCreate)

        {

            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemons = _pokemonRepository.GetPokemons()
                .Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.Trim().ToUpper()).FirstOrDefault();


            if (pokemons != null)

            {

                ModelState.AddModelError("", $"Owner {pokemonCreate.Name} already exists");

                return StatusCode(422, ModelState);


            }

            if (!ModelState.IsValid)

                return BadRequest(ModelState);


            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            

            if (!_pokemonRepository.CreatePokemon(ownerId , categoryId , pokemonMap))

            {

                ModelState.AddModelError("", $"Something went wrong saving {pokemonMap.Name}");

                return StatusCode(500, ModelState);

            }

            return Ok("Succesfully created");


        }

        [HttpPut("{pokemonId}")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult UpdatePokemon([FromQuery] int pokemonId , [FromQuery] int ownerId , [FromQuery] int categoryId , [FromBody] PokemonDto updatePokemon)

        {

            if (updatePokemon == null)

                return BadRequest(ModelState);

            if (pokemonId != updatePokemon.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(pokemonId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemon = _pokemonRepository.GetPokemons()
                .Where(c => c.Name.Trim().ToUpper() == updatePokemon.Name.Trim().ToUpper()).FirstOrDefault();

            if (pokemon != null)

            {
                ModelState.AddModelError("", $"Country {updatePokemon.Name} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(updatePokemon);


            if (!_pokemonRepository.UpdatePokemon(ownerId , categoryId , pokemonMap))

            {
                ModelState.AddModelError("", $"Something went wrong saving");
                return StatusCode(500, ModelState);
            }


            return Ok("Succesfully created");


        }

        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult Deletepokemon(int pokemonId ) 

        {

            if (!_pokemonRepository.PokemonExists(pokemonId))

                return NotFound();

            var pokemon = _pokemonRepository.GetPokemon(pokemonId);

            if (_pokemonRepository.DeletePokemon(pokemon))
            {

                ModelState.AddModelError("", $"Category {pokemon.Name} cannot be deleted because it is used by at least one pokemon");

                return StatusCode(409, ModelState);

            }

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            if (!_pokemonRepository.DeletePokemon(pokemon))

            {

                ModelState.AddModelError("", $"Something went wrong deleting {pokemon.Name}");

                return StatusCode(500, ModelState);

            }

            return Ok("Succesfuly deleted");
       
        }

    }


}


