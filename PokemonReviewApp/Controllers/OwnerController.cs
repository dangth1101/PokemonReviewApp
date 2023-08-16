using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository ownerRepository;
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;

        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            this.ownerRepository = ownerRepository;
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult getOwners()
        {
            var owners = mapper.Map<List<OwnerDTO>>(ownerRepository.getOwners());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpGet("{ownerID}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult getOwner(int ownerID)
        {
            if (!ownerRepository.exists(ownerID)) return NotFound();

            var owner = mapper.Map<OwnerDTO>(ownerRepository.getOwner(ownerID));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpGet("{ownerID}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult getPokemonByOwner(int ownerID)
        {
            if (!ownerRepository.exists(ownerID)) return NotFound();

            var pokemon = mapper.Map<List<PokemonDTO>>(ownerRepository.getPokemonByOwner(ownerID));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpGet("owners/{pokemonID}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult getOwnerOfAPokemon(int pokemonID)
        {
            var owners = mapper.Map<List<OwnerDTO>>(ownerRepository.getOwnerOfAPokemon(pokemonID));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createOwner(
            [FromQuery] int countryID,
            [FromBody] OwnerDTO body)
        {
            if (body == null) return BadRequest(ModelState);

            var owner = ownerRepository
                                .getOwners()
                                .Where(c => c.firstName.Trim().ToUpper() == body.firstName.Trim().ToUpper() 
                                            && c.lastName.Trim().ToUpper() == body.lastName.Trim().ToUpper())
                                .FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ownerMap = mapper.Map<Owner>(body);
            ownerMap.country = countryRepository.getCountry(countryID);

            if (!ownerRepository.createOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successful created");
        }

        [HttpPut("{ownerID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult updateOwner(int ownerID, [FromBody] OwnerDTO body)
        {
            if (body == null) return BadRequest(ModelState);

            if (ownerID != body.id) return BadRequest(ModelState);

            if (!countryRepository.exists(ownerID)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ownerMap = mapper.Map<Owner>(body);

            if (!ownerRepository.updateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ownerID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult deleteOwner(int ownerID)
        {
            if (!ownerRepository.exists(ownerID))
            {
                return NotFound();
            }

            var ownerToDelete = ownerRepository.getOwner(ownerID);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!ownerRepository.deleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
