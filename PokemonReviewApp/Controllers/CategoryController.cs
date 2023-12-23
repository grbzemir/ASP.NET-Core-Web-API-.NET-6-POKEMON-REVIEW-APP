﻿using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace PokemonReviewApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController: Controller
    {
        
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository , IMapper mapper)
        {

            _categoryRepository = categoryRepository;
            _mapper = mapper;
            
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]

        public IActionResult GetCategories()

        {

            var categories = _mapper.Map < List < PokemonDto >> (_categoryRepository.GetCategories());
            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(categories);

        }
        
        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(404)]

        public IActionResult GetCategory(int categoryId)

        {

            if(!_categoryRepository.CategoryExists(categoryId))

                return NotFound();

            var category = _mapper.Map<Category>(_categoryRepository.GetCategory(categoryId));

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(category);

        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(404)]

        public IActionResult GetPokemonsByCategoryId(int categoryId)

        {

            var pokemons = _mapper.Map<List<PokemonDto>>
                (_categoryRepository.GetPokemonByCategory(categoryId));

            if (!ModelState.IsValid)

                return BadRequest();

            return Ok(pokemons);

        }
        

        


    }
}
