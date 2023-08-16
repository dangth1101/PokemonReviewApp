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
    public class ReviewController : Controller
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IReviewerRepository reviewerRepository;
        private readonly IPokemonRepository pokemonRepository;
        private readonly IMapper mapper;

        public ReviewController(IReviewRepository reviewRepository, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository, IMapper mapper) 
        {
            this.reviewRepository = reviewRepository;
            this.reviewerRepository = reviewerRepository;
            this.pokemonRepository = pokemonRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Review>))]
        public IActionResult getReviews()
        {
            var reviews = mapper.Map<List<ReviewDTO>>(reviewRepository.getReviews());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpGet("{reviewID}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult getReview(int reviewID)
        {
            if (!reviewRepository.exists(reviewID)) return NotFound();

            var review = mapper.Map<ReviewDTO>(reviewRepository.getReview(reviewID));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(review);
        }

        [HttpGet("reviews/{pokeID}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult getReviewsOfAPokemon(int pokeID)
        {
            var reviews = mapper.Map<List<ReviewDTO>>(reviewRepository.getReviewsOfAPokemon(pokeID));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createReview(
            [FromQuery] int reviewerID,
            [FromQuery] int pokemonID,
            [FromBody] ReviewDTO body)
        {
            if (body == null) return BadRequest(ModelState);

            var review = reviewRepository
                                .getReviews()
                                .Where(c => c.title.Trim().ToUpper() == body.title.Trim().ToUpper())
                                .FirstOrDefault();

            if (review != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviewMap = mapper.Map<Review>(body);

            reviewMap.reviewer = reviewerRepository.getReviewer(reviewerID);
            reviewMap.pokemon = pokemonRepository.getPokemon(pokemonID);

            if (!reviewRepository.createReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successful created");
        }

        [HttpPut("{reviewID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult updateReview(int reviewID, [FromBody] ReviewDTO body)
        {
            if (body == null) return BadRequest(ModelState);

            if (reviewID != body.id) return BadRequest(ModelState);

            if (!reviewRepository.exists(reviewID)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviewMap = mapper.Map<Review>(body);

            if (!reviewRepository.updateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult deleteReview(int reviewID)
        {
            if (!reviewRepository.exists(reviewID))
            {
                return NotFound();
            }

            var reviewToDelete = reviewRepository.getReview(reviewID);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!reviewRepository.deleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
