using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using System.Diagnostics.Metrics;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult getCountries()
        {
            var countries = mapper.Map<List<CountryDTO>>(countryRepository.getCountries());

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(countries);
        }

        [HttpGet("{countryID}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult getCountry(int countryID)
        {
            if (!countryRepository.exists(countryID)) return NotFound();

            var country = mapper.Map<CountryDTO>(countryRepository.getCountry(countryID));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(country);

        }

        [HttpGet("owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult getCountryByOwner(int ownerID)
        {
            var country = mapper.Map<CountryDTO>(countryRepository.getCountryByOwner(ownerID));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(country);
        }

        [HttpGet("owners/{countryID}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult getOwnersFromACountry(int countryID)
        {
            if (!countryRepository.exists(countryID)) return NotFound();

            var owners = mapper.Map<List<OwnerDTO>>(countryRepository.getOwnersFromACountry(countryID));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(owners);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createCountry([FromBody] CountryDTO body)
        {
            if (body == null) return BadRequest(ModelState);

            var country = countryRepository
                                .getCountries()
                                .Where(c => c.name.Trim().ToUpper() == body.name.Trim().ToUpper())
                                .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var countryMap = mapper.Map<Country>(body);

            if (!countryRepository.createCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successful created");
        }

        [HttpPut("{countryID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult updateCountry(int countryID, [FromBody] CountryDTO body)
        {
            if (body == null) return BadRequest(ModelState);

            if (countryID != body.id) return BadRequest(ModelState);

            if (!countryRepository.exists(countryID)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var countryMap = mapper.Map<Country>(body);

            if (!countryRepository.updateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{countryID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult deleteCategory(int countryID)
        {
            if (!countryRepository.exists(countryID))
            {
                return NotFound();
            }

            var countryToDelete = countryRepository.getCountry(countryID);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!countryRepository.deleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
