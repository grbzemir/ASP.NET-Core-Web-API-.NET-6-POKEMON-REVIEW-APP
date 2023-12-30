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
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository,
            ICountryRepository CountryRepository
            , IMapper mapper)


        {

            _ownerRepository = ownerRepository;
            _countryRepository = CountryRepository;
            _mapper = mapper;


        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]

        public IActionResult GetOwners()

        {

            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(owners);


        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(404)]

        public IActionResult GetOwners(int ownerId)

        {

            if (!_ownerRepository.OwnerExists(ownerId))

                return NotFound();

            var owner = _mapper.Map<Owner>(_ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(owner);

        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(404)]

        public IActionResult GetPokemonByOwner(int ownerId)

        {

            if (!_ownerRepository.OwnerExists(ownerId))

            {

                return NotFound();
            }

            var owner = _mapper.Map<Owner>(_ownerRepository.GetPokemonsByOwner(ownerId));

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            return Ok(owner);

        }


        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]


        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerCreate)

        {

            if (ownerCreate == null)

                return BadRequest(ModelState);

            var owners = _ownerRepository.GetOwners()
                .Where(c => c.Name.Trim().ToUpper() == ownerCreate.LastName.Trim().ToUpper()).FirstOrDefault();


            if (owners != null)

            {

                ModelState.AddModelError("", $"Owner {ownerCreate.LastName} already exists");

                return StatusCode(422, ModelState);


            }

            if (!ModelState.IsValid)

                return BadRequest(ModelState);


            var OwnerMap = _mapper.Map<Owner>(ownerCreate);

            OwnerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(OwnerMap))

            {

                ModelState.AddModelError("", $"Something went wrong saving {OwnerMap.Name}");

                return StatusCode(500, ModelState);

            }

            return Ok("Succesfully created");








        }


        [HttpPut("{ownerId}")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult UpdateOwner([FromBody] OwnerDto updateOwner)

        {

            if (updateOwner == null)

                return BadRequest(ModelState);

            var owner = _ownerRepository.GetOwners()
                .Where(c => c.Name.Trim().ToUpper() == updateOwner.FirstName.Trim().ToUpper()).FirstOrDefault();

            if (owner != null)

            {
                ModelState.AddModelError("", $"Country {updateOwner.FirstName} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(updateOwner);


            if (!_ownerRepository.UpdateOwner(ownerMap))

            {
                ModelState.AddModelError("", $"Something went wrong saving");
                return StatusCode(500, ModelState);
            }


            return Ok("Succesfully created");


        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult DeleteOwner(int ownerId)

        {

            if (!_ownerRepository.OwnerExists(ownerId))

                return NotFound();

            var owner = _ownerRepository.GetOwner(ownerId);

            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            if (!_ownerRepository.DeleteOwner(ownerId))

            {
                ModelState.AddModelError("", $"Something went wrong saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully created");

        }
    }

}
