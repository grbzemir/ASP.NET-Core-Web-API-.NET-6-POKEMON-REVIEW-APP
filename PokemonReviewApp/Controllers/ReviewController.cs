using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReviewController : Controller
    {

        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;


        public ReviewController(IReviewRepository reviewRepository, IMapper mapper , IPokemonRepository  pokemonRepository , IReviewerRepository reviewerRepository)
          {

            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
            
       


        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]

        public IActionResult GetReviews()

        {

            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(404)]

        public IActionResult GetPokemon(int reviewId)

        {

            if (!_reviewRepository.ReviewExists(reviewId))

                return NotFound();

            var review = _mapper.Map<Review>(_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(review);

        }

        [HttpDelete("pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]


        public IActionResult DeleteReview(int reviewId)

        {

            if (!_reviewRepository.ReviewExists(reviewId))

                return NotFound();

            var review = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)

                return BadRequest(ModelState);


            return Ok(review);

        }


        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]


        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId , [FromBody] ReviewDto reviewCreate)

        {

            if (reviewCreate == null)
                return BadRequest(ModelState);

            var reviews = _reviewRepository.GetReviews()
                .Where(c => c.Title.Trim().ToUpper() == reviewCreate.Title.Trim().ToUpper()).FirstOrDefault();


            if (reviews != null)

            {

                ModelState.AddModelError("", $"Owner {reviewCreate.Title} already exists");

                return StatusCode(422, ModelState);


            }

            if (!ModelState.IsValid)

                return BadRequest(ModelState);


            var reviewMap = _mapper.Map<Review>(reviewCreate);

            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);




            if (!_reviewRepository.CreateReview(reviewMap))

            {

                ModelState.AddModelError("", $"Something went wrong saving {reviewMap.Title}");

                return StatusCode(500, ModelState);

            }

            return Ok("Succesfully created");


        }



    }

 }