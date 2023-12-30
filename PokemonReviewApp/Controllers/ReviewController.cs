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

        [HttpGet("{reviewId}")]
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

        [HttpPut("{reviewerId}")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult updateReview(int reviewId, [FromBody] ReviewDto updateReview)

        {

            if (updateReview == null)

                return BadRequest(ModelState);

            if (reviewId != updateReview.Id)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var review = _reviewRepository.GetReviews()
                .Where(c => c.Title.Trim().ToUpper() == updateReview.Title.Trim().ToUpper()).FirstOrDefault();

            if (review != null)

            {
                ModelState.AddModelError("", $"Country {updateReview.Title} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(updateReview);


            if (!_reviewRepository.UpdateReview(reviewMap))

            {
                ModelState.AddModelError("", $"Something went wrong saving");
                return StatusCode(500, ModelState);
            }


            return Ok("Succesfully created");


        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult Deletereview(int reviewId)

        {

            if (!_reviewRepository.ReviewExists(reviewId))

                return NotFound();

            var review = _reviewRepository.GetReview(reviewId);

            if (_reviewRepository.DeleteReview(review))
            {

                ModelState.AddModelError("", $"Category {review.Title} cannot be deleted because it is used by at least one pokemon");

                return StatusCode(409, ModelState);

            }

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(review))

            {

                ModelState.AddModelError("", $"Something went wrong deleting {review.Title}");

                return StatusCode(500, ModelState);

            }

            return Ok("Succesfuly deleted");

        }


    }

 }