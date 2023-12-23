﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/controller]")]
    [ApiController]
    public class PokemonController: Controller
    {

        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository , IMapper mapper)
        {

            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
   
         
        public IActionResult GetPokemons()

        {

            var pokemons = _mapper.Map < List < PokemonDto >> (_pokemonRepository.GetPokemons());
 
         
            
            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(pokemons);

        }

        [HttpGet("{pokeId")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(404)]

        public IActionResult GetPokemons(int pokeId)

        {

            if(!_pokemonRepository.PokemonExists(pokeId))

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

    }
}
