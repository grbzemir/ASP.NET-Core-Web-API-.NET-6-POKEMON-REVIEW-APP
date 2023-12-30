using AutoMapper;
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

    public class CategoryController : Controller
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {

            _categoryRepository = categoryRepository;
            _mapper = mapper;

        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]

        public IActionResult GetCategories()

        {

            var categories = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetCategories());
            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(categories);

        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(404)]

        public IActionResult GetCategory(int categoryId)

        {

            if (!_categoryRepository.CategoryExists(categoryId))

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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)

        {

            if (categoryCreate == null)

                return BadRequest(ModelState);


            var category = _categoryRepository.GetCategories().
                Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper()).
                FirstOrDefault();

            if (category != null)

            {

                ModelState.AddModelError("", $"Category {categoryCreate.Name} already exists");

                return StatusCode(422, ModelState);

            }

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryCreate);


            if (!_categoryRepository.CreateCategory(categoryMap))

            {

                ModelState.AddModelError("", $"Something went wrong saving {categoryMap.Name}");

                return StatusCode(500, ModelState);

            }

            return Ok("Succesfuly created");
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updatedCategory) 
        
        {

            if(updatedCategory == null)

                    return BadRequest(ModelState);

            if(categoryId != updatedCategory.Id)

                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(categoryId))

                    return NotFound();

            if(!ModelState.IsValid)

                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(updatedCategory);

            if (!_categoryRepository.UpdateCategory(categoryMap))

            {

                ModelState.AddModelError("", $"Something went wrong updating {categoryMap.Name}");

                return StatusCode(500, ModelState);

            }

            return Ok("Succesfuly updated");
        
        
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult DeleteCategory(int categoryId)

        {

            if (!_categoryRepository.CategoryExists(categoryId))

                return NotFound();

            var category = _categoryRepository.GetCategory(categoryId);

            if (_categoryRepository.GetPokemonByCategory(categoryId).Count() > 0)

            {

                ModelState.AddModelError("", $"Category {category.Name} cannot be deleted because it is used by at least one pokemon");

                return StatusCode(409, ModelState);

            }

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            if (!_categoryRepository.DeleteCategory(category))

            {

                ModelState.AddModelError("", $"Something went wrong deleting {category.Name}");

                return StatusCode(500, ModelState);

            }

            return Ok("Succesfuly deleted");
        }
        

        


    }
}
