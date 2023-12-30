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
    public class ReviewerController:Controller
    {

        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository , IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
            
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]

        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map < List < ReviewerDto >> (_reviewerRepository.GetReviewers());
           
            if (!ModelState.IsValid)
               
                return BadRequest(ModelState);
            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(404)]

        public IActionResult GetReviewer(int reviewerId)
        {
            if(!_reviewerRepository.ReviewerExists(reviewerId))
               
                return NotFound();
            
            var reviewer = _mapper.Map<Reviewer>(_reviewerRepository.GetReviewer(reviewerId));
            
            if (!ModelState.IsValid)
                
                return BadRequest(ModelState);
           
            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]

        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if(!_reviewerRepository.ReviewerExists(reviewerId))
               
                return NotFound();
            
            var reviews = _mapper.Map < List < ReviewDto >> (_reviewerRepository.GetReviewsByReviewer(reviewerId));
            
            if (!ModelState.IsValid)
                
                return BadRequest(ModelState);
            
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate)

        {

            if (reviewerCreate == null)
                return BadRequest(ModelState);

            var reviewer = _reviewerRepository.GetReviewers()
                .Where(c => c.FirstName.Trim().ToUpper() == reviewerCreate.FirstName.Trim().ToUpper()).FirstOrDefault();

            if (reviewer != null)

            {
                ModelState.AddModelError("", $"Country {reviewerCreate.FirstName} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);


            if (!_reviewerRepository.CreateReviewer(reviewerMap))

            {
                ModelState.AddModelError("", $"Something went wrong saving {reviewerCreate.FirstName}");
                return StatusCode(500, ModelState);
            }


            return Ok("Succesfully created");

    }

}

}


