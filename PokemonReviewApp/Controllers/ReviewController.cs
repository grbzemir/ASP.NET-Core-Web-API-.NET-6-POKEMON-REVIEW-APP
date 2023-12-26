using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReviewController : Controller
    {

        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;


        public ReviewController(IReviewRepository reviewRepository , IMapper mapper)
        {

            _reviewRepository = reviewRepository;
            _mapper = mapper;
            
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]

        public IActionResult GetReviews()

        {

            var reviews = _mapper.Map < List < ReviewDto >> (_reviewRepository.GetReviews());

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(404)]

        public IActionResult GetPokemon(int reviewId)

        {

            if(!_reviewRepository.ReviewExists(reviewId))

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

            if(!_reviewRepository.ReviewExists(reviewId))

                return NotFound();

            var review = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)

                return BadRequest(ModelState);


            return Ok(review);

        }



    }

 }