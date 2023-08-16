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
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository pokemonRepository;
        private readonly IReviewRepository reviewRepository;
        private readonly IMapper mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IReviewRepository reviewRepository, IMapper mapper)
        {
            this.pokemonRepository = pokemonRepository;
            this.reviewRepository = reviewRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult getPokemon()
        {
            var pokemon = mapper.Map<List<PokemonDTO>>(pokemonRepository.getPokemon());

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(pokemon);

        }

        [HttpGet("{pokeID}")]
        [ProducesResponseType(200, Type=typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult getPokemon(int pokeID)
        {
            if (!pokemonRepository.exists(pokeID)) return NotFound();

            var pokemon = mapper.Map<PokemonDTO>(pokemonRepository.getPokemon(pokeID));
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpGet("{pokeID}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult getPokemonRating(int pokeID)
        {
            if (!pokemonRepository.exists(pokeID)) return NotFound();

            var rating = pokemonRepository.getPokemonRating(pokeID);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createPokemon(
            [FromQuery] int ownerID,
            [FromQuery] int categoryID,
            [FromBody] PokemonDTO body)
        {
            if (body == null) return BadRequest(ModelState);

            var pokemon = pokemonRepository.getPokemon().Where(p => p.name.Trim().ToUpper() ==  body.name.Trim().ToUpper()).FirstOrDefault();

            if (pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pokemonMap = mapper.Map<Pokemon>(body);

            if (!pokemonRepository.createPokemon(ownerID, categoryID, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successful save");
        }

        [HttpPut("{pokemonID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult updatePokemon(int pokemonID, [FromBody] PokemonDTO body)
        {
            if (body == null) return BadRequest(ModelState);

            if (pokemonID != body.id) return BadRequest(ModelState);

            if (!pokemonRepository.exists(pokemonID)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pokemonMap = mapper.Map<Pokemon>(body);

            if (!pokemonRepository.updatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{pokemonID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult deleteReview(int pokemonID)
        {
            if (!pokemonRepository.exists(pokemonID))
            {
                return NotFound();
            }

            var pokemonToDelete = pokemonRepository.getPokemon(pokemonID);
            var reviewsToDelete = reviewRepository.getReviewsOfAPokemon(pokemonID).ToList();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!reviewRepository.removeRange(reviewsToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting List");
                return StatusCode(500, ModelState);
            }

            if (!pokemonRepository.deletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting Pokemon");
                return StatusCode(500, ModelState);
            }   

            return NoContent();
        }
    }
}
