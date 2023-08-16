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
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository reviewerRepository;
        private readonly IMapper mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            this.reviewerRepository = reviewerRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult getReviewers()
        {
            var reviewers = mapper.Map<List<ReviewerDTO>>(reviewerRepository.getReviewers());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(reviewers);
        }

        [HttpGet("{reviewerID}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult getReviewer(int reviewerID)
        {
            if (!reviewerRepository.exists(reviewerID)) return NotFound();

            var reviewer = mapper.Map<ReviewerDTO>(reviewerRepository.getReviewer(reviewerID));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(reviewer);
        }

        [HttpGet("{reviewerID}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult getReviewsByReviewer(int reviewerID)
        {
            if (!reviewerRepository.exists(reviewerID)) return NotFound();

            var reviews = mapper.Map<List<ReviewDTO>>(reviewerRepository.getReviewsByReviewer(reviewerID));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createReviewer([FromBody] ReviewerDTO body)
        {
            if (body == null) return BadRequest(ModelState);

            var reviewer = reviewerRepository
                                .getReviewers()
                                .Where(r => r.lastName.Trim().ToUpper() == body.lastName.Trim().ToUpper())
                                .FirstOrDefault();

            if (reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviewerMap = mapper.Map<Reviewer>(body);

            if (!reviewerRepository.createReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successful created");
        }

        [HttpPut("{reviewerID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult updateReview(int reviewerID, [FromBody] ReviewerDTO body)
        {
            if (body == null) return BadRequest(ModelState);

            if (reviewerID != body.id) return BadRequest(ModelState);

            if (!reviewerRepository.exists(reviewerID)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviewerMap = mapper.Map<Reviewer>(body);

            if (!reviewerRepository.updateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewerID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult deleteReviewer(int reviewerID)
        {
            if (!reviewerRepository.exists(reviewerID))
            {
                return NotFound();
            }

            var reviewerToDelete = reviewerRepository.getReviewer(reviewerID);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!reviewerRepository.deleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
