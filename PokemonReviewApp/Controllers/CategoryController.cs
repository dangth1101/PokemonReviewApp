using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult getCategories()
        {
            var categories = mapper.Map<List<CategoryDTO>>(categoryRepository.getCategories());

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(categories);
        }

        [HttpGet("{categoryID}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult getCategory(int categoryID)
        {
            if (!categoryRepository.exists(categoryID)) return NotFound();

            var category = mapper.Map<CategoryDTO>(categoryRepository.getCategory(categoryID));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(category);
        }

        [HttpGet("pokemon/{categoryID}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult getPokemonByCategory(int categoryID)
        {
            if (!categoryRepository.exists(categoryID)) return NotFound();

            var pokemon = mapper.Map<List<PokemonDTO>>(categoryRepository.getPokemonByCategory(categoryID));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(pokemon);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createCategory([FromBody] CategoryDTO body)
        {
            if (body == null) return BadRequest(ModelState);

            var category = categoryRepository
                                .getCategories()
                                .Where(c => c.name.Trim().ToUpper() == body.name.Trim().ToUpper())
                                .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categoryMap = mapper.Map<Category>(body);

            if (!categoryRepository.createCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successful created");
        }

        [HttpPut("{categoryID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult updateCategory(int categoryID, [FromBody] CategoryDTO body)
        {
            if (body == null) return BadRequest(ModelState);

            if (categoryID != body.id) return BadRequest(ModelState);

            if (!categoryRepository.exists(categoryID)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categoryMap = mapper.Map<Category>(body);

            if (!categoryRepository.updateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult deleteCategory(int categoryID) { 
            if (!categoryRepository.exists(categoryID))
            {
                return NotFound();
            }

            var categoryToDelete = categoryRepository.getCategory(categoryID);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!categoryRepository.deleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
